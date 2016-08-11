using UnityEngine;
using System.Collections;
using System;
public class Bumper : MonoBehaviour {

    // Use this for initialization
    private bool _hit;
    private Rigidbody2D rg2d;
    private GameObject _collidingObject;
    private CustomAnimator _animator;



    void Start () {

        try {
            _animator = GetComponentInParent<CustomAnimator>();
            rg2d = GetComponentInParent<Rigidbody2D>();
        } catch (Exception e) {
            Debug.Log("Components could not be found");
        }


    }

    void Update()
    {

    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            DoBumperDamage(col);
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            DoBumperDamage(col);
        }
    }


    void DoBumperDamage(Collision2D col)
    {
        if (_animator._bumperActive)
        {
            Vector3 direction = col.gameObject.transform.position - transform.parent.transform.position;
            float _thisImpulse = (Constants.BUMPER_IMPULSE_MAG * Mathf.Sqrt(rg2d.velocity.SqrMagnitude())) + Constants.BASE_BUMPER_IMPLUSE;

            float _thisVel = rg2d.velocity.magnitude;

            float _otherVel = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;

            float _velDifference = Mathf.Abs(_thisVel - _otherVel);

            float _thisMomemtum = GetComponentInParent<EntityMovementHandler>().GetForce;
            float _otherMomentum = col.gameObject.GetComponent<EntityMovementHandler>().GetForce;

            float _momemtumDiff = Mathf.Abs(Mathf.Abs(_thisMomemtum) - Mathf.Abs(_otherMomentum));


            float entityDamage = (_momemtumDiff / 400.0f) * Constants.BUMPER_DAMAGE_BASE;
            //  Logger.Log(_thisMomemtum + " "  + _otherMomentum +  " ==== " +  _momemtumDiff + "\n" + " Entity Damage -> " + entityDamage);

            //Logger.Log(transform.parent + " Velocities -> " + _thisVel + " " + _otherVel +  " " + _velDifference + "\n" + "Entity Damage -> " +  entityDamage);




            if (col.gameObject.GetComponent<EntityMovementHandler>().GetType() == typeof(MovementHandler))
            {
                MovementHandler _movementHandler = (MovementHandler)col.gameObject.GetComponent<EntityMovementHandler>();
                _movementHandler.DoDamage(entityDamage);

            } else if (col.gameObject.GetComponent<EntityMovementHandler>().GetType() == typeof(AIMovementHandler))
            {
                AIMovementHandler _aiMovementHandler = (AIMovementHandler)col.gameObject.GetComponent<EntityMovementHandler>();
                _aiMovementHandler.DoDamage(entityDamage);
            }

            col.gameObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * _thisImpulse);

            _hit = true;
        }
    }


    void OnCollisionExit2D(Collision2D col)
    {
        _hit = false;
    }

    public bool Hit {
        get {return _hit; }
        set {_hit = value; }
    }

    public GameObject Other {
        get {return _collidingObject; }
    }
}
