using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Boost : Item
{
    [SerializeField] float boostForce = 100f;
    [SerializeField] float radius = 2.5f;
    private bool once = false;
    private bool done = false;
    public override void Start()
    {
        init();
        GetComponent<SphereCollider>().radius = radius;
    }
    public override void Update()
    {
        base.Update();
        if (used && !once)
        {
            Rigidbody rb = pogo.GetComponent<Rigidbody>();
            // send player upwards 
            rb.AddRelativeForce(Vector3.forward * boostForce);
            // allow collection of new item
            resetAttachpoint();
            // destroy after use
            // Destroy(gameObject);
            // ensure only run once
            once = true;
        }
    }
    // public override void OnTriggerEnter(Collider other) {
    //     // ? anyone in a certain radius of player gets pushed away from centre
    //     print("hit");
    //     if (used){
    //         // only perform knockback once when 
    //         if(!done){
    //             // apply knockback 
    //             knockbackPlayer(other);
    //             done = true;
    //         }
    //     }
    // }
    private void OnTriggerStay(Collider other)
    {
        // ? anyone in a certain radius of player gets pushed away from centre
        if (other.gameObject.tag == "Player" && other.gameObject.transform.root != pogo.transform.root)
        {
            print("hit");
            if (used)
            {
                // only perform knockback once when 
                if (!done)
                {
                    // apply knockback 
                    knockbackPlayer(other);
                    done = true;
                }
            }
        }
    }
}
