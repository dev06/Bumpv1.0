using UnityEngine;
using System.Collections;

public class BoundaryRing : MonoBehaviour {

    public GameObject BoostRing; 
    void OnCollisionEnter2D(Collision2D col)
    {
        Instantiate(BoostRing, col.gameObject.transform.position, Quaternion.identity); 
    }
}
