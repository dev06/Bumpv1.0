using UnityEngine;
using System.Collections;
using System;
public class Bumper : MonoBehaviour {

    // Use this for initialization
    private bool _hit;
    private GameObject _collidingObject;
    private CustomAnimator _animator;



    void Start () {
        try {
            _animator = GetComponentInParent<CustomAnimator>();
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
            if (_animator._bumperActive)
            {
                Vector3 direction = col.gameObject.transform.position - transform.parent.transform.position;
                float _thisImpulse = (Constants.BUMPER_IMPULSE_MAG * Mathf.Sqrt(GetComponentInParent<Rigidbody2D>().velocity.SqrMagnitude())) + Constants.BASE_BUMPER_IMPLUSE;

                float _thisVel = GetComponentInParent<Rigidbody2D>().velocity.magnitude;
                float _otherVel = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;

                float _velDifference = Mathf.Abs(_thisVel - _otherVel);


                col.gameObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * _thisImpulse);


                float entityDamage = (_velDifference / 100.0f) * Constants.BUMPER_DAMAGE_BASE;


                if (col.gameObject.GetComponent<EntityMovementHandler>().GetType() == typeof(MovementHandler))
                {
                    MovementHandler _movementHandler = (MovementHandler)col.gameObject.GetComponent<EntityMovementHandler>();

                    _movementHandler.DoDamage(entityDamage);
                    Logger.Log("PLAYER HEALTH => " + _movementHandler.Health);


                } else if (col.gameObject.GetComponent<EntityMovementHandler>().GetType() == typeof(AIMovementHandler))
                {
                    AIMovementHandler _aiMovementHandler = (AIMovementHandler)col.gameObject.GetComponent<EntityMovementHandler>();
                    _aiMovementHandler.DoDamage(entityDamage);
                    Logger.Log("BOT HEALTH => " + _aiMovementHandler.Health);
                }
               _hit = true;
            }
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
