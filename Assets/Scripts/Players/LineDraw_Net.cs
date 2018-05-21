using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityStandardAssets._2D;

public class LineDraw_Net : NetworkInteractiveObject
{
    #region variables
    //PLAYER INFORMATION
    [Header("Player Information")]
    [SerializeField]
    PlayerInfo playerInfo; //Information about the current player

    [Header("HUD Interaction")]
    public DrawButton[] hudButtons;
    public DrawButton messageDrawButton;
    public DrawButton normalDrawButton;


    //PREFAB DRAW
    [Header("Draw Prefab")]
    [SerializeField]
    GameObject lineObjectPrefab; //this needs to be the line prefab in the assets folder
    private Color color;
    public Color messageColor;
    public Color normalColor;

    public Shader shader;

    //ENUM-dropdownMenu+PHYSICS
    [Header("Physics Parameters")]
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
    public float pointInterval = 3f; //the maximum distance in pixels between each point of line
    [Tooltip("Ideally 10 to avoid to move the draw")]
    public float mass = 10.0f;
    public float density = 75.0f;

    //OTHER CUSTOM PARAMETERS
    [Header("Gameplay Parameters")]
    public float startWidth = 0.1f; //Try to maintain the ratio of start-endWitdh...
    public float endWidth = 0.1f;   //or the collider will be in troubles!
    public float deathTime = 0;
    public float drawTime = 3;      // 0 == inf

    //GLOBAL VARIABLES
    Vector3 oldMousePos;                    //we store the mouse position when user first clicks
    List<Vector3> positionsLine = new List<Vector3>();
    private float drawTimer;
    bool waitUntilNextDraw = false;         //Set to true when draws over undrawable surface, to cancel the draw
    bool drawing = false;                   //Set to true when the user is currently drawing
    private GameObject box;
    private PlatformerCharacter2D boxController;

    #endregion

    public Action OnDestroyLines = null;

    private void OnEnable()
    {
        OnDestroyLines += destroyAllLines;
    }

    private void OnDisable()
    {
        OnDestroyLines -= destroyAllLines;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        color = messageColor;                       //Starts with the message type color, needs to be on the awake for network troubles
    }

    public void Start()
    {
        if (!isLocalPlayer)//hide Bentley HUD
        {
            transform.Find("HUD").gameObject.SetActive(false);
        }
        else
        {
            //Initiate DrawButtons
            changeDrawType();                       //Set Listeners to the HUD buttons
            messageDrawButton.initiatePressed();    //Starts with the message type button
        }
    }

    private void findBoxReference()
    {
        //setBox reference
        box = GameObject.FindGameObjectWithTag("Player");
        if (box != null)
            boxController = box.GetComponent<PlatformerCharacter2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (box == null)
            findBoxReference();

        if (!isLocalPlayer)
            return;

        //To avoid confusions between HUD and draw_interaction
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //when user first click mouse
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
            if (Input.GetMouseButton(0) && (((drawTime == 0) || (drawTimer <= drawTime))) && (!waitUntilNextDraw))
            {
                drawing = true;
                Vector3 mp = Input.mousePosition;
                //if we have dragged mouse more than pointInterval pixels
                if (Vector3.Distance(mp, oldMousePos) > pointInterval)
                {
                    //get mouse world coords
                    Vector3 mwc = Camera.main.ScreenToWorldPoint(mp);
                    mwc.z = 0;

                    //update the line renderer while drawing on all clients
                    updateLine(mwc);
                }
                drawTimer += Time.deltaTime;
                //In case user is drawing over an undrawable part, negate the draw capacity until next draw
                if (!isDrawableSurface())
                {
                    waitUntilNextDraw = true;
                }
            }

            //if mouse button is up
            if (Input.GetMouseButtonUp(0))
            {
                drawing = false;
                waitUntilNextDraw = false; //Next draw
                if (usePhysics)
                {
                    //Add collider to the lr                    
                    updateLineCollider();
                }
                else
                {
                    //Clean positionsLine in case that the next draw have physics
                    cleanLinePositions();
                }
                SetDeviceShine(false);
            }
        }
        else
        {
            //in case someone is making a draw and collides to the HUD
            if (drawing)
            {
                //Act similar the user makes a buttonUp
                drawing = false;
                waitUntilNextDraw = true; //This line change cause the user do not really makes a button up, so if the user is still...
                                          //..drawing, we need to force the user to re-click again to start drawing, or bugs will be made
                if (usePhysics)
                {
                    updateLineCollider();
                }
                else
                {
                    cleanLinePositions();
                }
                SetDeviceShine(false);
            }
        }
    }

    //Server creates new draw instance
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

        if (boxController != null)
            boxController.SetDeviceShine(true);
    }

    //Find any/all lines and destroy them
    #region destroyAllLines
    public void destroyAllLines()
    {
        print("destroy");
        if (isServer)
        {
            RpcDestroyAllLines();
            print("destroy");
        }
        else
        {
            CmdDestroyAllLines();
        }
    }
    [Command]
    void CmdDestroyAllLines()
    {
        RpcDestroyAllLines();
    }
    [ClientRpc]
    void RpcDestroyAllLines()
    {
        print("rpcdestroy");
        ////find any/all lines and destroy them
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("line");
        foreach (GameObject td in toDestroy)
        {
            NetworkServer.Destroy(td);
        }
    }
    #endregion

    //Updates line renderer while drawing
    #region updateLine
    public void updateLine(Vector3 newPosWorld)
    {
        //update line on all clients
        if (isServer)
        {
            RpcUpdateLine(newPosWorld);
        }
        else
        {
            CmdUpdateLine(newPosWorld); //updates on server (And it will update locally)
        }
    }
    [Command]
    void CmdUpdateLine(Vector3 newPosWorld)
    {
        RpcUpdateLine(newPosWorld);
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
    #endregion

    //Updates line renderer collider
    #region updateLineCollider
    void updateLineCollider()
    {
        if (isServer)
        {
            RpcUpdateLineCollider();
        }
        else
        {
            CmdUpdateLineCollider(); //updates on server
        }
    }
    [Command]
    void CmdUpdateLineCollider()
    {
        RpcUpdateLineCollider();
    }
    [ClientRpc]
    void RpcUpdateLineCollider()
    {
        GameObject lineObject = GameObject.FindWithTag("line");
        //get line renderer component
        LineRenderer lr = lineObject.GetComponent<LineRenderer>();
        //Check if the draw object distance is enough to saw it on screen
        if (lr.positionCount > 1)
        {
            List<Vector2> positionsCollider = new List<Vector2>();

            //COLLIDER DIRTY VERSION
            for (int i = 0; i < positionsLine.Count; i++)
            {
                positionsCollider.Add(new Vector2(positionsLine[i].x - colliderThickness / 2, positionsLine[i].y - colliderThickness / 2));
            }
            //To allow convex problems
            for (int i = positionsLine.Count - 1; i >= 0; i--)
            {
                positionsCollider.Add(new Vector2(positionsLine[i].x + colliderThickness / 2, positionsLine[i].y + colliderThickness / 2));
            }

            //COLLIDER CLEARN VERSION
            //float ux = 0, uy = 0;
            //for (int iEdge = 0; iEdge < positionsLine.Count - 1; iEdge++)
            //{
            //    float vx = positionsLine[iEdge + 1].x - positionsLine[iEdge].x;
            //    float vy = positionsLine[iEdge + 1].y - positionsLine[iEdge].y;
            //    float vlen = (float)System.Math.Sqrt(vx * vx + vy * vy);
            //    if (vlen != 0.0)
            //    {
            //        vx /= vlen; vy /= vlen;
            //        ux = -vy; uy = vx;
            //    }
            //    positionsCollider.Add(new Vector2(positionsLine[iEdge].x + ux * colliderThickness / 2, positionsLine[iEdge].y + uy * colliderThickness / 2));
            //    if (iEdge == positionsLine.Count - 2)
            //    {
            //        positionsCollider.Add(new Vector2(positionsLine[iEdge].x + ux * colliderThickness / 2, positionsLine[iEdge].y + uy * colliderThickness / 2));
            //    }
            //}
            //for (int i = positionsLine.Count - 1; i >= 0; i--)
            //{
            //    float vx = positionsCollider[i].x, vy = positionsCollider[i].y;
            //    float px = positionsLine[i].x, py = positionsLine[i].y;
            //    px = px - (vx - px); py = py - (vy - py);
            //    positionsCollider.Add(new Vector2(px, py));
            //}

            PolygonCollider2D collider = lr.gameObject.AddComponent<PolygonCollider2D>();
            collider.points = positionsCollider.ToArray();

            //RigidBody Properties
            lr.GetComponent<Rigidbody2D>().useAutoMass = useAutoMass;
            if (useAutoMass == true)
            {
                collider.density = density;
            }
            else
            {
                lr.GetComponent<Rigidbody2D>().mass = mass;
            }
            lr.GetComponent<Rigidbody2D>().bodyType = getBodyType(bodyType);

            positionsLine.Clear();
        }
        else
        {
            Destroy(lineObject);
        }
    }
    #endregion

    //Clean positionsLine in case that the next draw have physics
    #region cleanLinePositions
    void cleanLinePositions()
    {
        if (isServer)
        {
            RpcCleanLinePositions();
        }
        else
        {
            CmdCleanLinePositions(); //updates on server
        }
    }
    [Command]
    void CmdCleanLinePositions()
    {
        RpcCleanLinePositions();
    }
    [ClientRpc]
    void RpcCleanLinePositions()
    {
        positionsLine.Clear();
    }
    #endregion

    //Advise box-device if is/is not drawing, to change shine sprites
    private void SetDeviceShine(bool shine)
    {
        if (isServer)
        {
            RpcSetDeviceShine(shine);
        }
        else
        {
            CmdSetDeviceShine(shine);
        }
    }
    [Command]
    private void CmdSetDeviceShine(bool shine)
    {
        RpcSetDeviceShine(shine);
    }
    [ClientRpc]
    private void RpcSetDeviceShine(bool shine)
    {
        if (boxController != null)
            boxController.SetDeviceShine(shine);
    }
    ////Set Listeners to the HUD buttons
    #region changeDrawType
    void changeDrawType()
    {
        if (isServer)
        {
            messageDrawButton.onClick.AddListener(delegate { RpcOnChangeDrawType("message"); });
            normalDrawButton.onClick.AddListener(delegate { RpcOnChangeDrawType("normal"); });
        }
        else
        {
            messageDrawButton.onClick.AddListener(delegate { CmdOnChangeDrawType("message"); });
            normalDrawButton.onClick.AddListener(delegate { CmdOnChangeDrawType("normal"); });
        }
    }
    [Command]
    void CmdOnChangeDrawType(string type)
    {
        RpcOnChangeDrawType(type);
    }
    [ClientRpc]
    void RpcOnChangeDrawType(string type)
    {
        switch (type)
        {
            case "message":
                color = messageColor;
                usePhysics = false;
                break;
            case "normal":
                color = normalColor;
                usePhysics = true;
                break;
        }
    }
    #endregion

    //Decides if the surface is drawable or not
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
                return hit.collider.gameObject.GetComponent<isDrawable>().drawable;
            }
        }
        return true;
    }

}