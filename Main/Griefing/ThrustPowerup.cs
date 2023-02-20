using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GriefingSystem;

namespace GriefingSystem{

    public class ThrustPowerup : Powerup
    {
        [SerializeField] float thrustForce;
        [SerializeField] float boostTime;
        [SerializeField] GameObject ThrustVFX;

        Rigidbody rb;
        Vector3 dir;
        bool viewIsMine;

        // Start is called before the first frame update
        void Start()
        {
            rb = myPogoStickTransform.GetComponent<Rigidbody>();

            //thrust bot stick
            dir = myPogoStickTransform.GetChild(0).GetChild(0).forward;

            Destroy(gameObject, boostTime);

            viewIsMine = myPogoStickTransform.root.GetComponent<PhotonView>().IsMine;

            Transform pogoJumpVFX1 = myPogoStickTransform.Find("VFX PogoJump").transform;
            Transform pogoJumpVFX2 = myPogoStickTransform.Find("VFX PogoJump (1)").transform;

            PhotonNetwork.Instantiate(ThrustVFX.name, pogoJumpVFX1.position, pogoJumpVFX1.rotation);
            PhotonNetwork.Instantiate(ThrustVFX.name, pogoJumpVFX2.position, pogoJumpVFX2.rotation);
        }

        private void FixedUpdate()
        {
            if (viewIsMine)
            {
                rb.velocity += dir * thrustForce * Time.deltaTime;
            }
        }
    }

}