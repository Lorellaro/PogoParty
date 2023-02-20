using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hammer : Item
{

    [SerializeField] float rotSpeed;
    [SerializeField] float yOffsetFromPlayer;

    Rigidbody rb;
    float sTime;
    InputManager inputs;
    bool press = false;
    bool once = false;
    Collider[] colliders;
    Quaternion initialRotation;
    float input = 0;

    private void Awake()
    {
        // get instance of input manager
        if (inputs == null)
        {
            inputs = InputManager.Instance;
        }
    }
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        initialRotation = rb.rotation;
        PhotonView photonView = transform.GetComponent<PhotonView>();
        // photonView.RPC("UpdatePlayerList", RpcTarget.AllBuffered);
        // photonView.RPC("AttemptClaimHammer", RpcTarget.AllBuffered);
        //call rpc buffered to claim ownership of the hammer and have all other players know that this player owns it


        colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        //Get my pogo stick
        //GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("PlayerRoot");


    }

    // [PunRPC]
    // private void AttemptClaimHammer()
    // {
    //     if (isInUse) { return; }

    //     //else claim for self
    //     if (allPlayers[0].GetComponent<PhotonView>().ViewID == 1001)
    //     {
    //         attachPoint = allPlayers[0].gameObject.transform.GetChild(2).GetChild(0);
    //         myPogostickTrans = allPlayers[0].transform.GetChild(2).GetChild(0);
    //     }

    //     for (int i = 0; i < allPlayers.Count; i++)
    //     {
    //         if (allPlayers[i].GetComponent<PhotonView>().IsMine)
    //         {
    //             //myPogostickTrans = allPlayers[i].transform.GetChild(2).GetChild(0);
    //         }
    //     }
    // }



    public override void Update()
    {
        if (inputs.interactHeld)
        {
            press = true;
            if (!once)
            {
                once = true;
                sTime = Time.time;
                transform.parent = null;
                GetComponent<Renderer>().enabled = true;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                foreach (Collider col in colliders)
                {
                    col.enabled = true;
                }
            }
        }
        Cooldown();
        if (press)
        {
            // if used 
            if (sTime > 0 && duration > 0)
            {
                // check if item hasnt gone over the duration 
                if (Time.time >= sTime + duration)
                {
                    resetAttachpoint();
                    // do something if item has lasted longer than the duration
                    Destroy(gameObject);
                }
            }

        }
        input += Time.deltaTime * rotSpeed;
    }
    void FixedUpdate()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            // rb.AddTorque(transform.right * rotSpeed * Time.deltaTime);
            rb.MoveRotation(initialRotation * Quaternion.AngleAxis(360 * input, transform.up));
            Vector3 movePos = new Vector3(attachPoint.position.x, attachPoint.position.y + yOffsetFromPlayer, attachPoint.position.z);
            rb.MovePosition(movePos);
        }

    }
}
