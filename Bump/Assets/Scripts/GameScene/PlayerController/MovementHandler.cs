using UnityEngine;
using System.Collections;

public class MovementHandler : EntityMovementHandler
{

    #region--- PRIVATE MEMBERS---

    private bool isUsingController = false;
    private float _startBoostForce = 20f;
    //public Color color;

    #endregion---/PRIVATE MEMEBERS


    public GameObject BoostRing;
    [HideInInspector]
    public Vector2 movementDirection;
    public float MovementVelocity;

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

        //Debug
        if (Input.GetKeyDown(KeyCode.H))
        {
            Health = 100;
        }

        CalculateForce(Time.deltaTime);
    }

    /// <summary>
    /// Moves the Player
    /// </summary>
    void Move()
    {
        float horizontal = Input.GetAxis((_gameSceneManager.CustomInputManager.IsUsingController) ? Constants.CONTROLLER_LEFT_STICK_HORIZONTAL : Constants.HORIZONTAL) * Time.deltaTime;
        float vertical = Input.GetAxis((_gameSceneManager.CustomInputManager.IsUsingController) ? Constants.CONTROLLER_LEFT_STICK_VERTICAL : Constants.VERTICAL) * Time.deltaTime;
        Vector2 movement = new Vector2(horizontal, vertical);
        Velocity = rg2d.velocity.magnitude;
        rg2d.AddForce((movement * (MovementVelocity * MovementVelocity) / rg2d.mass));

        movementDirection = movement;

        if (_gameSceneManager.CustomInputManager.IsUsingController)
        {
            if (_gameSceneManager.CustomInputManager.GetInputEventPress == InputEvent.GameInputEventPress.BOOST)
            {
                bool boost;
                Boost(movement, Mathf.Pow(_startBoostForce, 4.2f), out boost);
                if (boost) {
                    for (int i = 0; i < 5; i++)
                    {
                        AddExternalObject(BoostRing, transform.position, transform.rotation, color);
                    }
                    _gameSceneManager._cameraController.ShouldJitterCamera = true;
                }
            }

            if (_gameSceneManager.CustomInputManager.GetInputEventHold == InputEvent.GameInputEventHold.LIGHT_ATTACK && _animator._triggerHit == false && _animator._bumperActive == false)
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

        //AnimateBumper(1);

        FaceFoward(transform, movement);
        RegisterDoubleBoost();
        DoubleBoost(movement);

    }

    private void RegisterDoubleBoost()
    {
        if (_canBoost == false)
        {
            if (Input.GetKeyUp("joystick " + 1 + " button " + CustomInputManager.BOOST))
            {
                _canBoost = true;
            }
        }


        if (_canBoost)
        {
            if (_doubleBoosted == false)
            {
                _doubleBoostTimer += Time.deltaTime;

                if (_doubleBoostTimer < _doubleBoostDelay)
                {
                    if (_gameSceneManager.CustomInputManager.GetInputEventPress == InputEvent.GameInputEventPress.BOOST)
                    {
                        Logger.Log("Double Boosted!");
                        _doubleBoostTimer = 0;
                        _doubleBoosted = true;
                        _doubleBoostCoolDown = .5f;
                        _canBoost = false;
                    }
                } else
                {
                    _doubleBoostTimer = 0;
                    _doubleBoosted = false;
                    _canBoost = false;
                }
            }
        }

        if (_doubleBoosted)
        {
            if (_doubleBoostCoolDown > 0.0f)
            {
                _doubleBoostCoolDown -= Time.deltaTime;
            } else
            {
                _doubleBoosted = false;
                _canBoost = false;
            }
        }
    }


    public override void DoubleBoost(Vector3 movement)
    {
        if (_doubleBoosted)
        {
            Boost(movementDirection, Mathf.Pow(_startBoostForce, 3.0f));
            if (_boosted) {
                AddExternalObject(BoostRing, transform.position, transform.rotation, color);
            }
        }
    }





    void FixedUpdate()
    {
        if (isMoving(false, movementDirection)) {
            if (_animator._triggerHit) {
                AnimateBumper(50.0f * Time.fixedDeltaTime);
            }
        } else {
            _animator.ResetIndex();
        }

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
