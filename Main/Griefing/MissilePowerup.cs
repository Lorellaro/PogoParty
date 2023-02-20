using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GriefingSystem;
using Photon.Pun;

namespace GriefingSystem
{
    public class MissilePowerup : Powerup
    {
        [Header("Missile Specific Variables")]
        [SerializeField] float missileFireSpeed;
        [SerializeField] float missileTurnSpeed;
        [SerializeField] float missileDeviationSpeed;
        [SerializeField] float missileDeviationAmount;
        [SerializeField] float beginHoningDistance;
        [SerializeField] Vector2 cameraShakeAmpDuration;
        [SerializeField] Vector2 movementRandomnessRange;
        [SerializeField] Vector3 targetOffset;
        [SerializeField] Rigidbody rb;
        [SerializeField] GameObject explosionVFXPrefab;
        [SerializeField] GameObject explosionSFXPrefab;

        Transform currentClosestTransform;
        Vector3 currentClosestPos;
        Vector3 missileDirection;
        bool follow = true;
        InputManager _inputManager;

        private void Awake()
        {
            if (_inputManager == null)
            {
                _inputManager = InputManager.Instance;
            }

            cameraShake = GetComponent<CameraShaker>();
        }

        private void Start()
        {
            //Prevents null ref error
            if (!myPogoStickTransform) { return; }
            if (myPogoStickTransform.root.GetComponent<PhotonView>().IsMine)
            {
                _inputManager.OnEndInteract += enableDisableFollow;
                PlayPowerupUseSFX();
            }
        }

        private void OnDisable()
        {
            if (!myPogoStickTransform) { return; }
            if (myPogoStickTransform.root.GetComponent<PhotonView>().IsMine)
            {
                _inputManager.OnEndInteract -= enableDisableFollow;
            }
        }

        private void FixedUpdate()
        {
            //Prevents null ref error
            if(!myPogoStickTransform) { return; }

            //Follow playerPos
            if (follow)
            {
                FollowPlayer();
            }

            //Fire at nearest enemy
            else
            {
                FlyToTarget();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //return if holding down fire button or if it hit the player that fired it
            if (follow) { return; }
            if (other.transform.root.GetComponent<PhotonView>()?.ViewID == myPlayerViewId) { return; }

            GetComponent<PhotonView>().RPC("Explode", RpcTarget.All);
            //Explode();
        }

        private void FollowPlayer()
        {
            Vector3 rotDir = (-myPogoStickTransform.right);// - transform.position).normalized;
            Quaternion rotGoal = Quaternion.LookRotation(rotDir);

            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotGoal, missileTurnSpeed));
            rb.MovePosition(myPogoStickTransform.position + followPointOffset);
        }

        private void FlyToTarget()
        {
            //Get nearest enemy
            Transform target = GetClosestEnemy(allPogoStickPhysTrans.ToArray());

            if (Vector3.Distance(target.position, transform.position) < beginHoningDistance)
            {
                missileDirection = (target.position - transform.position).normalized;
            }

            else
            {
                missileDirection = ((target.position + targetOffset) - transform.position).normalized;
            }

            Vector3 deviation = new Vector3(Mathf.Cos(Time.time * missileDeviationSpeed), 0, 0);

            Quaternion rotGoal =   Quaternion.LookRotation(missileDirection + (transform.TransformDirection(deviation) * missileDeviationAmount));
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotGoal, missileTurnSpeed));
            rb.MovePosition(transform.position + transform.forward * missileFireSpeed * Time.deltaTime);
        }

        private void enableDisableFollow()
        {
            follow = false;
        }

        [PunRPC]
        private void Explode()
        {
            Destroy(transform.parent.gameObject, 0.05f);
            GetComponent<Collider>().enabled = false;
            Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
            Instantiate(explosionSFXPrefab, transform.position, Quaternion.identity);
            cameraShake?.shakeCamera(cameraShakeAmpDuration.x, cameraShakeAmpDuration.y);
        }

        //Efficient way of getting closest object
        Transform GetClosestEnemy(Transform[] _playerTransforms)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (Transform potentialTarget in _playerTransforms)
            {
                if (potentialTarget.root.GetComponent<PhotonView>().ViewID == myPlayerViewId)
                {
                    continue;
                }

                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            return bestTarget;
        }
    }

}