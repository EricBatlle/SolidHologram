using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class BouncingCapsule : ActivableObject {

    [SerializeField] private Rigidbody2D rigidBody2D;

    private Vector3 initialPos;

    private void Start()
    {
        initialPos = this.transform.position;
    }

    private void OnEnable()
    {
        OnBoxReferenceFinded += setOnEnable;
    }

    private void setOnEnable()
    {
        box.OnPlayerDies += EndBehaviour;
    }

    private void OnDisable()
    {
        if (box != null)
            box.OnPlayerDies -= EndBehaviour;
    }

    public override void StartBehaviour()
    {
        rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
    }
    public override void EndBehaviour()
    {
        Invoke("Realocate", 1.5f);
    }
    private void Realocate()
    {
        rigidBody2D.bodyType = RigidbodyType2D.Static;
        this.transform.position = this.initialPos;
        rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidBody2D.velocity = new Vector2(0, 0);
    }

}
