using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float shakeOffset = 1f;
    [SerializeField] private float stableTime = 2.5f;
    [SerializeField] private float waitTime = 5f;
    [SerializeField] private float dropDist = 1000f;
    [SerializeField] private float dropTime = 1.5f;
    [SerializeField] private float platformResetMoveSpeed = 0.5f;

    private bool _hasCollided = false;
    private Vector3 startPos;
    private Vector3 startScale;

    private void Awake()
    {
        startPos = transform.position;
        startScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player) && !_hasCollided)
        {
            _hasCollided = true;
            StartCoroutine(Part1());
        }
    }

    private IEnumerator Part1()
    {
        //Shake
        Vector3 newPos = new Vector3(gameObject.transform.position.x + shakeOffset, gameObject.transform.position.y, gameObject.transform.position.z + shakeOffset);
        LeanTween.move(gameObject, newPos, stableTime/10).setEaseShake().setLoopCount(10);

        yield return new WaitForSeconds(stableTime);

        //Drop and shrink

        LeanTween.moveY(gameObject, startPos.y - dropDist, dropTime).setEaseInOutSine();
        LeanTween.scale(gameObject, Vector3.zero, dropTime).setEaseInOutSine();

        yield return new WaitForSeconds(waitTime);

        //Wait x amount of time until returning

        //Reset logic
        _hasCollided = false;

        //Reset Scale
        LeanTween.scale(gameObject, startScale, platformResetMoveSpeed).setEaseInOutSine();

        //Reset Position
        LeanTween.move(gameObject, startPos, platformResetMoveSpeed).setEaseInOutSine();
    }
}
