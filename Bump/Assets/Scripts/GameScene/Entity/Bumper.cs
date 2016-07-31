using UnityEngine;
using System.Collections;
using System;
public class Bumper : MovementHandler {

    // Use this for initialization

    MovementHandler movementHandler;
    void Start () {

        if (GetComponentInParent<MovementHandler>() != null)
        {
            movementHandler = GetComponentInParent<MovementHandler>();
        }

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
                float impulse = (Constants.BUMPER_IMPULSE_MAG * Mathf.Sqrt(GetComponentInParent<Rigidbody2D>().velocity.SqrMagnitude())) + Constants.BASE_BUMPER_IMPLUSE;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * impulse);
            
                if (col.gameObject.name.Equals("Player") == false && transform.parent.name.Equals("Bot(Clone)") == false)
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
}
