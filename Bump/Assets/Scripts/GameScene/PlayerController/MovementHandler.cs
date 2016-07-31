﻿using UnityEngine;
using System.Collections;

public class MovementHandler : EntityMovementHandler
{

    #region--- PRIVATE MEMBERS---

    private bool isUsingController = false;
    private float _startBoostForce = 1000f;

    #endregion---/PRIVATE MEMEBERS


    public GameObject BoostRing;
    [HideInInspector]
    public Vector2 movementDirection;
    public float Velocity;



    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        pCol2D = transform.FindChild("BumperCollider").GetComponent<PolygonCollider2D>();
        _animator = GetComponent<CustomAnimator>();
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
        rg2d.AddForce(movement * Velocity);
        movementDirection = movement;

        if (GameSceneManager.isControllerConnected)
        {
            if (Input.GetButtonDown(Constants.CONTROLLER_X_BUTTON))
            {
                bool boost;
                Boost(movement, _startBoostForce, out boost);
                if (boost) {
                    AddExternalObject(BoostRing, transform.position, transform.rotation);
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
                    AddExternalObject(BoostRing, transform.position, transform.rotation);
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
                AnimateBumper(1);
            }
        } else {
            _animator.ResetIndex();
        }

        //AnimateBumper(1);
        FaceFoward(transform, movement);
    }


    public bool IsBumperActive {
        get {return _animator._bumperActive; }
    }
}
