using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FakeBox : Item
{
    private GameObject explosion;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionRate = 0.4f;
    private RaycastHit hit;
    private bool once = false;
    private bool done = false;
    private float start = -1;
    private Vector3 hitPos = new Vector3(float.PositiveInfinity, 0, 0);
    private float speed = 4f;
    
    public override void Start()
    {
        canHold = false;
        duration = -1;
        init();
        // start "inactive"
        GetComponent<Collider>().enabled = false;
        foreach(Transform child in transform){
            child.GetComponent<Renderer>().enabled = false;
        }
        // determine logic
        gameMode = GameController.gameMode;
            
        // last child is explosion 
        explosion = transform.GetChild(transform.childCount-1).gameObject;
        explosion.SetActive(false);
    }
    public override void Update()
    {
        base.Update();
        if(used){
            transform.rotation = Quaternion.Euler(0, 0, 180);
            GetComponent<Collider>().enabled = true;
            foreach(Transform child in transform){
                child.GetComponent<Renderer>().enabled = true;
            }
            // find floor 
            if (!once)
            {
                Physics.Raycast(transform.position + new Vector3(0, 50, 0), Vector3.down, out hit, 100.0f, LayerMask.GetMask("Ground"));
                hitPos = hit.point + new Vector3(0, 0.1965611f, 0);
                once = true;
            }
            // drop to floor if in the air
            if (hitPos.x != float.PositiveInfinity)
            {
                transform.position = Vector3.MoveTowards(transform.position, hitPos, speed * Time.deltaTime);
            }

            if (explosion.activeSelf)
            {
                // Destroy when explosion is greater than the explosion radius
                if (explosion.transform.localScale.x >= explosionRadius)
                {
                    explosion.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
                    Destroy(gameObject, 0.5f);
                }
                // explosion increses from point at steady rate
                explosion.transform.localScale += new Vector3(explosionRate, explosionRate, explosionRate);
                explosion.GetComponent<SphereCollider>().radius = explosion.transform.localScale.x;
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
                GetComponent<Collider>().enabled = false;
                foreach(Transform child in transform){
                    child.GetComponent<Renderer>().enabled = false;
                }
                explosion.SetActive(true);
            }
        }
    }
}
