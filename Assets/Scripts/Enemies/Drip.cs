using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drip : MonoBehaviour {

    public GameObject dropPrefab;
	public bool random_0_dropEvery = false;
    public bool dropping = false;

    public float dropEvery = 2.0f;
    public float startDropping = 0.0f;
    [SerializeField] private CustomTrigger startTrigger;

    // Use this for initialization

    private void OnEnable()
    {
        if (startTrigger != null)
            startTrigger.OnEnter = InvokeDripDrop;
    }
    private void OnDisable()
    {
        if (startTrigger != null)
            startTrigger.OnEnter = null;
    }

    void Start () {
        //Every 2 seconds, spawn a drop
        if (!dropping)
        {
            if (random_0_dropEvery == true)
            {
                dropEvery = Random.Range(0.5f, dropEvery);
                dropping = true;
                InvokeRepeating("dripDrop", startDropping, dropEvery);

            }
            else
            {
                if (startTrigger == null)
                {
                    dropping = true;
                    InvokeRepeating("dripDrop", startDropping, dropEvery);
                }
            }
        }		
    }
	
    public void InvokeDripDrop()
    {
        if (!dropping)
        {
            dropping = true;
            InvokeRepeating("dripDrop", startDropping, dropEvery);
        }        
    }

    public void dripDrop()
    {
        var drop = (GameObject)Instantiate(dropPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

		drop.transform.SetParent(transform);
	}
}
