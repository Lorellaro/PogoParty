using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameStarter : MonoBehaviour
{
    [SerializeField] List<GameObject> allObjsNeedingSyncing;

    PhotonView photonView;

    float roomTime;

    bool started;

    // Start is called before the first frame update
    void Start()
    {
         photonView = GetComponent<PhotonView>();

        for (int i = 0; i < allObjsNeedingSyncing.Count; i++)
        {
            allObjsNeedingSyncing[i].SetActive(false);
        }
    }

    void Update()
    {
        if (started) { return; }

        if (roomTime > 15f)
        {

            for (int i = 0; i < allObjsNeedingSyncing.Count; i++)
            {
                allObjsNeedingSyncing[i].SetActive(true);
            }

            started = true;

            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            roomTime += Time.deltaTime;
            this.photonView.RPC("SetRoomTime", RpcTarget.AllBuffered, roomTime);
        }
        else
        {


        }

    }

    [PunRPC]
    public void SetRoomTime(float _roomTime)
    {
        roomTime = _roomTime;
    }
}
