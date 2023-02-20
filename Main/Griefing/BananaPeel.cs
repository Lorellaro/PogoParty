using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GriefingSystem;

namespace GriefingSystem
{
    public class BananaPeel : Powerup
    {
        [Header("Banana Specific")]
        [SerializeField] float throwForce;
        [SerializeField] float upwardThrowForce;
        [SerializeField] float downwardForceGain;
        [SerializeField] float selfKnockbackForce;
        [SerializeField] int projectionLinePoints = 50;
        [SerializeField] float timeBtwPoints = 0.1f;
        [SerializeField] LayerMask projectionCollidableLayers;
        [SerializeField] GameObject bananaThrowSFX;
        [SerializeField] GameObject crossDecal;

        InputManager _inputManager;
        bool follow = false;
        Rigidbody rb;
        Camera mainCam;
        LineRenderer lineRenderer;

        private void Awake()
        {
            if (_inputManager == null)
            {
                _inputManager = InputManager.Instance;
            }
            mainCam = Camera.main;
            rb = GetComponent<Rigidbody>();
            lineRenderer = GetComponent<LineRenderer>();
            StartCoroutine(FadeIntoExistence());
        }

        private void Start()
        {
            //Prevents null ref error
            if (!myPogoStickTransform) { return; }
            if (myPogoStickTransform.root.GetComponent<PhotonView>().IsMine)
            {
                _inputManager.OnEndInteract += releaseBanana;
                lineRenderer.enabled = true;
                crossDecal.SetActive(true);
                follow = true;

                GetComponent<PhotonView>().RPC("UpdatePlayerViewId", RpcTarget.All, myPlayerViewId);
                PlayPowerupUseSFX();
            }
        }

        private void OnDisable()
        {
            if (!myPogoStickTransform) { return; }
            if (myPogoStickTransform.root.GetComponent<PhotonView>().IsMine)
            {
                _inputManager.OnEndInteract -= releaseBanana;
            }
        }

        private void FixedUpdate()
        {
            if (follow)
            {
                FollowPlayer();
                //ProjectLine();
            }
            else if (!rb.isKinematic)
            {
                //rb.AddForce(Vector3.down * downwardForceGain * Time.deltaTime);
            }
        }

        private void Update()
        {
            if (follow)
            {
                ProjectLine();
            }
        }

        private void releaseBanana()
        {
            //Disable if already thrown
            if (!follow) { return; }
            follow = false;
            rb.isKinematic = false;
            PhotonNetwork.Instantiate(bananaThrowSFX.name, transform.position, Quaternion.identity);
            ThrowBanana();
            lineRenderer.enabled = false;
            crossDecal.SetActive(false);
        }

        private void FollowPlayer()
        {
            //rb.MovePosition(myPogoStickTransform.position + -myPogoStickTransform.right * 1.2f);//+ followPointOffset);
            //rb.MovePosition(myPogoStickTransform.Find("attachPoint").transform.position);
            rb.MovePosition(myPogoStickTransform.position + followPointOffset);
        }

        private void ThrowBanana()
        {
            //Throw facing player forward
            //Vector3 throwDirection = -myPogoStickTransform.right * throwForce + Vector3.up * upwardThrowForce;

            Vector3 throwDirection = mainCam.transform.forward * throwForce + Vector3.up * upwardThrowForce;

            rb.velocity = throwDirection * Time.fixedDeltaTime;
        }

        private void ProjectLine()
        {
            //Draw aim projection line
            lineRenderer.positionCount = (int)projectionLinePoints;
            Vector3 startPos = transform.position;
            Vector3 startVelocity = (mainCam.transform.forward * throwForce + Vector3.up * upwardThrowForce) * Time.fixedDeltaTime;

            List<Vector3> projectionPoints = new List<Vector3>();

            for (float t = 0; t < projectionLinePoints; t += timeBtwPoints)
            {
                Vector3 newPoint = startPos + t * startVelocity;
                newPoint.y = startPos.y + startVelocity.y * t + Physics.gravity.y / 2 * t * t;
                projectionPoints.Add(newPoint);

                if (Physics.OverlapSphere(newPoint, 0.2f, projectionCollidableLayers.value).Length > 0)
                {
                    lineRenderer.positionCount = projectionPoints.Count;
                    break;
                }
            }

            lineRenderer.SetPositions(projectionPoints.ToArray());
            crossDecal.transform.position = projectionPoints[projectionPoints.Count - 1];
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player")) { return; }
            //if (!collision.transform.root.GetComponent<PhotonView>().IsMine) { return; }

            //flip away then pop and dissapear
            Vector3 randomDir = Random.onUnitSphere;
            randomDir.y = Mathf.Abs(randomDir.y);

            rb.velocity = randomDir * selfKnockbackForce * Time.deltaTime;
            rb.MoveRotation(Quaternion.Euler(randomDir));

            GetComponent<PhotonView>().RPC("playerCollided", RpcTarget.All);
        }


        [PunRPC]
        private void playerCollided()
        {
            gameObject.layer = LayerMask.NameToLayer("Item");//Cannot collide with player anymore
            rb.constraints = RigidbodyConstraints.None;
            LeanTween.scale(gameObject, Vector3.zero, 1f);
            Destroy(transform.parent.gameObject, 1f);
        }

    }
}
