using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomSeedGenerator : MonoBehaviourPunCallbacks
{
    [SerializeField] public float randomSeed;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)//(PhotonNetwork.IsMasterClient)
        {
            //generate seed
            //Can be dependant on duration of match
            Random.InitState(System.DateTime.Now.Millisecond);
            randomSeed = Mathf.Round(Random.Range(0, 9999999999999999));//1111111111111
            GetComponent<PhotonView>().RPC("SetRandomSeed", RpcTarget.AllBuffered, randomSeed);

        }
    }

    [PunRPC]
    public void SetRandomSeed(float _randomSeed)
    {
        randomSeed = _randomSeed;
    }

}
