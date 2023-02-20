using System;
using System.Collections;
using Main.GameHandlers;
using Main.Level.Race;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoulderThrower : MonoBehaviour
{
    // public static BoulderThrower Instance;
    [SerializeField] GameObject pelletPrefab;
    [SerializeField] Transform targetObj;
    [SerializeField] float minTime;
    [SerializeField] private float maxTime;
    [SerializeField] private int maxBoulders = 10;
    [SerializeField] private Transform minPosition, maxPosition;
    private int boulderCount;

    private Vector3 target;
    
    private enum BoostDirection
    {
        Forward,
        Backward,
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] private BoostDirection boostDirection;

    private Vector3 boostDir;

    

    private IEnumerator ThrowPellets()
    {
        float waitTime = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(waitTime);
        if (boulderCount < maxBoulders)
        {
            var minPositionPosition = minPosition.position;
            var maxPositionPosition = maxPosition.position;
            Vector3 spawnPos = new Vector3
            (
                Random.Range(minPositionPosition.x, maxPositionPosition.x),
                Random.Range(minPositionPosition.y, maxPositionPosition.y),
                Random.Range(minPositionPosition.z, maxPositionPosition.z)
            );
            //GameObject boulder = PhotonNetwork.Instantiate(pelletPrefab.name, spawnPos, Quaternion.identity);
            GameObject boulder = Instantiate(pelletPrefab, spawnPos, Quaternion.identity);
            BoulderNew boulderNew = boulder.GetComponent<BoulderNew>();
            boulderNew.boulderThrower = this;
            boulderNew.throwerBoostDir = boostDir;
            boulderCount++;
            // boulder.GetComponent<BoulderNew>().SetDirections(target, transform.position);
        }

        StartCoroutine(ThrowPellets());
    }

    private void Start()
    {
        if (RoundManager.Instance == null)
        {
            Activate();
        }
        RoundManager.Instance.onRoundManagerReady += Activate;
        target = targetObj.position - transform.position;
        target = target.normalized;
        switch (boostDirection)
        {
            case BoostDirection.Up:
                boostDir = Vector3.up;
                break;
            case BoostDirection.Down:
                boostDir = Vector3.down;
                break;
            case BoostDirection.Forward:
                boostDir = Vector3.forward;
                break;
            case BoostDirection.Backward:
                boostDir = Vector3.back;
                break;
            case BoostDirection.Left:
                boostDir = Vector3.left;
                break;
            case BoostDirection.Right:
                boostDir = Vector3.right;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(boostDirection), boostDirection, null);
        }
    }
    public Vector3 GetTarget()
    {
      return target;
    }

    public Vector3 GetKnockBackDir()
    {
      return transform.position;
    }

    public void BoulderDestroyed()
    {
      boulderCount--;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetObj.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawWireSphere(minPosition.position, 0.5f);
        Gizmos.DrawWireSphere(maxPosition.position, 0.5f);
    }

    private void Activate()
    {
        StartCoroutine(ThrowPellets());
    }

    
}
