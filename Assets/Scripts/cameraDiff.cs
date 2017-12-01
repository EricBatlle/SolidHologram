using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraDiff : MonoBehaviour {

    public float size = 7;

	// Use this for initialization
	void Start () {
        Camera.main.GetComponent<Camera>().orthographicSize = size;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
