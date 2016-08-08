using UnityEngine;
using System.Collections;

public class MovementHandler : EntityMovementHandler
{

    #region--- PRIVATE MEMBERS---

    private bool isUsingController = false;
    private float _startBoostForce = 2000f;
    //public Color color;

    #endregion---/PRIVATE MEMEBERS


    public GameObject BoostRing;
    [HideInInspector]
    public Vector2 movementDirection;
    public float Velocity;



    void Start()
    {   
        base.Init(); 
        Health = 100; 
        GetComponent<SpriteRenderer>().color = color;
    }


    void Update()
    {
        AdjustColliderOffset(_animator.index);
        CanBoost();
        Move();
    }

    /// <summary>
    /// Moves the Player
    /// </summary>
    void Move()
    {
        float horizontal = Input.GetAxis((GameSceneManager.isControllerConnected) ? Constants.CONTROLLER_LEFT_STICK_HORIZONTAL : Constants.HORIZONTAL);
        float vertical = Input.GetAxis((GameSceneManager.isControllerConnected) ? Constants.CONTROLLER_LEFT_STICK_VERTICAL : Constants.VERTICAL);
        Vector2 movement = new Vector2(horizontal, vertical);

        rg2d.AddForce((movement * Velocity) / rg2d.mass);

        movementDirection = movement;

        if (GameSceneManager.isControllerConnected)
        {
            if (Input.GetButtonDown(Constants.CONTROLLER_X_BUTTON))
            {
                bool boost;
                Boost(movement, _startBoostForce, out boost);
                if (boost) {
                    AddExternalObject(BoostRing, transform.position, transform.rotation, color);
                }
            }

            if (Input.GetButton(Constants.CONTROLLER_SQUARE_BUTTON) && _animator._triggerHit == false && _animator._bumperActive == false)
            {
                _animator._triggerHit = true;
                _animator._bumperActive = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool boost;
                Boost(movement, _startBoostForce, out boost);
                if (_boosted) {
                    AddExternalObject(BoostRing, transform.position, transform.rotation, color);
                }
            }

            if (Input.GetMouseButton(0) && _animator._triggerHit == false && _animator._bumperActive == false)
            {
                _animator._triggerHit = true;
                _animator._bumperActive = true;
            }
        }

        if (isMoving(false, movement)) {
            if (_animator._triggerHit) {
                AnimateBumper(50.0f * Time.fixedDeltaTime);
                Logger.Log(50.0f * Time.fixedDeltaTime); 
            }
        } else {
            _animator.ResetIndex();
        }

        //AnimateBumper(1);
        FaceFoward(transform, movement);
    }


    public void DoDamage(float damage)
    {
        base.DoDamage(damage); 

    }


    public bool IsBumperActive {
        get { return _animator._bumperActive; }
    }


    public float Health {
        get { return _health; }
        set {SetHealth(value); }
    }
}
