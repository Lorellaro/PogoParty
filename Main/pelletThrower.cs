using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pelletThrower : MonoBehaviour
{
    [SerializeField] GameObject pelletPrefab;
    [SerializeField] Transform targetObj;
    [SerializeField] public Transform pelletKnockbackDir;
    [SerializeField] float timeBtwPellets;
    [SerializeField] private float delayTime;

    Vector3 target;

    private IEnumerator throwPellets()//Call recursively to fire pellet  every x amount of time
    {
        yield return new WaitForSeconds(timeBtwPellets);
        Instantiate(pelletPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(throwPellets());
    }

    private IEnumerator StartThrowing()
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(throwPellets());
    }

    public Vector3 getTarget()
    {
        return target;
    }

    private void Start()
    {
        target = targetObj.position - transform.position;
        target = target.normalized;
        if (delayTime <= 0)
        {
            StartCoroutine(throwPellets());
        }
        else
        {
            StartCoroutine(StartThrowing());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetObj.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
