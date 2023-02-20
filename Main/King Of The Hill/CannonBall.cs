using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public Vector3 Direction;
    [SerializeField] float speed = 100;
    [SerializeField] float courseCorrectionSpeed;
    [SerializeField] float courseCorrectionDuration;
    [SerializeField] Rigidbody rb;
    [SerializeField] float lifetime;

    bool courseCorrection = true;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        StartCoroutine(StopCourseCorrection());
    }

    private void FixedUpdate()
    {
        rb.velocity += Direction * speed * Time.deltaTime;

        if (courseCorrection)
        {
            rb.velocity += ((transform.position - target.position).normalized * -1) * courseCorrectionSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = true;
        courseCorrection = false;
    }

    private IEnumerator StopCourseCorrection()
    {
        yield return new WaitForSeconds(courseCorrectionDuration);
        courseCorrection = false;
    }
}
