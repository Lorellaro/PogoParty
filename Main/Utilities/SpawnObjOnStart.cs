using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnObjOnStart : MonoBehaviour
{
    [SerializeField] GameObject objToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(objToSpawn.name, transform.position, Quaternion.identity);
    }
}
