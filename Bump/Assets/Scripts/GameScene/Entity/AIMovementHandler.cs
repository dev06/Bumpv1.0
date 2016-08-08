using UnityEngine;
using System.Collections;

public class AIMovementHandler : EntityMovementHandler {

	// Use this for initialization

	#region--PUBLIC VARS--
	public GameObject Ring;
	public GameObject target;
	#endregion--/PUBLIC VARS--
	
	private Bumper _bumper;
	private Vector2 previousPosition;
	private Vector2 force = Vector2.zero;
	private Vector2 _partrolVec = Vector2.zero;
	private bool hit;
<<<<<<< HEAD
	private float minForceDistance           = 10; //when to stop adding force
=======

	private float minForceDistance           =  10; //when to stop adding force
>>>>>>> 4063d87f2efc4ef189168ea0efd21c80ef0b3bf6
	private float updatePositionEvery        = .5f;
	private float attackFrequency            = .8f;  //0.0f (0%) - 1.0f (100%)
	private float _boostForce 				 = 1000f;
	private float _boostFrequency 			 = .02f;
	private float _startBoostSpeed           = 20.0f;
	private float _straightDistanceOffset    = 30f;
	private float _angle                     = 10f;
	private float _velocity                  = 65f;
	private float _evadeFrequency 			 = .01f;
	private float _evadeForce                = 400f;
	private float _changeDirFreq             = .02f;
	private float _patrolDX;
	private float _patrolDY;
	private float angle;
	private float frameCounter;


	//target instances
	private MovementHandler targetMovementHandler;
	private bool inContactWithTarget;


	//DEBUG MEMBERS
	public static bool TOGGLE;

	void Start () {
		Init();
		InitThis();
	}

	void InitThis() {
		previousPosition = target.transform.position;
		if (target.GetComponent<MovementHandler>() != null)
		{
			targetMovementHandler = target.GetComponent<MovementHandler>();
		}
		_animator = GetComponent<CustomAnimator>();
		_bumper = GetComponentInChildren<Bumper>();
		GetComponent<SpriteRenderer>().color = color;
		Health = 100.0f;
	}

	// Update is called once per framesd
	void Update () {
		Manage();
	}

	///summary
	///Adds force to the entity to the target
	///summary
	private void Move(bool shouldMove)
	{
		if (shouldMove)
		{
			UseProjectedTrajectory();
			UseEvasion();
			CanBoost();
			UseBoost();
		}
	}

	private void Manage()
	{
		if (target != null)
		{
			if (isDead()) Destroy();
			Move(TOGGLE);
			AnimateBotBumper(1);
		} else
		{
			ChooseNextTarget();
		}
	}

	private void ChooseNextTarget()
	{
		Transform newTarget = GameSceneManager.Players[Random.Range(0, GameSceneManager.Players.Count)].gameObject.transform;
		if (newTarget == transform)
		{
			newTarget = GameSceneManager.Players[Random.Range(0, GameSceneManager.Players.Count)].gameObject.transform;
		}
	}

	private void AddSimpleForce(Vector2 targetVec)
	{


		Vector2 direction = targetVec - new Vector2(transform.position.x, transform.position.y);
		FaceFoward(transform, direction);

		if (Vector2.Distance(targetVec, transform.position) > minForceDistance) {
			rg2d.AddForce(((direction.normalized * Time.deltaTime) * (_velocity * _velocity) / rg2d.mass));
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
			force = GetProjectedVector() + Patrol();
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
					AddExternalObject(Ring, transform.position, transform.rotation, color);
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



	private Vector2 Patrol()
	{
		float differenceOffset = 50;
		float currentVelocity = Mathf.Sqrt(rg2d.velocity.magnitude);
		float lowerBound = currentVelocity - (differenceOffset * 2.0f);
		float upperBound = currentVelocity + (differenceOffset * 0.03f);
		float resultValue = Random.Range(lowerBound, upperBound);
		bool changedDir = false;

		if (resultValue > currentVelocity)
		{
			ChangeDirection(false);
		}

		//rg2d.AddForce((_partrolVec * _velocity) / rg2d.mass);
		return _partrolVec;

	}

	void OnCollisionEnter2D(Collision2D col)
	{
		ChangeDirection(true);
	}

	private void ChangeDirection(bool opposite)
	{
		_patrolDX = (!opposite) ? Random.Range(-1.0f, 1.0f) : _patrolDX * -Random.Range(0.0f, 1.0f);
		_patrolDY = (!opposite) ? Random.Range(-1.0f, 1.0f) : _patrolDY * -Random.Range(0.0f, 1.0f);
		_partrolVec.x = _patrolDX;
		_partrolVec.y = _patrolDY;
	}

	private bool isDead()
	{
		return Health <= 0;
	}

	private void Destroy()
	{
		//Destroy(gameObject);
	}

	public void DoDamage(float damage)
	{
		base.DoDamage(damage);

	}

	public float Health {
		get { return _health;}
		set { SetHealth(value);}
	}
}
