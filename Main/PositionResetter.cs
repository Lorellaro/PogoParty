using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionResetter : MonoBehaviour
{
    [SerializeField] Transform respawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform playerTransform = other.gameObject.transform.root;
            ResetPlayerPosition(playerTransform);
        }
    }

    public void ResetPlayerPosition(Transform playerTransform)
    {
        playerTransform.GetChild(0).GetChild(1).GetChild(4).transform.position = respawnPos.position;//Ragdoll root obj
        playerTransform.GetChild(2).GetChild(0).transform.position = respawnPos.position;//Pogostick

        Rigidbody[] allRbs = playerTransform.GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < allRbs.Length; i++)
        {
            allRbs[i].velocity = Vector3.zero;
            allRbs[i].angularVelocity = Vector3.zero;
        }
        //playerTransform.GetChild(2).GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
        //playerTransform.GetChild(2).GetChild(0).GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
