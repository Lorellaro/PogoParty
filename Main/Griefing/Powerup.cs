using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace GriefingSystem
{
    public abstract class Powerup : MonoBehaviour
    {
        [Header("Generic Powerup Variables")]
        [SerializeField] protected Transform myPogoStickTransform;
        [SerializeField] protected Vector3 followPointOffset;
        [SerializeField] GameObject poofVFX;
        [SerializeField] GameObject powerupUsedSFX;

        protected CameraShaker cameraShake;
        protected List<GameObject> allPlayers;
        protected List<Transform> allPogoStickPhysTrans;
        protected PhotonView powerupView;
        protected float timealive;
        protected int myPlayerViewId;
        protected Vector3 initScale;

        //--Setter functions -- set by powerup holder on instantiation

        public void SetPogostickTransform(Transform _myPogoStickTransform)
        {
            myPogoStickTransform = _myPogoStickTransform;
        }

        public void SetAllPlayers(List<GameObject> _allPlayers)
        {
            allPlayers = _allPlayers;
        }

        public void SetAllPogoStickTrans(List<Transform> _allPogoStickPhysTrans)
        {
            allPogoStickPhysTrans = _allPogoStickPhysTrans;
        }

        public void SetPlayerViewId(int _myPlayerViewId)
        {
            myPlayerViewId = _myPlayerViewId;
        }

        [PunRPC]
        protected void UpdatePlayerViewId(int _myPlayerViewId)
        {
            myPlayerViewId = _myPlayerViewId;
        }

        protected void PlayPowerupUseSFX()
        {
            PhotonNetwork.Instantiate(powerupUsedSFX.name, transform.position, Quaternion.identity);
        }

        protected IEnumerator FadeIntoExistence()
        {
            initScale = transform.localScale;
            Collider mycol = GetComponent<Collider>();
            mycol.enabled = false;

            if (poofVFX)
            {
                Instantiate(poofVFX, transform.position + followPointOffset, Quaternion.identity);
            }

            LeanTween.scale(gameObject, Vector3.zero, 0f);
            yield return new WaitForSeconds(0.001f);
            LeanTween.scale(gameObject, initScale, 0.1f);
            yield return new WaitForSeconds(0.25f);
            mycol.enabled = true;
        }
    }

}
