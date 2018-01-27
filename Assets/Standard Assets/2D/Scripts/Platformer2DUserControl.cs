using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : NetworkBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
		private bool isColliding;

        //Only local player can use it
        private void Start()
        {
            if (!isLocalPlayer)
            {
                Destroy(this);
                return;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            m_Character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
		{
			// Read the inputs.
			bool crouch = Input.GetKey (KeyCode.LeftControl);
			//if(!isColliding){
				float h = CrossPlatformInputManager.GetAxis ("Horizontal");
			//}
			// Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }

    }
}