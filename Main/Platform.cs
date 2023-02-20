using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject player;
    // attach to platform
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "PogoStickBot")
        {
            // set pogo sticks transform to platform's transform
            player.transform.parent = transform;
        }
    }
    // detach from platfrom
    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "PogoStickBot")
        {
            // set pogo transform back to original
            player.transform.parent = null;
        }
    }
}
