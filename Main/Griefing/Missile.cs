using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Missile : Item
{
    // area of effect of item
    [SerializeField] private float radius = 5f;
    // how fast the item will travel
    [SerializeField] private float speed = 5f;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionRate = 0.4f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float turnSpeed = 0.07f;
    [SerializeField] private float initInvincibilityTime = 0.7f;
    [SerializeField] private float maxLifeSpan = 8f;
    private bool found = false;
    private bool once = false;
    private Transform currentClosestTransform;
    private Vector3 currentClosestPos;
    private Transform target;
    private bool done = false;
    private bool ran = false;
    private bool oneTime = false;
    private Quaternion rot = Quaternion.identity;
    private Vector3 dir = Vector3.zero;
    private Rigidbody rb;
    private float timeAliveFromRelease;

    public override void Start()
    {
        canHold = true;
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        if (!playerView.IsMine) { return; }
        base.Update();

        print("Past base.update");
        if (!used) transform.forward = (transform.parent.parent.right + transform.parent.parent.forward).normalized;
        print("past !USED");
        if (used)
        {
            print("USED");
            timeAliveFromRelease += Time.deltaTime;
            print(" Time alive form release");

            if (timeAliveFromRelease > maxLifeSpan)
            {
                Explode();
            }

            if (playerView.IsMine)
            {
                print("Player vuew is mine");
                // ? check the left stick for direction forward or backward
                // ? forward has target lock
                // ? backward has no target and flies in direction opposite of player

                // check what game mode it is 
                // next position and in front? ignore radius? (race mode)
                if (gameMode == "Race_Levelfjafjsil")
//                if (gameMode == "Race_Level")
                {
                    Debug.LogWarning("Race Mode has not been implemented yet");
                    // get positions
                    // pos + 1
                    // get playerView position
                    // find player view in position
                    // check if first 
                    // if(pos == 1){
                    //     target = new GameObject().transform;
                    // }

                    // check if not first
                    // else{
                    //     find position -1 
                    //     target = positionsArray[pos-1].transform;  

                    // }
                }

                // get enemy closest to player
                else
                {
                    // try to get target
                    try
                    {
                        // find all players in radius and select closest one
                        if (!once)
                        {
                            GameObject[] playerList = allPlayers.ToArray();
                            Transform[] playerTransList = new Transform[playerList.Length];
                            for (int i = 0; i < playerList.Length; i++) playerTransList.SetValue(playerList[i].transform, i);
                            GetClosestEnemy(playerTransList);
                            target = currentClosestTransform;
                            once = true;
                        }
                    }
                    // if null thrown give target
                    catch (System.NullReferenceException)
                    {
                        if (!once)
                        {
                            // create a fake target
                            target = new GameObject().transform;
                            once = true;

                        }
                    }

                }

                // do once
                if (!done)
                {
                    // fly up
                    dir = transform.forward;
                    resetAttachpoint();
                    done = true;
                }

                // wait time

                print("Time alive");
                if (timeAliveFromRelease > initInvincibilityTime)
                {
                    // if there is no target 
                    if (target.childCount == 0)
                    {

                        // change direction to fly straight in same direction
                        dir = (target.GetChild(2).GetChild(0).position - transform.position).normalized;
                    }
                    // target found
                    else
                    {
                        Debug.LogWarning("bitches :o");
                        dir = (target.GetChild(2).GetChild(0).position - transform.position).normalized;
                    }
                }
                print("Finished time alive");
            }
            Cooldown();
            if (finished)
            {
                if (!oneTime)
                {
                    // make invisible until explosion destroys this
                    GetComponent<Renderer>().enabled = false;
                    GetComponent<Collider>().enabled = false;
                    // destroy target if created artificially
                    if (target){
                        if (target.childCount == 0)
                        {
                            //Destroy(target.gameObject);
                        }
                    }
                    
                    oneTime = true;
                }
            }
        }
        print("Finished update");
    }

    private void FixedUpdate()
    {
        if (used)
        {
            // stop movement of missile when exploded
            if (!oneTime)
            {
                Quaternion rotGoal = Quaternion.LookRotation(dir);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed));
                rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);

                //float angle = 5 * Time.deltaTime;
                //Vector3 rot = new Vector3(0, 0, 1 * Time.deltaTime);
                
                //rb.MoveRotation(Quaternion.Euler(rot));
            }
        }
    }
    // explode on impact
    public override void OnTriggerEnter(Collider collision)
    {
        print("BOOM");
        //Wait x amount of time before it can explode
        if (timeAliveFromRelease > initInvincibilityTime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        //Disable self and explode
        resetAttachpoint();
        Destroy(gameObject, 3f);
        PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    //More efficient way of getting closest object
    Transform GetClosestEnemy(Transform[] _playerTransforms)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        for (int p = 0; p < _playerTransforms.Length; p++)
        {
            // if player using item is selected then skip
            if (_playerTransforms[p].root.GetComponent<PhotonView>().ViewID == playerView.ViewID)
            {
                continue;
            }
            Vector3 directionToTarget = _playerTransforms[p].position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            // outside of radius
            if (dSqrToTarget >= radius)
            {
                continue;
            }
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = _playerTransforms[p];
            }
        }

        currentClosestPos = bestTarget.position;
        currentClosestTransform = bestTarget;

        return bestTarget;
    }
}
