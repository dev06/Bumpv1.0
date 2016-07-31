using UnityEngine;
using System.Collections;

public class AIMovmentHandler : EntityMovementHandler {

	// Use this for initialization

	public GameObject Ring;
	public GameObject target;

	private Vector2 previousPosition;
	private Vector2 force = Vector2.zero;


	private float minForceDistance           = 10; //when to stop adding force
	private float updatePositionEvery        = .5f;
	private float attackFrequency            = .8f;  //0.0f (0%) - 1.0f (100%)
	private float _boostForce 				 = 1000f;
	private float _boostFrequency 			 = .02f;
	private float _startBoostSpeed           = 20.0f;
	private float _straightDistanceOffset    = 30f;
	private float _angle                     = 10f;
	private float _velocity                  = 50f;
	private float _evadeFrequency 			 = .01f;
	private float _evadeForce                = 400f;
	private float angle;
	private float frameCounter;
	public Color _color; 


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
		
		}
		previousPosition = target.transform.position;

		if (target.GetComponent<MovementHandler>() != null)
		{
			targetMovementHandler = target.GetComponent<MovementHandler>();

		}

		_animator = GetComponent<CustomAnimator>();
		GetComponent<SpriteRenderer>().color = _color; 
		
	}

	// Update is called once per framesd
	void Update () {
		Move();
		AnimateBotBumper(1);



	}

	///summary
	///Adds force to the entity to the target
	///summary
	private void Move()
	{
		UseProjectedTrajectory();
		UseEvasion();
		CanBoost();
		UseBoost();
	}

	private void AddSimpleForce(Vector2 targetVec)
	{


		Vector2 direction = targetVec - new Vector2(transform.position.x, transform.position.y);
		FaceFoward(transform, direction);

		if (Vector2.Distance(targetVec, transform.position) > minForceDistance) {
			rg2d.AddForce(direction.normalized * _velocity);
		}
	}

	private Vector2 GetProjectedVector()
	{
		float generation = .01f;
		Vector2 currentPosition = target.transform.position;
		float dy =  currentPosition.y - previousPosition.y;
		float dx =  currentPosition.x - previousPosition.x;
		float distance = Vector2.Distance(target.transform.position, transform.position);
		float px = currentPosition.x + (dx * Mathf.Pow(distance, generation));
		float py = currentPosition.y + (dy * Mathf.Pow(distance, generation));
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
		bool withinRange = Vector2.Distance(target.transform.position, transform.position) < minForceDistance * 3.0f;


		if (withinRange)
		{

			if (targetMovementHandler != null)
			{

				if (targetMovementHandler.IsBumperActive)
				{
					float value = Random.Range(0.0f, 1.0f);
					if (value < _evadeFrequency * 2.0f)
					{
						Evade(_evadeForce);
					}
				} else
				{
					float value = Random.Range(0.0f, 1.0f);
					if (value < _evadeFrequency)
					{
						Evade(_evadeForce);
					}
				}
			}
		}
	}

	private void Evade(float intensity)
	{
		Vector2 opposingForce = transform.position - target.transform.position;
		float angle = (int)GetAngle();
		Vector2 addedForce = new Vector2(Mathf.Sin(angle) * MathUtil.RangeBetweenTwo(-1, 1), Mathf.Cos(angle) * MathUtil.RangeBetweenTwo(-1, 1));

		Vector2 finalForce = (opposingForce * intensity) + addedForce * intensity;
		//FaceFoward(transform, finalForce);
		rg2d.AddForce(finalForce.normalized * intensity);

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
		if (WithinRange(transform.position, target.transform.position, minForceDistance * 2.0f) && Random.Range(0.0f, 1.0f) <= attackFrequency)
		{
			_animator.AnimateBumper(rate);


		} else
		{
			_animator.ResetIndex();
		}
	}

	private void UseBoost()
	{

		if (Random.Range(0.0f, 1.0f) <= _boostFrequency)
		{
			if (rg2d.velocity.SqrMagnitude() > _startBoostSpeed)
			{
				bool boosted;
				Boost(force.normalized, _boostForce, out boosted);
				if (boosted)
				{
					AddExternalObject(Ring, transform.position, transform.rotation, _color);
				}
			}
		}

		bool withinRange = Vector2.Distance(target.transform.position, transform.position) < minForceDistance * 2.0f;
		float targetVelocity = (target.GetComponent<Rigidbody2D>() != null) ? target.GetComponent<Rigidbody2D>().velocity.SqrMagnitude() : 0;
		if (withinRange)
		{
			if (rg2d.velocity.SqrMagnitude() < targetVelocity)
			{

				if (targetVelocity > 100.0f)
				{
					Evade(targetVelocity / 30.0f);
				}
			}
		}
	}

}
