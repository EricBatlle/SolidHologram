using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    [SerializeField] private Mover mover;
    public float moveEvery = 2.0f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("movePlatform", 2.0f, moveEvery);
	}

    void movePlatform()
    {
        mover.Move();
    }
}
