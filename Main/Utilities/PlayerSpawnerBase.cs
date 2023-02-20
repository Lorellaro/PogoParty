using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class PlayerSpawnerBase : MonoBehaviour
{
    [SerializeField] Leaderboard leaderboard;
    public GameObject playerPrefab;

    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    [SerializeField] private bool disableOnJoin;

    protected Vector3 randomPos;
    protected GameObject spawnedPlayer;

    public virtual void Start()
    {
        randomPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
        if (disableOnJoin) InputManager.Instance.AllowInput(false);

        leaderboard.gameObject.GetComponent<PhotonView>().RPC("addToLeaderboardList", RpcTarget.AllBuffered, "/N.A/", 0);

        leaderboard.gameObject.GetComponent<PhotonView>().RPC("searchForNewPlayerGameObjects", RpcTarget.AllBuffered);
    }
}
