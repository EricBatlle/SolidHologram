using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTo : MonoBehaviour {

	public Direction direction;
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
	bool isAvailableDirection(Direction dir){
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

	private Vector3 vecDirection;
	private Vector3 destination;
	private bool startMoving = false;

	public float maxDisplacement = 5;
	public float speed = 1;

	// Use this for initialization
	void Start () {
		vecDirection = getDirectionVector(direction);
		Vector3 maxVecDirection = vecDirection * maxDisplacement;
		vecDirection *= speed;
		destination = this.transform.position + maxVecDirection;
	}

	// Update is called once per frame
	void Update () {

		//if( (startMoving == true) && (this.transform.position.y <= destination.y)){
		if(startMoving == true){
			if (isAvailableDirection(direction)) {
				transform.Translate(vecDirection * Time.deltaTime);
			}
		}
	}

	//Start moving when Player touch the trigger
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			startMoving = true;
		}

	}

}
