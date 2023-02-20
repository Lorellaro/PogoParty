using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] float pelletForce = 1000f;
    [SerializeField] float forceApplicationTimer;
    [SerializeField] float knockupForce = 1000f;
    [SerializeField] private float destroyTime = 5f;

    Vector3 direction;
    Vector3 knockBackDirection;
    Rigidbody rb;
    float currentTime;
    public GameObject destructible;
    bool destroyed = false;
    bool canPlay = true;
    bool audioPlaying = false;
    public ParticleSystem pSystem;
    AudioSource rockColliding;

    private void Start()
    {
        rockColliding = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        direction = transform.parent.gameObject.GetComponent<pelletThrower>().getTarget();//set pellet throwing direction to parent's target
        StartCoroutine(destroyBoulder());
        knockBackDirection = transform.parent.GetChild(1).forward;
        rb.AddTorque(transform.right * 20);
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        if(!destroyed){
            // if time is less than the max time 
            if (currentTime < forceApplicationTimer)
            {
                // apply force to rigidbody
                rb.velocity += direction * pelletForce * Time.deltaTime;
            }
        }
        
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!destroyed){
            if (collision.gameObject.CompareTag("Player"))
            {
                GameObject playerObj = collision.gameObject;
                Rigidbody playerRB = playerObj.GetComponent<Rigidbody>();
                playerRB.velocity += knockBackDirection * knockupForce * Time.deltaTime;
            }
           
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground" && canPlay){
                StartCoroutine(playParticles());
                pSystem.Play();
            }
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground" && !audioPlaying){
                StartCoroutine(playSound());
                rockColliding.Play();
            }
        }
        
    }
    private IEnumerator destroyBoulder(){
        // wait then destroy
        yield return new WaitForSeconds(destroyTime);
        destroyed = true;
        // making the boulder disappear but not destroyed
        gameObject.GetComponentInChildren<Collider>().enabled = false;
        gameObject.GetComponentInChildren<Renderer>().enabled = false;
        // create cracked version of pellet
        Instantiate(destructible, transform.position, transform.rotation);
        
        // destroy last to not destroy script
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private IEnumerator playParticles(){
        canPlay = false;
        yield return new WaitForSeconds(0.3f);
        canPlay = true;
    }
    private IEnumerator playSound(){
        audioPlaying = true;
        yield return new WaitForSeconds(rockColliding.clip.length);
        audioPlaying = false;
    }
}
