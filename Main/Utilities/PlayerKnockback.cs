using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Collider))]
public class PlayerKnockback : MonoBehaviour
{

    [Tooltip("enable to swap functionality to knock you back in the opposite direction that you(the player) were hit by")]
    [SerializeField] private bool oppositeKnockback = false;
    [SerializeField] private Transform knockbackTransform;

    [SerializeField] private float knockBackForce;
    [SerializeField] private float upwardsKnockBackForce;
    [SerializeField] private float knockbackTime = 0.15f;
    [SerializeField] GameObject collisionVFX;
    [SerializeField] GameObject collisionSFX;

    private List<int> playerIDs = new List<int>();
    private List<float> playerCollisionTimes = new List<float>();
    private Vector3 _knockBackDirection;
    private bool knockback;
    private CameraShakeController cameraShakeScript;
    private Rigidbody myPogostickRB;
    private Coroutine stopStartKnockbackTime;

    // Start is called before the first frame update
    void Start()
    {
        cameraShakeScript = GameObject.FindGameObjectWithTag("Game Controller").GetComponent<CameraShakeController>();

        if (!cameraShakeScript)
        {
            Debug.LogWarning("You need to setup the game controller bozo");
        }

        if (oppositeKnockback) { return; }

        if (knockbackTransform == null)
        {
            Debug.LogError("knockBackTransform has not been set! GameObject: " + gameObject.name);
            return;
        }
    }

    private void FixedUpdate()
    {
        //If not set yet or if not running on my instance
        if(myPogostickRB == null || !myPogostickRB.transform.root.GetComponent<PhotonView>().IsMine) { return; }

        //is set to true when a player is hit
        if (knockback)
        {
            if (oppositeKnockback)
            {
                myPogostickRB.velocity = _knockBackDirection.normalized * knockBackForce + Vector3.up * upwardsKnockBackForce * Time.deltaTime;
            }
            else
            {
                myPogostickRB.velocity = _knockBackDirection * knockBackForce + Vector3.up * upwardsKnockBackForce  * Time.deltaTime;
            }
        }
    }

    private void KnockBackApplier(Collider other)
    {
        //Opposite knockback
        if (oppositeKnockback && other.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("My brother in Christ, you cannot use player knockback opposite knockback without setting the box collider trigger to OFF!");
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject playerObj = other.transform.root.gameObject.transform.GetChild(2).GetChild(0).gameObject;
            //Rigidbody playerRB = playerObj.GetComponent<Rigidbody>();
            // var velocity = playerRB.velocity;
            //playerRB.velocity = _knockBackDirection * knockBackForce + Vector3.up * upwardsKnockBackForce  * Time.deltaTime;
            // playerRB.velocity = velocity;

            myPogostickRB = playerObj.GetComponent<Rigidbody>();
            if(stopStartKnockbackTime == null)
            {
                stopStartKnockbackTime = StartCoroutine(startStopKnockback());
            }
        }
    }
    
    private void KnockBackApplier(Collision collision)
    {
        //Opposite Knockback
        if (oppositeKnockback && collision.gameObject.CompareTag("Player"))
        {
            GameObject playerObj = collision.transform.root.gameObject.transform.GetChild(2).GetChild(0).gameObject;
            myPogostickRB = playerObj.GetComponent<Rigidbody>();

            _knockBackDirection = -1 * (collision.GetContact(0).point - playerObj.transform.position);

            if (stopStartKnockbackTime == null)
            {
                stopStartKnockbackTime = StartCoroutine(startStopKnockback());
            }
            return;
        }
        if (!oppositeKnockback && collision.gameObject.CompareTag("Player"))
        {
            GameObject playerObj = collision.transform.root.gameObject.transform.GetChild(2).GetChild(0).gameObject;
            myPogostickRB = playerObj.GetComponent<Rigidbody>();

            _knockBackDirection = (collision.GetContact(0).point - playerObj.transform.position);

            if (stopStartKnockbackTime == null)
            {
                stopStartKnockbackTime = StartCoroutine(startStopKnockback());
            }
            return;
        }

        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     GameObject playerObj = collision.transform.root.gameObject.transform.GetChild(2).GetChild(0).gameObject;
        //     Rigidbody playerRB = playerObj.GetComponent<Rigidbody>();
        //     var velocity = playerRB.velocity;
        //     velocity = _knockBackDirection * knockBackForce + Vector3.up * upwardsKnockBackForce  * Time.deltaTime;
        //     playerRB.velocity = velocity;
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        KnockBackApplier(other);
        StartCoroutine(cameraShakeScript.BurstShake(1.9f, 0.25f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) { return; }
        KnockBackApplier(collision);
        StartCoroutine(cameraShakeScript.BurstShake(1.9f, 0.25f));

        if (!collisionVFX) { return; }
        //Spawn VFX
        //PhotonNetwork.Instantiate(collisionVFX.name, collision.GetContact(0).point, Quaternion.identity);
        spawnVFX(collision);
    }

    //needs to only spawn in one per player hit within a small time frame 
    private void spawnVFX(Collision collision)
    {
        int collisionViewID = collision.transform.root.GetComponent<PhotonView>().ViewID;

        //if first time being hit
        if(playerIDs.Count == 0)
        {
            //play vfx
            Instantiate(collisionVFX, collision.GetContact(0).point, Quaternion.identity);
            //Play SFX
            if (collisionSFX != null)
            {
                PhotonNetwork.Instantiate(collisionSFX.name, collision.GetContact(0).point, Quaternion.identity);
            }

            //start this player's timer
            playerIDs.Add(collisionViewID);
            playerCollisionTimes.Add(0.5f);

            return;
        }



        //if returning id reset it's timer

        for (int i = 0; i < playerIDs.Count; i++)
        {

            //view id is already in list
            if (collisionViewID == playerIDs[i])
            {
                //Check if player has been hit recently
                if (playerCollisionTimes[i] < 0.2)
                {
                    Instantiate(collisionVFX, collision.GetContact(0).point, Quaternion.identity);

                    //Play SFX
                    if (collisionSFX != null)
                    {
                        PhotonNetwork.Instantiate(collisionSFX.name, collision.GetContact(0).point, Quaternion.identity);
                    }

                    //Restart this player's timer
                    playerCollisionTimes[i] = 0.5f;
                }

                return; // return if ID is already present
            }

        }
        //if time since last hit with this player id is less than  minimum wait time then instantiate
        Instantiate(collisionVFX, collision.GetContact(0).point, Quaternion.identity);

        //Play SFX
        if (collisionSFX != null)
        {
            PhotonNetwork.Instantiate(collisionSFX.name, collision.GetContact(0).point, Quaternion.identity);
        }

        //if new id add it
        playerIDs.Add(collisionViewID);
        playerCollisionTimes.Add(0.5f);


    }

    private IEnumerator startStopKnockback()
    {
        knockback = true;
        yield return new WaitForSeconds(knockbackTime);
        knockback = false;
        stopStartKnockbackTime = null;
    }

    private void Update()
    {
        //reduce whoever's time needs it
        for (int i = 0; i < playerCollisionTimes.Count; i++)
        {
            if (playerCollisionTimes[i] > 0)
            {
                playerCollisionTimes[i] -= Time.deltaTime;
            }
        }
    }
}
