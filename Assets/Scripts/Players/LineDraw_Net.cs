using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LineDraw_Net : NetworkBehaviour
{
    //PREFAB DRAW
    [SerializeField] GameObject lineObjectPrefab; //this needs to be the line prefab in the assets folder

    //ENUM-dropdownMenu+PHYSICS
    public bool usePhysics = true;
	public bool useAutoMass = true;
    public enum PhysicsType
    {
        Dynamic,
        Static,
        Kinematic
    }
    RigidbodyType2D getBodyType(PhysicsType pt)
    {
        if (pt == PhysicsType.Dynamic)
        { return RigidbodyType2D.Dynamic; }
        else if (pt == PhysicsType.Static)
        { return RigidbodyType2D.Static; }
        else if (pt == PhysicsType.Kinematic)
        { return RigidbodyType2D.Kinematic; }
        return RigidbodyType2D.Dynamic;
    }
    public PhysicsType bodyType;
    public float colliderThickness = 0.1f;
    [SerializeField] float pointInterval = 3f; //the maximum distance in pixels between each point of line
    public float mass = 10.0f;
	public float density = 75.0f;

    //OTHER CUSTOM PARAMETERS
    public Shader shader;
    public Color color;
    public float startWidth = 0.1f; //Try to maintain the ratio of start-endWitdh...
    public float endWidth = 0.1f;  //or the collider will be in troubles!
    
    public float deathTime = 0;
    public float drawTime = 3; // 0 == inf

    //GLOBAL VARIABLES
    Vector3 oldMousePos; //we store the mouse position when user first clicks
    List<Vector3> positionsLine = new List<Vector3>();
    private float drawTimer;

    // Update is called once per frame
    void Update()
    {
        //only the server can draw
        if (!isServer)
            return;

        //when user first clicks mouse
        if (Input.GetMouseButtonDown(0))
        {
            //get mouse screen position and store it
            Vector3 mp = Input.mousePosition;
            oldMousePos = mp;

            //get world position and reset z coord so it's same as characters
            Vector3 mwc = Camera.main.ScreenToWorldPoint(mp);
            mwc.z = 0;

            //spawn new line object on server and all clients
            CmdMakeNewLine(mwc);
            drawTimer = 0;
        }

        //if mouse button is down, and the draw restrictions are true
		if (Input.GetMouseButton(0) && ((drawTime == 0) || (drawTimer <= drawTime)) && (isDrawableSurface()))
        {
            Vector3 mp = Input.mousePosition;
            //if we have dragged mouse more than pointInterval pixels
            if (Vector3.Distance(mp, oldMousePos) > pointInterval)
            {
                //get mouse world coords
                Vector3 mwc = Camera.main.ScreenToWorldPoint(mp);
                mwc.z = 0;

                //update line on all clients
                RpcUpdateLine(mwc);
            }
            drawTimer += Time.deltaTime;
        }

        //if mouse button is up
        if (Input.GetMouseButtonUp(0))
        {
            if (usePhysics == true)
            {
                //Add collider to the lr
                RpcUpdateLineCollider();
            }
        }
    }

    [Command]
    void CmdMakeNewLine(Vector3 mouseWorldCoords)
    {
        //find any/all lines and destroy them
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("line");
        foreach (GameObject td in toDestroy)
        {
            NetworkServer.Destroy(td);
        }
		

        //create a new line object. NOTE: NetworkServer.Spawn spawns a default copy of the gameobject
        //if we change any properties here they will NOT be sent to client
        GameObject instance = Instantiate(lineObjectPrefab, mouseWorldCoords, Quaternion.identity) as GameObject;

        NetworkServer.Spawn(instance);

    }

    [ClientRpc]
    void RpcUpdateLine(Vector3 newPosWorld)
    {
        //find the line gameobject (there should be only one)
        GameObject lineObject = GameObject.FindWithTag("line");
        //get line renderer component
        LineRenderer lr = lineObject.GetComponent<LineRenderer>();
        
        //Auto-destruction object
        if ((deathTime != 0) && (deathTime > 0))
        {
            Destroy(lr.gameObject, deathTime);
        }

        //ToDo: Move this out of the updateLineLoop
        //Custom LR settings
        lr.transform.SetParent(transform);//Set the line renderer new objects as a child of the object who calls the script
        lr.material = new Material(shader);
        lr.material.color = color;
        lr.startWidth = startWidth;
        lr.endWidth = endWidth;
        //instance.gameObject.GetComponent<LineRenderer>().useWorldSpace = false;//Set positions to relative, in this case, to the parent object --> the camera

        // the line renderer's points are set to 'local', not world.
        // so we have to transform the world position of new point in local coords
        Vector3 newPosLocal = lineObject.transform.InverseTransformPoint(newPosWorld);

        //add the new position to line renderer
        positionsLine.Add(newPosLocal);

        //lr.positionCount++;
        lr.positionCount = positionsLine.Count;
        lr.SetPosition(lr.positionCount - 1, newPosLocal);
    }

    [ClientRpc]
    void RpcUpdateLineCollider()
    {
        GameObject lineObject = GameObject.FindWithTag("line");
        //get line renderer component
        LineRenderer lr = lineObject.GetComponent<LineRenderer>();
        List<Vector2> positionsCollider = new List<Vector2>();

        for (int i = 0; i < positionsLine.Count; i++)
        {
            positionsCollider.Add(new Vector2(positionsLine[i].x, positionsLine[i].y - colliderThickness/2));
        }
        //To allow convex problems
        for (int i = positionsLine.Count - 1; i >= 0; i--)
        {
            positionsCollider.Add(new Vector2(positionsLine[i].x, positionsLine[i].y + colliderThickness/2));
        }
        PolygonCollider2D collider = lr.gameObject.AddComponent<PolygonCollider2D>();
        collider.points = positionsCollider.ToArray();

        //RigidBody Properties
		lr.GetComponent<Rigidbody2D>().useAutoMass = useAutoMass;
		if(useAutoMass == true){
			collider.density = density;
		}else{
			lr.GetComponent<Rigidbody2D>().mass = mass;
		}
        lr.GetComponent<Rigidbody2D>().bodyType = getBodyType(bodyType);
		
		positionsLine.Clear();

    }

    bool isDrawableSurface()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);


        if ((hit.collider != null))
        {
            //Has collider and is Drawable
			//Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
			//Debug.Log("Target Name: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponent<isDrawable>() != null)
            {
                return true;
            }
            //Has collider but is not Drawable
            
            return false;
        }
        return true;
    }

}
