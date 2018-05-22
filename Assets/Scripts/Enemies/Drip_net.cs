using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Drip_net : NetworkInteractiveObject
{

    public GameObject dropPrefab;
	public bool random_0_dropEvery = false;
    public float dropEvery = 2.0f;
    public float startDropping = 0.0f;

	// Use this for initialization
	void Start () {
        if (!isServer)
            return;

        //Every 2 seconds, spawn a drop
        if (random_0_dropEvery == true) {
			dropEvery = Random.Range (0.5f,dropEvery);
			//InvokeRepeating("CmddripDrop", startDropping, dropEvery);
            StartCoroutine(DripDropCoroutine());

		} else {
			//InvokeRepeating("CmddripDrop", startDropping, dropEvery);
            StartCoroutine(DripDropCoroutine());
        }

    }

    IEnumerator DripDropCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(dropEvery);
            //CmdDripDrop();
            DripDrop();
        }

    }

    #region DripDrop
    public void DripDrop()
    {
        if (!hasAuthority)
        {
            this.OnInteraction += NmCmdDripDrop;
            setLocalAuthority();
        }
        else
        {
            CmdDripDrop();
        }
    }

    public void NmCmdDripDrop()
    {
        CmdDripDrop();
    }

    [Command]
    public void CmdDripDrop()
    {
        var drop = (GameObject)Instantiate(dropPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
        //Instantiate(dropPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
        //dropPrefab.transform.SetParent(transform);//Set the line renderer new objects as a child of the object who calls the script

        //drop.transform.SetParent(transform);
        NetworkServer.Spawn(drop);
    }
    #endregion
}
