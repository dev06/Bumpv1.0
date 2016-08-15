using UnityEngine;
using System.Collections;
using System;
public class EntityMovementHandler : MonoBehaviour {

	protected GameSceneManager _gameSceneManager;
	protected Rigidbody2D rg2d;
	protected PolygonCollider2D pCol2D;
	protected bool _boosted = false;
	protected CustomAnimator _animator;
	protected float _boostCoolDown         = 1f;
	protected float _boostCounter          = 0.0f;
	protected float _health;
	protected float MaxHealth;
	protected float Force;
	protected float Velocity;
	protected Color color;
	protected float _doubleBoostTimer = 0.0f;
	protected float _doubleBoostDelay = 1.0f;
	protected float _doubleBoostCoolDown = 0.0f;
	protected bool _doubleBoosted;
	protected bool _canBoost;

	private float _oldMagnitude;


	void Start () {
		Init();
	}

	protected void Init() {
		try {
			rg2d = GetComponent<Rigidbody2D>();
			pCol2D = transform.FindChild("BumperCollider").GetComponent<PolygonCollider2D>();
			_animator = GetComponent<CustomAnimator>();
			_gameSceneManager = GameObject.FindWithTag("GameSceneManager").GetComponent<GameSceneManager>();
		} catch (Exception e) {
			Debug.LogError("One of the component is not found : Source EntityMovementHandler ");
		}
	}

	// Update is called once per frame
	void Update () {
	}

	protected void Move() {

	}

	protected void Boost(Vector2 movement, float boost, out bool isBoosting)
	{
		if (_boosted == false && (movement != Vector2.zero || (Mathf.Abs(rg2d.velocity.x) >= 2 || Mathf.Abs(rg2d.velocity.y) >= 2)))
		{
			Vector2 force = movement * boost;
			rg2d.AddForce(force);
			_boosted = true;
			isBoosting = _boosted;
		} else {

			isBoosting = false;
		}
	}


	protected void Boost(Vector2 movement, float boostForce)
	{

		rg2d.AddForce(movement * boostForce);
	}

	protected void CanBoost()
	{
		if (_boosted)
		{
			_boostCounter += Time.deltaTime;
			if (_boostCounter > _boostCoolDown)
			{
				_boostCounter = 0;
				_boosted = false;
			}
		}
	}

	public virtual void DoubleBoost(Vector3 movement)
	{

	}

	protected void AddExternalObject(GameObject obj, Vector3 position, Quaternion rotation, Color color)
	{
		GameObject ring = Instantiate(obj, position, rotation) as GameObject;

		if (ring.GetComponent<SpriteRenderer>() != null)
		{
			ring.GetComponent<SpriteRenderer>().color = color;
		}
	}

	protected void AnimateBumper(float rate) {
		_animator.AnimateBumper(rate);
	}

	protected void FaceFoward(Transform transform, Vector2 movement) {
		float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	protected void AdjustColliderOffset(float index)
	{
		Vector2 offset = new Vector2(-.45f, 0);
		offset.x = (.45f * index) / (_animator.sprites.Length - 1);
		pCol2D.offset = offset;
	}

	protected bool isMoving(bool useVelocity, Vector2 movement) {
		if (useVelocity) {
			return rg2d.velocity.x != 0 && rg2d.velocity.y != 0;
		}
		else {
			return movement.x != 0 || movement.y != 0;
		}
	}

	protected bool WithinRange(Vector2 firstPoint, Vector2 secondPoint, float distance)
	{
		return Vector2.Distance(firstPoint, secondPoint) < distance;
	}

	protected float GetDistance(Vector3 from, Vector3 to)
	{
		return Vector3.Distance(from, to);
	}

	protected void DoDamage(float damage)
	{
		if ((int)_health > 0)
		{
			_health -= damage;
		}

		if ((int)_health <= 0)
		{
			GameSceneManager.TOTAL_PLAYERS--;
			GameSceneManager.Players.Remove(this);
			Destroy(gameObject);
		}
	}

	protected void SetHealth(float _health)
	{
		this._health = _health;
	}


	protected float CalculateForce(float time)
	{
		float currentMag = rg2d.velocity.magnitude;
		float accel = (currentMag - _oldMagnitude) / time;
		Force = rg2d.mass * accel;
		_oldMagnitude = currentMag;
		return Force;
	}

	protected virtual void CollisionEnter(Collision2D col)
	{
		if (col.gameObject.GetComponent<EntityMovementHandler>() != null)
		{
			EntityMovementHandler _thisObject = this;
			EntityMovementHandler _otherObject = col.gameObject.GetComponent<EntityMovementHandler>();
			Rigidbody2D _otherRigidBody = col.gameObject.GetComponent<Rigidbody2D>();
			float _differenceForce = Mathf.Abs(Mathf.Abs(_thisObject.Velocity) - Mathf.Abs(_otherObject.Velocity));
			float _differenceMass = Mathf.Abs(_otherRigidBody.mass - rg2d.mass);
			Logger.Log("Difference Mass " + _differenceMass);

			if (Mathf.Abs(_thisObject.Velocity) > Mathf.Abs(_otherObject.Velocity))
			{
				_otherObject.DoDamage((_differenceForce / 1000.0f) * 20.0f);
			} else
			{
				_thisObject.DoDamage((_differenceForce / 1000.0f) * 20.0f);
			}

			if (col.gameObject.GetComponent<EntityMovementHandler>().GetType() == typeof(AIMovementHandler))
			{
				//col.gameObject.GetComponent<EntityMovementHandler>().DoDamage((_differenceForce / 1000.0f) * 50.0f);
			}
		}
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		CollisionEnter(col);
	}




	public float Health {
		get {return _health;}
		set {SetHealth(value); }
	}

	public float GetForce {
		get { return Force; }
	}

	public Color Color {
		get {return color; }
		set {color = value;}
	}





}
