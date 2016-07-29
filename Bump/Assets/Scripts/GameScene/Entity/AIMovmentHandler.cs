﻿using UnityEngine;
using System.Collections;

public class AIMovmentHandler : EntityMovementHandler {

	// Use this for initialization

	public GameObject Ring;

	private Vector2 previousPosition;
	private Vector2 force = Vector2.zero;
	private GameObject target;

	private float minForceDistance = .35f; //when to stop adding force
	private float updatePositionEvery = .5f;
	private float frameCounter;

	private float _straightDistanceOffset = 30;
	private float _angle = 10;
	private float angle;


	//target instances
	private MovementHandler targetMovementHandler;
	private bool inContactWithTarget;



	void Start () {
		Init();
		InitThis();
	}

	void InitThis() {
		target = GameObject.Find("Player");
		previousPosition = target.transform.position;

		if (target.GetComponent<MovementHandler>() != null)
		{
			targetMovementHandler = target.GetComponent<MovementHandler>();
		}
	}

	// Update is called once per framesd
	void Update () {
		UseProjectedTrajectory();
		UseEvasion();

	}

	///summary
	///Adds force to the entity to the target
	///summary
	private void Move()
	{

	}

	private void AddSimpleForce(Vector2 targetVec)
	{


		Vector2 direction = targetVec - new Vector2(transform.position.x, transform.position.y);
		FaceFoward(transform, direction);

		if (Vector2.Distance(targetVec, transform.position) > minForceDistance) {
			rg2d.AddForce(direction.normalized);
		}
	}

	private Vector2 GetProjectedVector()
	{
		Vector2 currentPosition = target.transform.position;
		float dy =  currentPosition.y - previousPosition.y;
		float dx =  currentPosition.x - previousPosition.x;
		float distance = Vector2.Distance(target.transform.position, transform.position);
		float px = currentPosition.x + (dx * Mathf.Pow(distance, 2));
		float py = currentPosition.y + (dy * Mathf.Pow(distance, 2));
		Vector2 projectedVector = new Vector2(px, py);
		previousPosition = currentPosition;
		return projectedVector;
	}

	private void UseProjectedTrajectory() {
		frameCounter += Time.deltaTime;
		if (frameCounter >= updatePositionEvery)
		{
			force = GetProjectedVector();
			frameCounter = 0;
		}
		AddSimpleForce(force);
	}

	void OnCollisionStay2D(Collision2D col)
	{
		inContactWithTarget = true;
	}

	void OnCollisionExit2D(Collision2D col)
	{
		inContactWithTarget = false;
	}


	private void UseEvasion() {
		bool withinRange = Vector2.Distance(target.transform.position, transform.position) < .4f;

		if (withinRange && Random.Range(0, 20) < 4)
		{
			Vector2 opposingForce = transform.position - target.transform.position;
			float angle = (int)GetAngle();
			Vector2 addedForce = new Vector2(Mathf.Sin(angle) * Random.Range(-1, 1), Mathf.Cos(angle) * Random.Range(-1, 1));

			Vector2 finalForce = (opposingForce * 15.0f) + addedForce * 15.0f;
			FaceFoward(transform, finalForce);
			rg2d.AddForce(finalForce);
		}
	}

	private float GetAngle()
	{
		float dy = transform.position.y - target.transform.position.y;
		float dx = transform.position.x - target.transform.position.x;

		angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
		return angle;
	}

}
