using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingCapsule : ActivableObject {

    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private killPlayer killingObject;

    private Vector3 initialPos;

    private void Start()
    {
        initialPos = this.transform.position;
    }

    private void OnEnable()
    {
        killingObject.OnKill += EndBehaviour;
    }

    private void OnDisable()
    {
        killingObject.OnKill -= EndBehaviour;
    }

    public override void StartBehaviour()
    {
        rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
    }
    public override void EndBehaviour()
    {
        rigidBody2D.bodyType = RigidbodyType2D.Static;
        this.transform.position = this.initialPos;
        rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
    }

}
