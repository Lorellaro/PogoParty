using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Random = UnityEngine.Random;



public class PogoStickPhysics : MonoBehaviour//I think its better to name this pogoStickController 
{
    #region [Variables]
    //Declaration & Initialisation
    [SerializeField] float bounceForce = 10000f;
    [SerializeField] float minBounceForce = 0.5f;
    [SerializeField] float bounceFuelCost = 5f;
    [SerializeField] float chargeTime = 1f;
    [Tooltip("Maximum amount of bonus force gained from holding the jump down for longer")]
    [SerializeField] float maxJumpHoldBonus = 2f;
    [SerializeField] float knockBackForce = 100000f;

    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] float uprightRotationSpeed = 0.1f;
    [SerializeField] float groundTouchLeeway = 0.2f;
    [SerializeField] float leanForce = 100f;


    [SerializeField] GameObject vcam;
    [SerializeField] Transform botStick;

    [Header("AudioSettings")]
    [SerializeField] GameObject jumpSfxPrefab;

    [SerializeField] CameraShakeController cameraShakeScript;
    [SerializeField] LayerMask jumpOffableLayers;

    [Header("Llwelyn Rigidbodies")]
    [SerializeField] List<Rigidbody> allRBList;

    [SerializeField] Material cannisterMaterialY;
    [SerializeField] Material cannisterMaterialX;

    public TextMeshPro totalText;
    public int total;
    Rigidbody rb;

    public bool isDisabled = false;
    bool bounce = false;
    bool isTouchingGround = false;
    bool tiltLeft = false;
    bool tiltRight = false;
    bool tiltBack = false;
    bool tiltForward = false;
    bool fuelTaken = false;
    bool firstBounceFrame = true;
    bool bounceCooldownOn = false;
    private Vector3 initialRotation;

    private Coroutine resetIsTouchingGround;

    public float jumpHoldCounter = 0f; //holds the value for how long the pogostick's jump
    private InputManager _inputManager;
    //[System.NonSerialized] public bool menuActive;
    public bool tutorialCompleted = false;
    public float jumpHoldTimeMultiplier = 0;
    private AudioSource pogoStickEngine;

    private PogoExplosionVFX pogoExplosionVFX;
    private GiveRandomAudioClip giveRandomAudioClip;

    public float amplitudeMult = 0.05f;

    private float platformY;
    /*
    private bool onPlatform;
    private bool extraBoost;
    private bool checkFalloff;
    */
    
    private bool canPlay = true;
    public bool inShopRange = false;

    [SerializeField]private float jumpLeniencyDuration = 0.5f;
    [SerializeField]private float jumpLeniencyBoostAmount = 10;
    [SerializeField]private ParticleSystem pSystem;

    private Rigidbody[] _rigidbodiesRagdoll;
    [SerializeField] private GameObject llwelynModel;
    [SerializeField] private bool isRagdoll = false;

    public GameObject pogoParent;
    [SerializeField] private float groundCheckDistance;

     
    public PhotonView view;

    # endregion

    void Awake()//called before start
    {
        view = transform.parent.parent.GetComponent<PhotonView>();

        if (view.IsMine)
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            if (pogoStickEngine == null)
            {
                pogoStickEngine = GetComponent<AudioSource>();
            }
            if (pogoExplosionVFX == null)
            {
                pogoExplosionVFX = GetComponent<PogoExplosionVFX>();
            }
            vcam = GameObject.Find("Main Camera");
            llwelynModel = GameObject.Find("Llwelyn_Ragdoll");
            pSystem = GetComponentInChildren<ParticleSystem>();
            if (_inputManager == null)
            {
                _inputManager = InputManager.Instance;
            }
            // DontDestroyOnLoad(_inputManager);
            cameraShakeScript = GameObject.FindGameObjectWithTag("Game Controller").GetComponent<CameraShakeController>();
        }
        giveRandomAudioClip = GetComponent<GiveRandomAudioClip>();
    }

    void Start(){

        //Prevents null ref error at start
        if (resetIsTouchingGround != null)
        {
            StopCoroutine(resetIsTouchingGround);
        }

        if (view.IsMine)
        {
            pogoParent = transform.parent.gameObject;
            // Debug.Log(pogoParent.name);
            // check if player has saved the game at all
            PlayerData data = SaveSystem.LoadPlayer();
        }


        //_rigidbodiesRagdoll = llwelynModel.gameObject.GetComponentsInChildren<Rigidbody>();
        //ToggleRagdoll(true);
    }

    public void IgnoreCollisions(bool ignore)
    {
        Physics.IgnoreLayerCollision(9, 10, ignore);
        Physics.IgnoreLayerCollision(10, 11, ignore);
    }

    // Update is called once per frame
    private void Update()
    {
        if(view.IsMine)
        {
            pogoStickEngine.pitch = Utilities.Map(jumpHoldCounter, 0f, 1.6f, 1f, 3f);
            cannisterMaterialY.SetFloat("_FillAmount", Utilities.Map(jumpHoldCounter, 0f, 1.6f, 0.04f, 0.124f));
            cannisterMaterialX.SetFloat("_FillAmount", Utilities.Map(jumpHoldCounter, 0f, 1.6f, 0.05f, 0.082f));

            // allow inputs if shop is inactive
           // if (!menuActive)
            //{
            if (isDisabled) { return; }

            TiltInput();//INPUT

            if (!bounce && jumpHoldCounter > 0 && !_inputManager.jumpHeld)
            {
                jumpHoldCounter -= Time.deltaTime * 6;
                pogoStickEngine.Stop();
            }

            if (_inputManager.jumpHeld)
            {
                //handle windup SFX
                if (!pogoStickEngine.isPlaying)
                {
                    giveRandomAudioClip.changeAndPlayClip();
                }

                if (jumpHoldCounter < maxJumpHoldBonus)
                {
                    jumpHoldCounter += Time.deltaTime * chargeTime;
                    _inputManager.SetControllerRumble(Utilities.Map(jumpHoldCounter, 0, maxJumpHoldBonus, 0f, 0.5f));//0.7 is max rumble strength                    
                }
                else
                {
                    _inputManager.SetControllerRumble(0.5f);
                }
                // shake while jump held
                cameraShakeScript.HoldShake(jumpHoldCounter * amplitudeMult);
            }

        }

        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!view.IsMine) return;
        // landing particles
        if ((jumpOffableLayers.value & (1 << collision.transform.gameObject.layer)) > 0 && !_inputManager.jumpHeld && canPlay){
            StartCoroutine(playParticles());
            pSystem.Play();
        }
        /*
        if (collision.collider.CompareTag(Tags.Platform))
        { 
            platformY = transform.position.y;
            onPlatform = true;
        }
        */
        //if ((jumpOffableLayers.value & (1 << collision.transform.gameObject.layer)) > 0 || !collisionPhotonView.IsMine)//check if touching ground or another player
        if ((jumpOffableLayers.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            if(resetIsTouchingGround != null)
            {
                StopCoroutine(resetIsTouchingGround);
            }

            isTouchingGround = true;
            initialRotation = Vector3.zero;
        }
    }

    //if on object with ground layer then set isTouchingGround true
    private void OnCollisionStay(Collision collision)
    {
        PhotonView collisionPhotonView = collision.gameObject.transform.root.GetComponent<PhotonView>();

        if ((jumpOffableLayers.value & (1 << collision.transform.gameObject.layer)) > 0 || !collisionPhotonView.IsMine)//check if touching ground or another player
        {
            if(resetIsTouchingGround != null)
            {
                StopCoroutine(resetIsTouchingGround);
            }

            isTouchingGround = true;
            initialRotation = Vector3.zero;
        }

        Rigidbody collisionRB = collision.gameObject.GetComponent<Rigidbody>();

        //if the obj didn't collide with the player and has an RB
        if (collisionRB)
        {
            if(!collision.gameObject.CompareTag("Player") || !collisionPhotonView.IsMine)
            {
                //pushOtherObjDown(collisionRB);
            }
        }
    }

    //disables isTouchingGround when the player leaves the floor
    private void OnCollisionExit(Collision collision)
    {
        if (!view.IsMine) return;
        if ((jumpOffableLayers.value & (1 << collision.transform.gameObject.layer)) > 0 && !_inputManager.jumpHeld && canPlay){
            StartCoroutine(playParticles());
            pSystem.Play();
        }
        
        if ((jumpOffableLayers.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            //StopCoroutine(disableIsTouchingGround());
            resetIsTouchingGround = StartCoroutine(disableIsTouchingGround());
        }
        if (collision.collider.CompareTag(Tags.Platform))
        {
            // Debug.Log("Player fell off platform");
        }

        //resetIsTouchingGround = StartCoroutine(disableIsTouchingGround());
    }

    //Used for physics calculations, helps reduce bugs
    private void FixedUpdate()
    {
        if (view.IsMine)
        {

            if (isDisabled) { return; }

            //bounce is set true in the pogo stick animation
            if (bounce)
            {

                if (transform.parent != null)
                {
                    transform.parent = pogoParent.transform;
                }

                //Jump
                rb = GetComponent<Rigidbody>();
                rb.velocity += bounceForce * botStick.forward * Time.deltaTime * (jumpHoldCounter + minBounceForce);//Jump

                //is only called once per bounce
                if (firstBounceFrame)
                {
                    firstBounceFrame = false;
                    PhotonNetwork.Instantiate(jumpSfxPrefab.name, transform.position, Quaternion.identity);
                    pogoStickEngine.Stop();
                    pogoExplosionVFX.explode();
                    StartCoroutine(cameraShakeScript.BurstShake(1.2f, 0.25f));
                }

            }
            else if (!bounce && isTouchingGround)
            {
                _inputManager.ResetControllerRumble();
                //cameraShakeScript.HoldShake(0);
            }

            Tilt();
        }
    }

    //Waits a split second after the player is no longer touching the floor before disabling 
    private IEnumerator disableIsTouchingGround()
    {
        yield return new WaitForSeconds(groundTouchLeeway);
        isTouchingGround = false;
    }

    private void TiltInput()// takes INPUT from regular update
    {
        if (_inputManager.movementInput.x < 0f)
        {
            tiltLeft = true;
            tiltRight = false;
        }
        else
        {
            tiltLeft = false;
        }

        if (_inputManager.movementInput.x > 0f)
        {
            tiltRight = true;
            tiltLeft = false;
        }
        else
        {
            tiltRight = false;
        }

        if (_inputManager.movementInput.y > 0f)
        {
            tiltForward = true;
            tiltBack = false;
        }
        else
        {
            tiltForward = false;
        }

        if (_inputManager.movementInput.y < 0f)
        {
            tiltBack = true;
            tiltForward = false;
        }
        else
        {
            tiltBack = false;
        }
    }

    private void Tilt()//Performs rotation in fixed update
    {
        rb = GetComponent<Rigidbody>();


        if (tiltForward)
        {
            rb.AddTorque(vcam.transform.right * rotateSpeed * Time.deltaTime);
        }

        else if (tiltBack)
        {
            rb.AddTorque(-vcam.transform.right * rotateSpeed * Time.deltaTime);
        }

        if (tiltRight)
        {
            rb.AddTorque(-vcam.transform.forward * rotateSpeed * Time.deltaTime);
        }

        else if (tiltLeft)
        {
            rb.AddTorque(vcam.transform.forward * rotateSpeed * Time.deltaTime);
        }

        float v = _inputManager.movementInput.y * rotateSpeed * Time.deltaTime;
        float h = _inputManager.movementInput.x * rotateSpeed * Time.deltaTime;

        rb.AddTorque(Vector3.forward * h, ForceMode.VelocityChange);
        rb.AddTorque(Vector3.up * v, ForceMode.VelocityChange);

        //Keeps pogostick upright

        // Quaternion rotationUpright = Quaternion.LookRotation(rb.transform.forward, Vector3.up);
        // GetComponent<Rigidbody>().MoveRotation(rotationUpright);

        //If the player is not pressing any moving keys then apply force to make it stand upright 
        // if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        // {
        //     //var rot = Quaternion.FromToRotation(transform.forward, Vector3.up);
        //     //rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightRotationSpeed);
        // }

        if (isTouchingGround)//if is colliding with layer ground then keep the stick upright
        {
            var rot2 = Quaternion.FromToRotation(transform.forward, Vector3.up);//get the rotation towards upwards
            rb.AddTorque(new Vector3(rot2.x, rot2.y, rot2.z) * uprightRotationSpeed / 2);//rotate upwards 
        }

        if (!isTouchingGround)//if not colliding with ground layer then lean in wasd direction full 360 degrees
        {
            //lean
            float forceHorizontal = _inputManager.movementInput.x * leanForce * Time.deltaTime;//get players a and d keys, then apply specified force amount to it
            float forceVertical = _inputManager.movementInput.y * leanForce * Time.deltaTime;//w and s keys (can be changed in input manager)
            //leans dependant on camera's forward direction
            rb.AddForce(vcam.transform.right * forceHorizontal, ForceMode.VelocityChange);
            rb.AddForce(vcam.transform.forward * forceVertical, ForceMode.VelocityChange);
        }
    }

    private IEnumerator DisableJump()//controls jump upforce duration
    {
        yield return new WaitForSeconds(0.15f);//jump upforce duration
        bounce = false;
        yield return new WaitForSeconds(groundTouchLeeway + 0.1f);
        bounceCooldownOn = false;
    }

    public void RegularBounce()//called from push out animation
    {
        //if already bouncing return
        if (bounceCooldownOn) { return; }

        if (isTouchingGround)
        {
            //first bounce frame is called in fixed update so that the bounce vfx and sfx only plays once
            firstBounceFrame = true;
            bounce = true;

            //Waits x  time then allows for jumping again
            bounceCooldownOn = true;
            //Disable jump from when it starts jumping
            StartCoroutine(DisableJump());
        }
    }

    public float getBounceForce()
    {
        return bounceForce;
    }

    public void setBounceForce(float val)
    {
        bounceForce = val;
    }

    public float getChargeTime()
    {
        return chargeTime;
    }

    public void setChargeTime(float val)
    {
        chargeTime = val;
    }

    private IEnumerator playParticles(){
        canPlay = false;
        yield return new WaitForSeconds(0.4f);
        canPlay = true;
    }

    public void SetRigidBodyVelocity(Vector3 newVelocity)
    {
        rb = GetComponent<Rigidbody>();
        //pogostick rb
        rb.velocity = newVelocity;

        //ragdoll rb
        for(int i = 0; i < allRBList.Capacity; i++)
        {
            allRBList[i].velocity = newVelocity;
        }
    }

    //Called in checkpoint
    public void ResetPosition(Vector3 newPosition)
    {
        rb = GetComponent<Rigidbody>();
        rb.position = newPosition;

        for (int i = 0; i < allRBList.Capacity; i++)
        {
            allRBList[i].position = newPosition;
        }
    }

    private void pushOtherObjDown(Rigidbody otherRB)
    {
        if (bounce)
        {
            otherRB.velocity += -botStick.forward * knockBackForce * Time.deltaTime;
        }
    }

    private void ToggleRagdoll(bool isAnimating)
    {
        //needs reworking for new ragdoll system
        /*
        isRagdoll = !isAnimating;
        foreach (var ragdollBone in _rigidbodiesRagdoll)
        {
            ragdollBone.isKinematic = isAnimating;
            ragdollBone.detectCollisions = !isAnimating;
        }

        if (isRagdoll)
        {
            llwelynModel.GetComponent<Animator>().enabled = false;
            llwelynModel.transform.parent = null;
            _rigidbodiesRagdoll[0].velocity = Vector3.forward * 50;
        }
        */
    }

    public PhotonView GetPhotonViewOnPlayer(){
        return view;
    }
}
