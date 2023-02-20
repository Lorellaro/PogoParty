using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PogoStickAnim : MonoBehaviour
{
    [SerializeField] Animator lleywlnAnimator;
    [SerializeField] Animator anim;
    [SerializeField] PhotonView view;

    PogoStickPhysics pogoStickPhysicsScript;
    private InputManager _inputManager;

    private void Start()
    {
        //anim = GameObject.Find("PogoStick").GetComponent<Animator>();
        //lleywlnAnimator = GameObject.Find("Llwelyn_Animated").transform.GetChild(1).GetComponent<Animator>();
        //Debug.Log(lleywlnAnimator.name);

        pogoStickPhysicsScript = GetComponent<PogoStickPhysics>();

        if (view.IsMine)
        {
            _inputManager = InputManager.Instance;

            if (pogoStickPhysicsScript == null) return;
            if (pogoStickPhysicsScript.isDisabled) { return; }
            _inputManager.OnStartJump += JumpStart;
            _inputManager.OnPerformedJump += JumpPerformed;
            _inputManager.OnEndJump += JumpEnd;
        }

        //view = GetComponent<PhotonView>();

    }

    private void JumpStart()
    {
        //anim = GameObject.Find("PogoStick").GetComponent<Animator>();
        //anim.SetBool("isMouseClickHeldDown", true);

    }

    private void JumpPerformed()
    {

        anim.SetBool("isMouseClickHeldDown", true);
    }

    private void JumpEnd()
    {
        lleywlnAnimator.SetTrigger("Extend");
        anim.SetBool("isMouseClickHeldDown", false);
    }

    // Update is called once per frame
     void Update()
     {
         if(!view.IsMine) return;
         if (pogoStickPhysicsScript == null) return;
         if (pogoStickPhysicsScript.isDisabled) { return; }
    
             if (_inputManager.jumpHeld)
             {
                 anim.SetBool("isMouseClickHeldDown", true);
             }
     }
}
