using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatforms : MonoBehaviour
{
    [SerializeField] float forceUp;
    [SerializeField] float bounceOffset;
    [SerializeField] bool maintainMomentum;
    [SerializeField] float momentumMultiplier;
    [SerializeField] Vector3 directionOffset;
    //[SerializeField] float waitTime = 0.1f;
    //[SerializeField] Animator myAnim;


    private void Awake()
    {
        //myAnim = transform.root.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(BounceUp(other));
    }

    private IEnumerator BounceUp(Collider other)
    {
        //yield return new WaitForSeconds(waitTime);
        yield return new WaitForSeconds(0);

        //some variation whether right or left idc

        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            // shoot pogo stick upward
            Rigidbody pogoRB = other.transform.root.GetChild(2).GetChild(0).GetComponent<Rigidbody>();// Gets pogostick from root

            Vector3 sprayDir = new Vector3(Random.Range(-bounceOffset, bounceOffset), 1f, Random.Range(-bounceOffset, bounceOffset));
            Vector3 bounceDirection = transform.up + sprayDir;

            if (maintainMomentum)
            {
                float altmomentumMultiplier = pogoRB.velocity.magnitude;

                pogoRB.velocity = (transform.up + directionOffset) * forceUp * Time.deltaTime;

                pogoRB.velocity += transform.up * (momentumMultiplier * altmomentumMultiplier) * Time.deltaTime;
            }
            else
            {
                pogoRB.velocity = bounceDirection.normalized * forceUp * Time.deltaTime;
            }

            //PlayVFX
            //jumpVFX.Play();

            //Handle Anim
            //myAnim.ResetTrigger("Bounce");
            //myAnim.SetTrigger("Bounce");
            StartCoroutine(disableAnimTrigAfterTime());
        }
    }

    private IEnumerator disableAnimTrigAfterTime()
    {
        yield return new WaitForSeconds(0.5f);
        //myAnim.ResetTrigger("Bounce");
    }
}
