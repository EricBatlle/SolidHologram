//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class DrawScreen : NetworkBehaviour
//{

//    //CONSTANTS
//    private static readonly Vector3 forwardCameraDistance = Vector3.forward * 5;

//    //VARIABLES
//    Vector3 newVertex;
//    Vector3 lastVertex;

//    //ENUM-dropdownMenu
//    public enum PhysicsType
//    {
//        Dynamic,
//        Static,
//        Kinematic
//    }
//    RigidbodyType2D getBodyType(PhysicsType pt)
//    {
//        if (pt == PhysicsType.Dynamic)
//        { return RigidbodyType2D.Dynamic; }
//        else if (pt == PhysicsType.Static)
//        { return RigidbodyType2D.Static; }
//        else if (pt == PhysicsType.Kinematic)
//        { return RigidbodyType2D.Kinematic; }
//        return RigidbodyType2D.Dynamic;
//    }
//    public PhysicsType bodyType;

//    //OTHER PARAMETERS
//    public bool usePhysics = true;

//    public float colliderThickness = 0.1f;
//    public float vertexPrecision = 0.0001f;
//    public float mass = 10.0f;

//    public float deathTime = 0;
//    public float drawTime = 3;

//    public Shader shader;
//    public Color color;

//    //SPHERE TRYS
//    [SyncVar]
//    Vector3 sphereX = new Vector3(0,0,0);

//    Vector3 incrementX= new Vector3(1, 0, 0);

//    // Use this for initialization
//    void Start()
//    {
//		/*
//        if (isServer)
//        {
//            //Destroy(this);
//            return;
//        }
//        */
//    }

//    // Update is called once per frame
//    void Update()
//    {
//		if (!isServer) {
//			return;
//		} 
//		else {
//			if (Input.GetMouseButtonDown(0))
//			{
//				print ("check mouse button");
//				//CmdAddSphere ();
//                //StartCoroutine( DrawLine());
//                //StartCoroutine(CmdDrawLine());
//                CmdCoroutine();
//                print(sphereX);
//			}
            
//        }

//    }

//    [Command]
//    void CmdAddSphere()
//    {
//        GameObject sphere = (GameObject)Instantiate(Resources.Load("SphereEric"));
//        NetworkServer.Spawn(sphere);
//    }

//    bool isDrawableSurface()
//	{
//		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);


//		if( (hit.collider != null) )
//		{
//			//Has collide
//			if(hit.collider.gameObject.GetComponent<isDrawable>() != null) {
//				return true;
//			}
//			Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
//			Debug.Log ("Target Name: " + hit.collider.gameObject.name);
//			return false;
//		}
//		return true;
//	}

//	[Command]
//	void CmdCoroutine(){
//        //GameObject lro = (GameObject)Instantiate(Resources.Load("LineRendererObject"));
//        GameObject lro = (GameObject)Instantiate(Resources.Load("LRO"));
//        NetworkServer.Spawn(lro);
//    }

//	IEnumerator DrawLine()
//	{
//		print ("drawLinenormalito");
//        float drawTimer = 0;

//		GameObject lro = (GameObject)Instantiate (Resources.Load ("LineRendererObject"));
//		NetworkServer.Spawn (lro);
//		LineRenderer lr = lro.GetComponent<LineRenderer> ();

//        //LineRenderer lr = new GameObject().AddComponent<LineRenderer>();
//        lr.transform.SetParent(transform);//Set the line renderer new objects as a child of the object who calls the script
//        lr.startWidth = 0.1f;
//        lr.endWidth = 0.1f;
//        lr.material = new Material(shader);
//        lr.material.color = color;
//        lr.useWorldSpace = false; //Set positions to relative, in this case, to the parent object --> the camera
//        lr.tag = "Draw";
//        List<Vector3> positionsLine = new List<Vector3>();

//        //Can't be ButtonDown, cause it only will works for a single frame
//		while ( (drawTimer < drawTime) && (Input.GetMouseButton(0)) && (isDrawableSurface()) )
//		//while ( (drawTimer < drawTime) && (Input.GetMouseButton(0)))
//		{
//			//Trying to deny drawing over objects with physics.
//			//OnMouseEnter();
//			//
//            drawTimer += Time.deltaTime;
//			//ToDo:
		
//			//Si pots dibuixar
//			//Te quedas la pos para mirar el siguiente loop
//			//SINO
//			//No draw el vertex i dibuixar cap a la pared on has colisionat respecte a la última posició válida

//            newVertex = Camera.main.ScreenToWorldPoint(Input.mousePosition) + forwardCameraDistance;
//            if (Vector3.Distance(lastVertex, newVertex) >= vertexPrecision) //This improve line quality
//            {
//                //At every moment we will add the mouse position to the list "positionsLine" plus some distance to do not draw JUST in camera plane
//                positionsLine.Add(newVertex);

//                //Construct the line render with those positinos
//                lr.positionCount = positionsLine.Count; //equalize the num of vertex with the num of positions to do not overflow
//                lr.SetPositions(positionsLine.ToArray());

//                lastVertex = newVertex;
//            }
//            yield return new WaitForSeconds(0);

//        }

//        //If the object have physics, apply them
//        if (usePhysics)
//        {
//            //Adding collider and physics to the draw
//            List<Vector2> positionsCollider = new List<Vector2>();

//            for (int i = 0; i < positionsLine.Count; i++)
//            {
//                positionsCollider.Add(new Vector2(positionsLine[i].x, positionsLine[i].y));
//            }
//            //To allow convex problems
//            for (int i = positionsLine.Count - 1; i > 0; i--)
//            {
//                positionsCollider.Add(new Vector2(positionsLine[i].x, positionsLine[i].y + colliderThickness));
//            }

//            PolygonCollider2D collider = lr.gameObject.AddComponent<PolygonCollider2D>();

//            collider.points = positionsCollider.ToArray();
//            collider.gameObject.AddComponent<Rigidbody2D>();

//            //Changing properties
//            lr.GetComponent<Rigidbody2D>().mass = mass;
//            lr.GetComponent<Rigidbody2D>().bodyType = getBodyType(bodyType);

//        }

//        //Auto-destruction object
//        if ((deathTime != 0) && (deathTime > 0))
//        {
//            Destroy(lr.gameObject, deathTime);
//        }

//		//NetworkServer.Spawn (lro);
//    }


//	//Create a LineRenderer and draws where mouse is moved. It also applies the physiscs to the draw
//	//[Command]
//	IEnumerator CmdDrawLine()
//	{
//		print ("drawLineComandoso");
//		float drawTimer = 0;

//		GameObject lro = (GameObject)Instantiate (Resources.Load ("LineRendererObject"));
//		NetworkServer.Spawn (lro);
//		LineRenderer lr = lro.GetComponent<LineRenderer> ();

//		//LineRenderer lr = new GameObject().AddComponent<LineRenderer>();
//		lr.transform.SetParent(transform);//Set the line renderer new objects as a child of the object who calls the script
//		lr.startWidth = 0.1f;
//		lr.endWidth = 0.1f;
//		lr.material = new Material(shader);
//		lr.material.color = color;
//		lr.useWorldSpace = false; //Set positions to relative, in this case, to the parent object --> the camera
//		lr.tag = "Draw";
//		List<Vector3> positionsLine = new List<Vector3>();

//		//Can't be ButtonDown, cause it only will works for a single frame
//		while ((drawTimer < drawTime) && (Input.GetMouseButton(0)))
//		{
//			drawTimer += Time.deltaTime;
//			newVertex = Camera.main.ScreenToWorldPoint(Input.mousePosition) + forwardCameraDistance;
//			if (Vector3.Distance(lastVertex, newVertex) >= vertexPrecision) //This improve line quality
//			{
//				//At every moment we will add the mouse position to the list "positionsLine" plus some distance to do not draw JUST in camera plane
//				positionsLine.Add(newVertex);

//				//Construct the line render with those positinos
//				lr.positionCount = positionsLine.Count; //equalize the num of vertex with the num of positions to do not overflow
//				lr.SetPositions(positionsLine.ToArray());

//				lastVertex = newVertex;
//			}
//			yield return new WaitForSeconds(0);

//		}

//		//If the object have physics, apply them
//		if (usePhysics)
//		{
//			//Adding collider and physics to the draw
//			List<Vector2> positionsCollider = new List<Vector2>();

//			for (int i = 0; i < positionsLine.Count; i++)
//			{
//				positionsCollider.Add(new Vector2(positionsLine[i].x, positionsLine[i].y));
//			}
//			//To allow convex problems
//			for (int i = positionsLine.Count - 1; i > 0; i--)
//			{
//				positionsCollider.Add(new Vector2(positionsLine[i].x, positionsLine[i].y + colliderThickness));
//			}

//			PolygonCollider2D collider = lr.gameObject.AddComponent<PolygonCollider2D>();

//			collider.points = positionsCollider.ToArray();
//			collider.gameObject.AddComponent<Rigidbody2D>();

//			//Changing properties
//			lr.GetComponent<Rigidbody2D>().mass = mass;
//			lr.GetComponent<Rigidbody2D>().bodyType = getBodyType(bodyType);

//		}

//		//Auto-destruction object
//		if ((deathTime != 0) && (deathTime > 0))
//		{
//			Destroy(lr.gameObject, deathTime);
//		}
//	}

//}