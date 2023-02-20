using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    // child script for trigger detection
    private void OnTriggerEnter(Collider coll)
    {
        if (LayerMask.LayerToName(coll.gameObject.layer) == "PogoStickBot")
        {
            // detects if the ring has been hit and sends collider back to parent scripts
            //transform.parent.GetComponent<RingController>().Triggered(this);

        }
    }
}
