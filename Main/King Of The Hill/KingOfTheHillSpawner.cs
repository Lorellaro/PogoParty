using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KingOfTheHillSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] Leaderboard leaderboard;
    [SerializeField] ZoneScorerMover zoneScorerMover;
    [SerializeField] public GameObject playerPrefab;

    [SerializeField] public List<GameObject> playerList;

   //PhotonView view;

    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;


    private void Start()
    {
        Vector3 randomPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);

        leaderboard.gameObject.GetComponent<PhotonView>().RPC("addToLeaderboardList", RpcTarget.AllBuffered, "/N.A/", 0);

        leaderboard.gameObject.GetComponent<PhotonView>().RPC("searchForNewPlayerGameObjects", RpcTarget.AllBuffered);
    }
}
