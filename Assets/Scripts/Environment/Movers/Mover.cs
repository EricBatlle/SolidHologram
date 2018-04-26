using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    
#region EnumDirection
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }
    Vector3 getDirectionVector(Direction dir)
    {
        if (dir == Direction.Up)
        { return Vector3.up; }
        else if (dir == Direction.Down)
        { return Vector3.down; }
        else if (dir == Direction.Right)
        { return Vector3.right; }
        else if (dir == Direction.Left)
        { return Vector3.left; }
        return Vector3.up;
    }
    bool isAvailableDirection(Direction dir)
    {
        if (dir == Direction.Up)
        { return (this.transform.position.y <= destination.y); }
        else if (dir == Direction.Down)
        { return (this.transform.position.y >= destination.y); }
        else if (dir == Direction.Right)
        { return (this.transform.position.x <= destination.x); }
        else if (dir == Direction.Left)
        { return (this.transform.position.x >= destination.x); }

        return false;
    }
    #endregion

    [SerializeField] private Direction openDirection;
    [SerializeField] private Direction closeDirection;

    private Vector3 vecDirection;
    private Vector3 destination;
    private Vector3 startPos;

    private IEnumerator SmoothMovementCoroutine;


    public float maxDisplacement = 5; //if 0 -> returns to the origin
    public float speed = 1;

    private bool open = false;
    private bool close = true;

    public void Start()
    {
        startPos = this.transform.position;
    }

 
    public void Move()
    {
        //If door is closed and someone says to move -> open
        if (close)
        {
            Open();
            open = true;
            close = false;
        }
        else
        {
            Close();
            open = false;
            close = true;
        }
    }

    public void Open()
    {        
        CalculateDirectionVectors(openDirection);
        SmoothMovementCoroutine = SmoothMovement(openDirection);
        StartCoroutine(SmoothMovementCoroutine);        
    }

    public void Close()
    {
        CalculateReturnDirectionVectors(closeDirection);
        SmoothMovementCoroutine = SmoothMovement(closeDirection);
        StartCoroutine(SmoothMovementCoroutine);
    }

    IEnumerator SmoothMovement(Direction direction)
    {
        while (isAvailableDirection(direction))
        {
            transform.Translate(vecDirection * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void CalculateDirectionVectors(Direction direction)
    {
        vecDirection = getDirectionVector(direction);
        Vector3 maxVecDirection = vecDirection * maxDisplacement;
        vecDirection *= speed;
        destination = startPos + maxVecDirection;
    }

    public void CalculateReturnDirectionVectors(Direction direction)
    {
        vecDirection = getDirectionVector(direction);
        vecDirection *= speed;
        destination = startPos;
    }
}
