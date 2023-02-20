using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Item : MonoBehaviour
{
    public PhotonView view;
    public PhotonView playerView;
    private GameObject player;
    public GameObject pogo;
    public Transform attachPoint;
    public string gameMode;
    // how long item will last
    public float duration = 10f;
    public Leaderboard leaderboard;
    public bool used = false;
    public bool canHold = false;
    public bool finished = false;
    public bool hideOnCollision = true;
    private bool pressed = false;
    private bool run = false;
    private InputManager _inputManager;
    private float startTime = -1;
    public float cooldown = -1;
    public float cooldownMax = 2;
    public List<GameObject> allPlayers;
    public float knockBackForce;
    public float upwardsKnockBackForce;
    //protected float timeAliveFromRelease;

    private void Awake()
    {
        // get instance of input manager
        if (_inputManager == null)
        {
            _inputManager = InputManager.Instance;
        }
    }
    public virtual void Start()
    {
        init();
        // start "inactive"
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        // determine logic
        gameMode = GameController.gameMode;
        leaderboard = GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<Leaderboard>();
        allPlayers = leaderboard.GetAllPlayers();
    }

    public virtual void Update()
    {
        //view = transform.parent.GetComponent<PhotonView>();
        //if (!view) { return; }
        if (view.IsMine)
        {
            print("View is mine");
            // if the object can be held
            if (canHold)
            {
                // interact held for at least one frame
                if (_inputManager.interactHeld)
                {
                    // reset any rotation on attach point
                    if (transform.parent)
                    {
                        transform.parent.localRotation = Quaternion.identity;
                    }
                    // interact was pressed
                    pressed = true;
                    // show on back 
                    view.RPC("ShowItem", RpcTarget.All);

                    //ShowItem();
                }
            }
            // check if the interact button has been released or when it is pressed if the item cannot be held
            if (((!_inputManager.interactHeld && pressed) || (!canHold && _inputManager.interactHeld)) && !used)
            {
                // released object
                transform.parent = null;
                used = true;
                startTime = Time.time;

            }
            // if used and there is a duration time
            if (startTime > 0 && duration > 0)
            {
                // check if item hasnt gone over the duration 
                if (Time.time >= startTime + duration)
                {
                    // allow logic after death
                    finished = true;
                }
            }

            // as soon as item is used reset attachpoint
            if (!canHold && used && !run)
            {
                run = true;
                view.RPC("resetAttachpoint", RpcTarget.All);

                //resetAttachpoint();
            }
            // when item is finished remove attach point
            if (finished && !run)
            {
                run = true;
                view.RPC("resetAttachpoint", RpcTarget.All);

                //resetAttachpoint();
            }
        }
        print("Finished item update");
    }

    [PunRPC]
    public void ShowItem()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // anything that isnt the player with item is hit 
        if (other.gameObject.tag == "Player" && other.gameObject.transform.root != player.transform)
        {
            print("in here");
            if (cooldown < 0)
            {
                knockbackPlayer(other);
                cooldown = Time.time;
                if (hideOnCollision)
                {
                    GetComponent<Renderer>().enabled = false;
                    GetComponent<Collider>().enabled = false;
                }
            }
        }
       

    }
    public void setAttachPoint(Transform ap)
    {
        attachPoint = ap;
    }

    [PunRPC]
    public void resetAttachpoint(){
        if (attachPoint.childCount > 0)
        {
            Destroy(attachPoint.GetChild(0).gameObject);
        }
    }
    
    [PunRPC]
    public void ReleaseItem()
    {
        //timeAliveFromRelease = 0;

    }

    public void init(){
        // make item move with attach point on player
        transform.parent.parent = attachPoint;
        // get player root
        player = attachPoint.gameObject.transform.root.gameObject;
        playerView = player.GetComponent<PhotonView>();
        // pogo stick
        pogo = player.transform.GetChild(2).GetChild(0).gameObject;
        // get view of item
        view = GetComponent<PhotonView>();
    }
    public void Cooldown(){
        if (cooldown > 0)
        {
            if (Time.time >= cooldownMax + cooldown)
            {
                cooldown = -1;
            }
        }
    }
    public void knockbackPlayer(Collider other){
        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        GameObject playerObj = other.transform.root.gameObject.transform.GetChild(2).GetChild(0).gameObject;
        Rigidbody playerRB = playerObj.GetComponent<Rigidbody>();
        Vector3 _knockBackDirection = -1 * (transform.position - collisionPoint);
        // playerRB.velocity = _knockBackDirection.normalized * knockBackForce + Vector3.up * upwardsKnockBackForce * Time.deltaTime;
        playerObj.transform.root.gameObject.GetComponent<PhotonView>().RPC("oppositeKnockback", RpcTarget.All, _knockBackDirection, knockBackForce, upwardsKnockBackForce);
    }
}
