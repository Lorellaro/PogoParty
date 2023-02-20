using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCMover : MonoBehaviour
{
    private Rigidbody _head;
    private bool _canAddForce;

    private void Awake()
    {
        _head = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(AllowAddForce());
        Random.InitState(transform.parent.gameObject.name.Length);
    }

    private void FixedUpdate()
    {
        if (_canAddForce)
        {
            float random = Random.Range(0, 9999999999);
            if (random > 99999999)
            {
                _head.AddForce(transform.up * 5, ForceMode.Force);
            }
            
            StartCoroutine(AllowAddForce());
        }

    }

    private IEnumerator AllowAddForce()
    {
        _canAddForce = false;
        yield return new WaitForSeconds(Random.Range(2, 5));
        _canAddForce = true;
    }
}
