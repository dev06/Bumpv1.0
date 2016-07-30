using UnityEngine;
using System.Collections;

public class AIMovmentHandler : EntityMovementHandler {

	// Use this for initialization

	public GameObject Ring;
	public GameObject target;

	private Vector2 previousPosition;
	private Vector2 force = Vector2.zero;


	private float minForceDistance           = .35f; //when to stop adding force
	private float updatePositionEvery        = .5f;
	private float attackFrequency            = .8f;  //0.0f (0%) - 1.0f (100%)
	private float _straightDistanceOffset    = 30f;
	private float _angle                     = 10f;
	private float angle;
	private float frameCounter;


	//target instances
	private MovementHandler targetMovementHandler;
	private bool inContactWithTarget;



	void Start () {
		Init();
		InitThis();
	}

	void InitThis() {
		if (target == null)
		{
			target = GameObject.Find("Player");
		}
		previousPosition = target.transform.position;

		if (target.GetComponent<MovementHandler>() != null)
		{
			targetMovementHandler = target.GetComponent<MovementHandler>();
		}

		_animator = GetComponent<CustomAnimator>();
	}

	// Update is called once per framesd
	void Update () {
		UseProjectedTrajectory();
		UseEvasion();
		AnimateBotBumper(1);
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
		Logger.Log(previousPosition); 
		return projectedVector;
	}

	private void UseProjectedTrajectory() {
		frameCounter += Time.deltaTime;
		if (frameCounter >= updatePositionEvery)
		{
			force = GetProjectedVector();
			frameCounter = 0;
		}
		//Boost(force, 60);
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
		bool withinRange = Vector2.Distance(target.transform.position, transform.position) < minForceDistance;

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

	private void AnimateBotBumper(float rate)
	{
		AdjustColliderOffset(_animator.index);
		if (WithinRange(transform.position, target.transform.position, minForceDistance) && Random.Range(0, 1) <= attackFrequency)
		{
			_animator.AnimateBumper(rate);

		} else
		{
			_animator.ResetIndex();
		}
	}

}
