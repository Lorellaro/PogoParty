using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cannon : MonoBehaviour
{
    [SerializeField] float timeBtwShots;
    [SerializeField] bool hasActivationRange;
    [SerializeField] float activationRange;
    [SerializeField] Leaderboard leaderboard; //Holds all player transforms
    [SerializeField] List<Transform> playerTransforms = new List<Transform>();
    [SerializeField] GameObject cannonBall;
    [SerializeField] Transform firepos;

    GiveRandomAudioClip giveRandomAudioClip;
    Transform currentClosestTransform;
    Vector3 currentClosestPos;
    float timeSinceLastFire;

    private void OnEnable()
    {
        UpdatePlayerTransforms();
        StartCoroutine(updateTransformRecursively());
        giveRandomAudioClip = GetComponent<GiveRandomAudioClip>(); 
    }

    // Update is called once per frame
    void Update()
    {

        Transform nearestTarget = GetClosestEnemy(playerTransforms.ToArray());
        if (nearestTarget)
        {
            transform.LookAt(nearestTarget);
        }

        //in range and enough time has passed since last shot
        if (Vector3.Distance(firepos.position, currentClosestPos) < activationRange && timeSinceLastFire <= 0)
        {
            //canfire
            fireBullet();
            timeSinceLastFire = timeBtwShots;
        }

        timeSinceLastFire -= Time.deltaTime;
    }

    private void UpdatePlayerTransforms()
    {
        List<GameObject> playerObjs = leaderboard.GetAllPlayers();

        
        for (int i = 0; i < playerObjs.Count; i++)
        {
            playerTransforms.Add(playerObjs[i].transform.GetChild(2).GetChild(0));
        }

    }

    private IEnumerator updateTransformRecursively()
    {
        UpdatePlayerTransforms();
        yield return new WaitForSeconds(timeBtwShots);

        StartCoroutine(updateTransformRecursively());
    }

    private void fireBullet()
    {
        GameObject cannonBal = Instantiate(cannonBall, firepos.position, Quaternion.identity);//PhotonNetwork.Instantiate(cannonBall.name, firepos.position, Quaternion.identity);
        cannonBal.GetComponent<CannonBall>().Direction = -(firepos.position - currentClosestPos).normalized;
        cannonBal.GetComponent<CannonBall>().target = currentClosestTransform;
        giveRandomAudioClip.changeAndPlayClip();
    }

    //More efficient way of getting closest object
    Transform GetClosestEnemy(Transform[] _playerTransforms)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in _playerTransforms)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        currentClosestPos = bestTarget.position;
        currentClosestTransform = bestTarget;

        return bestTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(firepos.position, activationRange);
    }
}
