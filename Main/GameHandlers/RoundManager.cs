using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Main.GameHandlers
{
    public class RoundManager : MonoBehaviour
    {
        public static RoundManager Instance;
        protected Dictionary<int, PlayerRoundData> players;
        protected PhotonView _photonView;
        [SerializeField] protected Transform[] startingPositions;
        protected bool _ready;
        [SerializeField] protected bool autoStart;

        protected virtual void Awake()
        {
            Instance = this;
            _photonView = GetComponent<PhotonView>();
            players = new Dictionary<int, PlayerRoundData>();
        }
        
        protected virtual void Start()
        {
            if (autoStart)
            {
                StartCoroutine(AutoStartRace());
            }
        }

        protected virtual IEnumerator AutoStartRace()
        {
            yield return new WaitForSeconds(3);
            GetAllPlayers();
        }
        
        
        [PunRPC]
        protected void GetAllPlayers()
        {
            if (_ready)
            {
                Debug.Log(GetType().Name + " is already ready, players list has been initialized");
                return;
            }
            GameObject[] allPlayerObjects = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < allPlayerObjects.Length / 16; ++i)
            {
                GameObject player = allPlayerObjects[i * 16].transform.root.gameObject;
                int viewID = player.GetComponent<PhotonView>().ViewID;
                PlayerRoundData playerRoundData =
                    new PlayerRoundData(player, 0, 0);
                players.Add(viewID, playerRoundData);
            }
            
            players = players.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            int position = 1;
            foreach (var playerData in players.Values)
            {
                playerData.RoundPosition = position;
                PogoStickPhysics player = playerData.PlayerObject.transform.GetChild(2).GetChild(0).GetComponent<PogoStickPhysics>();
                player.ResetPosition(startingPositions[position - 1].position);
                player.SetRigidBodyVelocity(Vector3.zero);
                player.transform.rotation = Quaternion.Euler(-90,0,0);
                position++;
            }

            _ready = true;
            RoundManagerReady();
            Debug.Log(GetType().Name + " ready");
            
        }
        
        
        public void CallGetAllPlayers()
        {
            _photonView.RPC("GetAllPlayers", RpcTarget.All);
        }
        
        [PunRPC]
        protected void UpdateRoundValue(int playerID, int roundValue)
        {
            if (!_ready)
            {
                Debug.Log(GetType().Name + " not ready, players list needs to be initialized");
                return;
            }
            players[playerID].RoundValue = roundValue;
            CallUpdatePlayerRoundData();
            
        }
        
        public void CallUpdateRoundValue(int playerID, int roundValue)
        {
            _photonView.RPC("UpdateRoundValue", RpcTarget.All, playerID, roundValue);
        }

        public void CallUpdateRoundPosition(int playerID, int newPosition)
        {
            _photonView.RPC("UpdateRoundPosition", RpcTarget.All, playerID, newPosition);
        }
        
        [PunRPC]
        protected void UpdateRoundPosition(int playerID, int newPosition)
        {
            if (!_ready)
            {
                Debug.Log(GetType().Name + " not ready, players list needs to be initialized");
                return;
            }
            players[playerID].RoundPosition = newPosition;
            CallUpdatePlayerRoundData();
        }
        
        public int GetRoundValue(int playerID)
        {
            if (!_ready)
            {
                Debug.Log(GetType().Name + " not ready, players list needs to be initialized");
                return -1;
            }
            return players[playerID].RoundValue;
        }

        public int GetRoundPosition(int playerID)
        {
            if (!_ready)
            {
                Debug.Log(GetType().Name + " not ready, players list needs to be initialized");
                return -1;
            }
            return players[playerID].RoundPosition;
        }

        [PunRPC]
        protected virtual void UpdatePlayerRoundData()
        {
            if (!_ready)
            {
                Debug.Log(GetType().Name + " not ready, players list needs to be initialized");
                return;
            }
            
            players = players.OrderByDescending(x => x.Value.RoundValue).ToDictionary(x => x.Key, x => x.Value);
            int position = 1;
            foreach (var player in players.Values)
            {
                player.RoundPosition = position;
                position++;
            }
            RoundUpdateUI();
        }
        
        public void CallUpdatePlayerRoundData()
        {
            _photonView.RPC("UpdatePlayerRoundData", RpcTarget.All);
        }
        
        public void DisplayPlayers()
        {
            if (!_ready)
            {
                Debug.Log(GetType().Name + " not ready, players list needs to be initialized");
                return;
            }
            foreach (var player in players)
            {
                Debug.Log("Player ID: " + player.Key + " Player Waypoint: " + player.Value.RoundValue + " Player Position: " + player.Value.RoundPosition);
            }
        }
        

        public event Action onRoundManagerReady;

        protected void RoundManagerReady()
        {
            onRoundManagerReady?.Invoke();
        }

        public event Action onRoundUpdateUI;

        protected void RoundUpdateUI()
        {
            onRoundUpdateUI?.Invoke();
        }
    }
    
    public class PlayerRoundData
    {
        public GameObject PlayerObject;
        public int RoundValue;
        public int RoundPosition;

        public PlayerRoundData(GameObject playerObject, int roundValue, int roundPosition)
        {
            PlayerObject = playerObject;
            RoundValue = roundValue;
            RoundPosition = roundPosition;
        }
    }
}