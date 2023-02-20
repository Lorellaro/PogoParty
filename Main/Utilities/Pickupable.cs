using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Pickupable : MonoBehaviour
{
    [SerializeField] PhotonView view;
    [SerializeField] Transform pickupCenter;
    [SerializeField] float pickupForce;
    [SerializeField] float dropForce;
    [SerializeField] float knockBackForce;
    [SerializeField] float knockUpForce;
    [SerializeField] float holdingMass = 0.000000001f;
    [SerializeField] float distAwayFromPlayer = 0.1f;
    [SerializeField] Vector3 holdOffset;
    [SerializeField] GameObject westButtonIcon;
    [SerializeField] ImageFade imageFadeScript;

    [HideInInspector] public bool explosive;
    [HideInInspector] public GameObject explosionVFX;

    Coroutine isFading;
    InputManager _inputManager;
    GameObject myPlayerPogostick;
    Rigidbody rb;
    Vector3 pogoDir;
    Vector3 newPogoDir;
    CharacterJoint connectedObjJoint;

    float defaultMass;
    float leewayRadius = 0.1f;

    bool Hardcore;
    bool isPlayerInRange;
    bool holdObject;
    bool buttonPress;
    bool exploded;

    int currentBarrelHolder;
    
    private void Start()
    {
        if (_inputManager == null)
        {
            _inputManager = InputManager.Instance;
        }

        _inputManager.OnStartInteract += pickupObj;

        rb = GetComponent<Rigidbody>();
        connectedObjJoint = GetComponent<CharacterJoint>();

        defaultMass = holdingMass;
        
    }

    //unsubscribe from event
    private void OnDisable()
    {
        _inputManager.OnStartInteract -= pickupObj;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (holdObject)
        {
            holdObj();
        }
    }

    //if in range pick up
    //if is held and pickup is called again, fire obj

    private void pickupObj()
    {
        //Play button press effects
        StartCoroutine(buttonPressedEffect());

        //If Holding shoot
        if (holdObject)
        {
            //shoot barrel
            //holdingMass = defaultMass;
            Vector3 direction = -myPlayerPogostick.transform.forward.normalized;
            rb.velocity = direction * dropForce * Time.deltaTime;
            Hardcore = true;//Sets knockback on
            holdObject = false;

            Destroy(connectedObjJoint);
            Destroy(gameObject, 10f);
        }

        if (!isPlayerInRange) { return; }
        //called on button press and when in range

        holdObject = true;

    }

    private IEnumerator buttonPressedEffect()
    {
        float sizeIncrease = 0.1f;
        float transitionTime = 0.075f;

        Vector3 newIconSize = new Vector3(westButtonIcon.transform.localScale.x + sizeIncrease, westButtonIcon.transform.localScale.y + sizeIncrease, westButtonIcon.transform.localScale.z + sizeIncrease);
        LeanTween.scale(westButtonIcon, newIconSize, transitionTime).setEaseInOutBounce();

        yield return new WaitForSecondsRealtime(transitionTime);

        newIconSize = new Vector3(westButtonIcon.transform.localScale.x - sizeIncrease, westButtonIcon.transform.localScale.y - sizeIncrease, westButtonIcon.transform.localScale.z - sizeIncrease);
        LeanTween.scale(westButtonIcon, newIconSize, transitionTime).setEaseInOutBounce();
    }

    private void holdObj()
    {

        rb.mass = holdingMass;

        //newPogoDir = -1 * (((pickupCenter.position - myPlayerPogostick.transform.position) - holdOffset));
        //newPogoDir = Vector3.MoveTowards(pickupCenter.position, myPlayerPogostick.transform.position, Time.deltaTime);
        //rb.velocity = newPogoDir * pickupForce * Time.deltaTime;//pogoDir * (pickupForce * Time.deltaTime);

        if (connectedObjJoint)
        {
            connectedObjJoint.connectedBody = myPlayerPogostick.GetComponent<Rigidbody>();
        }

        //rb.drag = 2f;
        //rb.angularDrag = 2f;

        /*
        //if dist from last position is smaller than 0.03
        if (Vector3.Distance(newPogoDir, pogoDir) < distAwayFromPlayer)
        {
            rb.isKinematic = true;
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.isKinematic = false;
            pogoDir = newPogoDir;
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("PlayerRoot")) { return; }
        if (other.gameObject.transform.root.GetComponent<PhotonView>().IsMine)
        {
            //null check for fade
            if (isFading != null)
            {
                StopCoroutine(isFading);
            }

            imageFadeScript.fadeIn();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        if (other.gameObject.transform.root.GetComponent<PhotonView>().IsMine)
        {
            myPlayerPogostick = other.gameObject.transform.root.GetChild(2).GetChild(0).gameObject;
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.root.CompareTag("PlayerRoot")) { return; }
        if (other.gameObject.transform.root.GetComponent<PhotonView>().IsMine)
        {
            myPlayerPogostick = null;
            isPlayerInRange = false;

            //fade button icon
            if (isFading != null)
            {
                StopCoroutine(isFading);
            }

            imageFadeScript.fadeOut();

            //if (isFading == null)
            //{

            //isFading = StartCoroutine(imageFadeScript.FadeImage(false));// Fade out
            //}
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( Hardcore && !collision.gameObject.transform.root.CompareTag("PlayerRoot") && !exploded)//if( LayerMask.LayerToName(collision.gameObject.layer) == "Player" &&
        {
            //play explosion VFX & SFX
            PhotonNetwork.Instantiate(explosionVFX.name, transform.position, Quaternion.identity);
            exploded = true;
            Destroy(gameObject, 0.05f);
            //Instantiate(explosionVFX, transform.position, Quaternion.identity);

            /*
            GameObject playerObj = collision.transform.root.gameObject.transform.GetChild(2).GetChild(0).gameObject;
            Rigidbody playerRB = playerObj.GetComponent<Rigidbody>();

            Vector3 _knockBackDirection = -1 * (collision.GetContact(0).point - playerObj.transform.position);
            playerRB.velocity = _knockBackDirection.normalized * knockBackForce + Vector3.up * knockUpForce * Time.deltaTime;
            */
            Hardcore = false;
        }
    }

    private IEnumerator turnOffHardHitter()
    {
        yield return new WaitForSeconds(0.75f);
        Hardcore = false;
    }
}





//shows the vfx and sfx variables if explosive
#if UNITY_EDITOR
[CustomEditor(typeof(Pickupable))]
public class Pickupable_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        Pickupable script = (Pickupable)target;

        // draw checkbox for the bool
        script.explosive = EditorGUILayout.Toggle("Explosive", script.explosive);

        if (script.explosive) // if bool is true, show other fields
        {
            script.explosionVFX = EditorGUILayout.ObjectField("explosionVFX", script.explosionVFX, typeof(GameObject), true) as GameObject;
        }
    }
}
#endif