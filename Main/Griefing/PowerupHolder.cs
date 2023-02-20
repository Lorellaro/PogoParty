using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GriefingSystem;
using Photon.Pun;

namespace GriefingSystem
{
    public class PowerupHolder : MonoBehaviour
    {
        [SerializeField] Leaderboard leaderboard;
        [SerializeField] GameObject powerupHeldPrefab;

        InputManager _inputManager;

        [SerializeField] List<GameObject> allPlayers;
        [SerializeField] List<Transform> allPogoStickPhysTrans;
        [SerializeField] PhotonView playerView;

        PowerupUIController powerupUI;
        int myPlayerViewId;

        private void Awake()
        {
            // get instance of input manager
            if (_inputManager == null)
            {
                _inputManager = InputManager.Instance;
            }

            leaderboard = GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<Leaderboard>();
            allPlayers = leaderboard.GetAllPlayers();
            allPogoStickPhysTrans = leaderboard.pogostickPhysTransform;

            myPlayerViewId = playerView.ViewID;
            powerupUI = GameObject.FindGameObjectWithTag("PowerupUI").GetComponent<PowerupUIController>();
        }

        private void OnEnable()
        {
            if (playerView.IsMine)
            {
                _inputManager.OnStartInteract += instantiateAndReleaseObj;
            }
        }

        private void OnDisable()
        {
            if (playerView.IsMine)
            {
                _inputManager.OnStartInteract -= instantiateAndReleaseObj;
            }
        }

        public bool powerupIsHeld()
        {
            if(powerupHeldPrefab == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public GameObject GetPowerupHeld()
        {
            return powerupHeldPrefab;
        }

        public void SetPowerupHeld(GameObject _powerupHeldPrefab)
        {
            powerupHeldPrefab = _powerupHeldPrefab;
        }

        public void SetPowerupHeldUI(Sprite _powerupHeldSprite)
        {
            if (powerupHeldPrefab == null) { return; }
            powerupUI.SetItemImage(_powerupHeldSprite);
        }

        public void instantiateAndReleaseObj()
        {
            if(powerupHeldPrefab == null) { return; }
            GameObject newPowerup = PhotonNetwork.Instantiate(powerupHeldPrefab.name, transform.position, Quaternion.identity);

            Powerup newPowerupScript = newPowerup.GetComponentInChildren<Powerup>();

            //Set vars values to new powerup
            newPowerupScript.SetPogostickTransform(transform);
            newPowerupScript.SetAllPlayers(allPlayers);
            newPowerupScript.SetAllPogoStickTrans(allPogoStickPhysTrans);
            newPowerupScript.SetPlayerViewId(myPlayerViewId);

            powerupHeldPrefab = null;
            powerupUI.playUIOutVFX();
        }
    }
}