using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GriefingSystem;

namespace GriefingSystem
{
    public class PowerupGiver : MonoBehaviour
    {
        [SerializeField] List<GameObject> powerups;
        [Header("Must be in the same order as powerups above")]
        [SerializeField] List<Sprite> powerupIcons; 
        [SerializeField] GameObject pickupVFX;
        [SerializeField] GameObject pickupSFX;
        [SerializeField] bool doesRespawn;
        [SerializeField] float respawnWaitTime;

        GameObject powerupUI;
        Collider myCol;
        Vector3 initScale;

        private void Awake()
        {
            powerupUI = GameObject.FindGameObjectWithTag("PowerupUI");
            myCol = GetComponent<Collider>();
            initScale = transform.localScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Transform playerPogostick = other.transform.root.GetChild(2).GetChild(0);

                PowerupHolder playerPowerupHolder = playerPogostick.GetComponent<PowerupHolder>();

                if (playerPowerupHolder.powerupIsHeld())
                {
                    playBoxVFX();
                    return;
                }

                // get random item in list--------------------------------------------------------------------------------------------------------------------------------MAYBE MAKE THIS -1, POWERUPS.COUNT
                int randomNum = Random.Range(0, powerups.Count);
                playerPowerupHolder.SetPowerupHeld(powerups[randomNum]);
                playerPowerupHolder.SetPowerupHeldUI(powerupIcons[randomNum]);

                //VFX + shrink object
                playBoxVFX();

                myCol.enabled = false;

                //check if player collided with is mine
                if (other.transform.root.GetComponent<PhotonView>().IsMine && powerupUI)
                {
                    powerupUI.GetComponent<PowerupUIController>().playUIVFX();
                }
                
            }
        }

        private void playBoxVFX()
        {
            //SFX
            Instantiate(pickupSFX, transform.position, Quaternion.identity);

            //VFX
            Instantiate(pickupVFX, transform.position, Quaternion.identity);
            LeanTween.scale(gameObject, Vector3.zero, 0.1f).setEaseInOutBounce();

            if (doesRespawn)
            {
                StartCoroutine(RespawnSelf());
            }
        }

        private IEnumerator RespawnSelf()
        {
            yield return new WaitForSeconds(respawnWaitTime);
            myCol.enabled = true;
            LeanTween.scale(gameObject, initScale, 0.1f).setEaseInOutBounce();

        }
    }
}