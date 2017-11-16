using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class networkSelector : NetworkBehaviour
{

    GameObject box;
    GameObject bentley;
    public GameObject camObject;


    // Use this for initialization
    void Start()
    {
        camObject = GameObject.Find("Main Camera");

        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Bentley"))
            {
                bentley = child.gameObject;
            }
            if (child.gameObject.CompareTag("Player"))
            {
                box = child.gameObject;
            }
        }


        //I'm the Server/Host - runner
        if (isServer)
        {
            //I'm the server
            if (isLocalPlayer)
            {
                //I'm the host
                bentley.SetActive(false);
                PlayerCamera pc = camObject.GetComponent<PlayerCamera>();
                pc.setPlayer(box);
            }
            else
            {
                box.SetActive(false);
                bentley.SetActive(true);
            }
        }

        //I'm the Client - artist
        if (!isServer)
        {
            if (isLocalPlayer)
            {
                box.SetActive(false);
                bentley.SetActive(true);
            }
            else
            {
                box.SetActive(true);
                bentley.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isServer)
            {
                //I'm the server
                print("server");
                if (isLocalPlayer)
                {
                    print("host");
                }
            }
            else
            {
                //I'm  not the server
                print("client");
                if (isLocalPlayer)
                {
                    print("muy client");
                }
            }
        }
    }
}