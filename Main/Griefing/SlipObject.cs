using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SlipObject : Item
{
    [SerializeField] float force = 1.2f;
    private RaycastHit hit;
    private bool once = false;
    private bool done = false;
    private float start = -1;
    private Vector3 hitPos = new Vector3(float.PositiveInfinity, 0, 0);
    private float speed = 5f;
    private bool slipped = false;
    
    public override void Start()
    {
        canHold = false;
        duration = -1;
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        if(used){
            transform.rotation = Quaternion.identity;
            GetComponent<Collider>().enabled = true;
            GetComponent<Renderer>().enabled = true;
            // find floor 
            if (!once)
            {
                Physics.Raycast(transform.position + new Vector3(0, 50, 0), Vector3.down, out hit, 100.0f, LayerMask.GetMask("Ground"));
                hitPos = hit.point;
                once = true;
            }
            // drop to floor if in the air
            if (hitPos.x != float.PositiveInfinity)
            {
                transform.position = Vector3.MoveTowards(transform.position, hitPos, speed * Time.deltaTime);
            }
            
        }
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (used)
        {
            if(!done){
                // wait time before allowing collisions allows player to still be hit later
                start = Time.time;
                done = true;
            }
            if(Time.time >= start + 0.8f){
                if(slipped){
                    // apply knockback
                    Rigidbody rb = pogo.GetComponent<Rigidbody>();
                    Vector3 dir = transform.position - other.transform.position;
                    rb.velocity = dir.normalized * force + Vector3.up * 0.03f;
                    Destroy(gameObject, 0.3f); 
                    slipped = true;
                }
                
            }
        }
    }
}


