using System;
using UnityEngine;
using UnityEngine.Networking;


namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : NetworkInteractiveObject
    {
        #region variables 
        [Header("Main Attributes")]
        [SerializeField] private float m_MaxSpeed = 10f;                                // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                              // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;                             // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                              // A mask determining what is ground to the character
        [Header("Standard Raycasts")]
        [SerializeField] private float collidersRaycastDistance = .6f;                  // Raycast distance to check if moving is allowed
        [SerializeField] private float ceilingAltitude = 0.78f;
        [SerializeField] private float rampDetection = 0.25f;
        [Header("Crouch settings")]
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;              // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private float collidersRayDistCrouchDifference = 0.5f; //Those values needs to be calculated manually 
        [SerializeField] private float ceilingAltitudeCrouchDifference = -0.3f;         //...if the crouch animation change
        [Header("Help settings")]
        [SerializeField] private GameObject helpPanel;
        private Animator m_AnimHelp;                    // Reference to the help panel's animator component.
        private SpriteRenderer m_SpriteRHelp;           // Reference to the help panel's SpriteRenderer component.
        private bool askHelp = false;

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.        

        public event Action OnPlayerDies = null;
        #endregion
                
        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>(); //Get Box animator
            m_AnimHelp = helpPanel.GetComponent<Animator>(); //Get Help Panel animator
            m_SpriteRHelp = helpPanel.GetComponent<SpriteRenderer>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            Camera.main.GetComponent<CustomCinemachine>().setTarget(gameObject.transform);
        }

        private void OnEnable()
        {
            this.OnPlayerDies += RespawnPlayer;
            this.OnPlayerDies += destroyAllLines;
            this.OnPlayerDies += RelocateCamera;
        }
        private void OnDisable()
        {
            this.OnPlayerDies -= RespawnPlayer;
            this.OnPlayerDies -= destroyAllLines;
            this.OnPlayerDies -= RelocateCamera;
        }

        //Every time the scene changes...
        public void OnLevelWasLoaded(int level)
        {
            //...the camera needs to reference again the player
            Camera.main.GetComponent<CustomCinemachine>().setTarget(gameObject.transform);

            //...the player has to be realocated to the new spawn point
            gameObject.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        }

        private void Start()
        {
            //For the first lobby start, onlevelwasloaded is not called, so Player has to be realocated to the first spawn point
            gameObject.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        }
        
        //MAIN LOOP         
        private void FixedUpdate()
        {
            if (!isLocalPlayer)
                return;

            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

            // Set the help animation
            askHelp = Input.GetKey(KeyCode.H) ? true : false;
            m_AnimHelp.SetBool("askHelp", askHelp);
        
        }

        //Die Collison and Trigger checks
        #region dieBehaviour
        //ToDo: Remove PlayerKilled, as killPlayer no longer should exists
        public void PlayerKilled()
        {
            if (OnPlayerDies != null)
                OnPlayerDies();
        }

        //Killing Colliders
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Killer"))
            {
                if (OnPlayerDies != null)
                    OnPlayerDies();
            }
        }

        //Killing Triggers
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Killer"))
            {
                if (OnPlayerDies != null)
                    OnPlayerDies();
            }
        }
        #endregion

        //Relocate player
        #region respawnPlayer
        public void RespawnPlayer()
        {
            //Esto da problemas, porque cuando hay objetos "retractiles" como la capsula, esto se dispara rápido
            //...en el servidor, pero en el cliente no llega aún, pero la capsula ya se ha reposicionado, debería mirar de hacer
            //...algo con syncvars?
            this.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        }
        //public void RespawnPlayer()
        //{
        //    if (!isLocalPlayer)
        //        return;
        //    print("respawn");
        //    if (isServer)
        //    {
        //        print("isserver");
        //        RpcDestroyAllLines();
        //    }
        //    else
        //    {
        //        print("notserver");
        //        CmdDestroyAllLines();
        //    }
        //}
        //[Command]
        //void CmdRespawnPlayer()
        //{
        //    RpcRespawnPlayer();
        //}
        //[ClientRpc]
        //void RpcRespawnPlayer()
        //{
        //    this.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        //}
        #endregion

        //Relocate Camera
        #region relocateCamera
        public void RelocateCamera()
        {
            if (!isLocalPlayer)
                return;

            if (isServer)
            {
                RpcRelocateCamera();
            }
            else
            {
                CmdRelocateCamera();
            }
        }
        [Command]
        private void CmdRelocateCamera()
        {
            RpcRelocateCamera();
        }
        [ClientRpc]
        private void RpcRelocateCamera()
        {
            //Find every virtual camera,
            GameObject[] virtualCameras = GameObject.FindGameObjectsWithTag("Vcam");
            foreach(GameObject vcam in virtualCameras)
            {
                //Set their priority to 0           
                vcam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
                //...better approach could be, if there are different prioritys, to make cameras store they "default" priority, and instead of force to 0, put them their original priority
            }
        }
        
        #endregion

        //Find any/all lines and destroy them :: same function that has Bentley
        #region destroyAllLines
        public void destroyAllLines()
        {
            if (!isLocalPlayer)
                return;

            if (isServer)
            {
                RpcDestroyAllLines();
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
            ////find any/all lines and destroy them
            GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("line");
            foreach (GameObject td in toDestroy)
            {
                NetworkServer.Destroy(td);
            }
        }
        #endregion

        //Shine Sprites behaviuor
        #region shineDevice
        public void SetDeviceShine(bool shine)
        {
            if (shine)
            {
                if (isServer)
                {
                    RpcSetDeviceShineOn();
                }
                else
                {
                    if (!hasAuthority)
                    {
                        this.OnInteraction += NmCmdSetDeviceShineOn;
                    }
                    else
                    {
                        CmdSetDeviceShineOn();
                    }
                }
            }
            else
            {
                if (isServer)
                {
                    RpcSetDeviceShineOff();
                }
                else
                {
                    if (!hasAuthority)
                    {
                        this.OnInteraction += NmCmdSetDeviceShineOff;
                    }
                    else
                    {
                        CmdSetDeviceShineOff();
                    }
                }
            }
        }
        #region shineOn
        private void NmCmdSetDeviceShineOn()
        {
            CmdSetDeviceShineOn();
        }
        [Command]
        private void CmdSetDeviceShineOn()
        {
            RpcSetDeviceShineOn();
        }
        [ClientRpc]
        private void RpcSetDeviceShineOn()
        {
            m_Anim.SetBool("Shine", true);

            //if shine-mode is active, set shine layer to 1, if not, "disable" it setting it to 0
            float shineLayerWeight = (m_Anim.GetBool("Shine")) ? 1 : 0;
            m_Anim.SetLayerWeight(m_Anim.GetLayerIndex("Shine Layer"), shineLayerWeight);
        }
        #endregion
        #region shineOff
        private void NmCmdSetDeviceShineOff()
        {
            CmdSetDeviceShineOff();
        }
        [Command]
        private void CmdSetDeviceShineOff()
        {
            RpcSetDeviceShineOff();
        }
        [ClientRpc]
        private void RpcSetDeviceShineOff()
        {
            m_Anim.SetBool("Shine", false);

            //if shine-mode is active, set shine layer to 1, if not, "disable" it setting it to 0
            float shineLayerWeight = (m_Anim.GetBool("Shine")) ? 1 : 0;
            m_Anim.SetLayerWeight(m_Anim.GetLayerIndex("Shine Layer"), shineLayerWeight);
        }
        #endregion
        #endregion

        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move * m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                //Prevent the movement if the future position collides to Layer Wall
                int layermask = (1 << 8);
                //int layermask2 = (1 << 10);
                //int finallayermask = layermask | layermask2;
                RaycastHit2D hit_CeilingCheck;
                RaycastHit2D hit_BodyCheck;
                RaycastHit2D hit_GroundCheck;
                Vector2 raycastDirection = (IsLookingAt("right", move)) ? Vector2.right : Vector2.left;
                float updatedCeilingAltitude;
                float updatedCollidersRaycastDistance;
                //Update raycasts if the player is crouching, cause colliders shrink and stretch
                if (crouch)
                {
                    updatedCeilingAltitude = ceilingAltitude + ceilingAltitudeCrouchDifference;
                    updatedCollidersRaycastDistance = collidersRaycastDistance + collidersRayDistCrouchDifference;
                }
                else
                {
                    updatedCeilingAltitude = ceilingAltitude;
                    updatedCollidersRaycastDistance = collidersRaycastDistance;        
                }
                //Ceiling Raycast Check
                Vector2 endPos = new Vector2(transform.position.x, transform.position.y+ updatedCeilingAltitude) + raycastDirection * updatedCollidersRaycastDistance;//m_GroundCheck.position;
                Debug.DrawLine(transform.position+new Vector3(0, updatedCeilingAltitude, 0), endPos, Color.red);
                hit_CeilingCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y+ updatedCeilingAltitude), raycastDirection, updatedCollidersRaycastDistance, layermask);

                //Body Raycast Check
                Vector2 endPos_body = new Vector2(transform.position.x, transform.position.y) + raycastDirection * updatedCollidersRaycastDistance;//m_GroundCheck.position;
                Debug.DrawLine(transform.position, endPos_body, Color.red);
                hit_BodyCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), raycastDirection, updatedCollidersRaycastDistance, layermask);

                //Ground Raycast Check
                Vector2 endPos_ground = new Vector2(m_GroundCheck.position.x, m_GroundCheck.position.y+rampDetection) + raycastDirection * updatedCollidersRaycastDistance;//m_GroundCheck.position;
                Debug.DrawLine(m_GroundCheck.position, endPos_ground, Color.red);
                hit_GroundCheck = Physics2D.Raycast(new Vector2(m_GroundCheck.position.x, m_GroundCheck.position.y+rampDetection), raycastDirection, updatedCollidersRaycastDistance, layermask);
                
                

                //If none of the raycasts are colliding to anything...
                if ((hit_CeilingCheck.collider == null) && (hit_BodyCheck.collider == null) && (hit_GroundCheck.collider == null))
                {
                    //... Move the character
                    m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
                }

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }

        //Returns if the player is looking to the specified direction (right or left)
        private bool IsLookingAt(string direction, float move)
        {
            if (move >= 0)
            {
                return (direction.Equals("right")) ? true : false;
            }
            else
            {
                return (direction.Equals("right")) ? false : true;
            }
        }

        //flip sprite to look where you're moving
        #region flip
        private void Flip()
        {
            if (isServer)
            {
                RpcFlip();
            }
            else
            {
                NmFlip();
                CmdFlip();
            }
        }

        private void flipFunction()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            //Flip also the Help Panel            
            m_SpriteRHelp.flipX = !m_SpriteRHelp.flipX;
        }

        private void NmFlip()
        {
            flipFunction();
        }
        [ClientRpc]
        private void RpcFlip()
        {
            flipFunction();
        }
        [Command]
        private void CmdFlip()
        {
            flipFunction();
        }
        #endregion
    }
}

