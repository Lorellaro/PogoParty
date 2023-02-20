using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldRBtoRB : MonoBehaviour
{
    [SerializeField] float pullStrength;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxDepenetrationVelocity = 0;
    }

    void OnCollisionStay(Collision colObj)
    {
        //We check to see if the surface we collided with has the tag of our hole, so we don't trigger this on any collision surface
        if (colObj.gameObject.tag == "Platter")
        {
            //Set only the Y axis of the velocity to a custom value, while leaving the existing x/z velocities intact by using them as the input value
            rb.velocity = new Vector3(rb.velocity.x, pullStrength, rb.velocity.z);
        }
    }
}
