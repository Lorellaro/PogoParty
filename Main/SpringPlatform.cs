using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpringPlatform : MonoBehaviour
{
    [SerializeField] float forceUp;
    [SerializeField] float waitTime = 0.1f;
    [SerializeField] Animator myAnim;
    [SerializeField] ParticleSystem jumpVFX;
    [SerializeField] bool springRelevantToScriptObj;

    GameObject myPogostick;
    Rigidbody myPogoRB;
    Coroutine bounceDisabler;

    bool bounce;

    private void Awake()
    {
        myAnim = transform.root.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (bounce)
        {
            myPogoRB.velocity = transform.forward * forceUp * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SpringUp(other));
    }

    private IEnumerator SpringUp(Collider other)
    {
        yield return new WaitForSeconds(waitTime);

        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            if (other.gameObject.transform.root.GetComponent<PhotonView>().IsMine)
            {
                myPogostick = other.gameObject;
                myPogoRB = other.transform.root.GetChild(2).GetChild(0).GetComponent<Rigidbody>();
                bounce = true;

                if (bounceDisabler != null)
                {
                    StopCoroutine(bounceDisabler);
                }

                bounceDisabler = StartCoroutine(disableBounce());
            }

            if (springRelevantToScriptObj)
            {
                // shoot pogo stick upward
                //Rigidbody pogoRB = other.transform.root.GetChild(2).GetChild(0).GetComponent<Rigidbody>();// Gets pogostick from root
                //pogoRB.velocity = transform.forward * forceUp * Time.deltaTime;
            }
            else
            {
                // shoot pogo stick upward
                //Rigidbody pogoRB = other.transform.root.GetChild(2).GetChild(0).GetComponent<Rigidbody>();// Gets pogostick from root
                //pogoRB.velocity = transform.root.up * forceUp * Time.deltaTime;
            }

            //PlayVFX
            jumpVFX.Play();

            //Handle Anim
            myAnim.ResetTrigger("Bounce");
            myAnim.SetTrigger("Bounce");
            StartCoroutine(disableAnimTrigAfterTime());
        }
    }

    private IEnumerator disableBounce()
    {
        yield return new WaitForSeconds(0.15f);//bounce duration
        bounce = false;
    }

    private IEnumerator disableAnimTrigAfterTime()
    {
        yield return new WaitForSeconds(0.5f);
        myAnim.ResetTrigger("Bounce");
    }
}
