using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GriefingSystem;
using Photon.Pun;

namespace GriefingSystem
{
    public class HammerPowerup : Powerup
    {
        [Header("Hammer Specific")]
        [SerializeField] float spinSpeed;
        [SerializeField] float knockBackForce;
        [SerializeField] float knockUpForce;
        [SerializeField] GameObject HitSFX;
        [SerializeField] GameObject HitVFX;
        [SerializeField] float duration;
        [SerializeField] Vector2 cameraShakeAmpDuration;

        Rigidbody rb;
        Quaternion initRot;
        PhotonView view;

        Coroutine delayVFXCoroutine;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(FadeIntoExistence());
            StartCoroutine(DeleteSelf());
            rb = GetComponent<Rigidbody>();
            initRot = rb.rotation;
            //Prevents null ref error
            if (!myPogoStickTransform) { return; }
            if (myPogoStickTransform.root.GetComponent<PhotonView>().IsMine)
            {
                PlayPowerupUseSFX();
            }
        }

        private void FixedUpdate()
        {
            view = GetComponent<PhotonView>();
            if (view.IsMine)
            {
                // rb.AddTorque(transform.right * rotSpeed * Time.deltaTime);
                rb.MovePosition(myPogoStickTransform.position + followPointOffset);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) { return; }
            if (other.transform.root.GetComponent<PhotonView>().ViewID == myPogoStickTransform.root.GetComponent<PhotonView>().ViewID) { return;  }

            Vector3 knockBackDirection = -1 * (transform.position - other.transform.position);
            other.transform.root.GetComponent<PhotonView>().RPC("oppositeKnockback", RpcTarget.All, knockBackDirection, knockBackForce, knockUpForce);
            PhotonNetwork.Instantiate(HitSFX.name, transform.position, Quaternion.identity);

            if (delayVFXCoroutine != null)
            {
                StopCoroutine(delayVFXCoroutine);
            }

            delayVFXCoroutine = StartCoroutine(PauseBeforeSpawnVFX(other));
            GetComponent<PhotonView>().RPC("ShakeCam", RpcTarget.All);
        }

        [PunRPC]
        private void ShakeCam()
        {
            cameraShake?.shakeCamera(cameraShakeAmpDuration.x, cameraShakeAmpDuration.y);
        }

        private IEnumerator PauseBeforeSpawnVFX(Collider _other)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(HitVFX, _other.ClosestPoint(transform.position), Quaternion.identity);
        }

        private IEnumerator DeleteSelf()
        {
            int scalingTime = 3;

            yield return new WaitForSeconds(duration - scalingTime);

            LeanTween.scale(gameObject, initScale * 0.5f, 0.3f).setEaseInOutSine().setLoopPingPong();

            yield return new WaitForSeconds(scalingTime - 1);
            LeanTween.scale(gameObject, Vector3.zero, 0.3f);
            Destroy(transform.parent.gameObject, 0.3f);

        }
    }
}