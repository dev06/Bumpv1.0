using UnityEngine;
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

	void Start () {
		Init();
		InitThis();
	}

	void InitThis() {
		target = GameObject.Find("Player");
		previousPosition = target.transform.position;
	}

	// Update is called once per framesd
	void Update () {
		UseProjectedTrajectory();
		CanBoost(); 

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
		// bool boosting; 
		// Boost(direction, 10f, out boosting); 
		// if(boosting){
		// 	AddExternalObject(Ring, transform.position, transform.rotation); 
		// 	boosting = false; 
		// }
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


}
