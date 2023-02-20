using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionPlaySfx : MonoBehaviour
{
    [SerializeField] GameObject landingSfx;
    [SerializeField] LayerMask allowedLayers;

    float cooldownTime = 0.2f;
    bool hasCooledDown = true;

    private void OnCollisionEnter(Collision collision)
    {
        //Wait period until can play sfx again
        if (!hasCooledDown) { return; }

        //check layers
        if ((allowedLayers.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            Instantiate(landingSfx, collision.transform.position, Quaternion.identity);
            StartCoroutine(CannotInstaniateUntilTime());
        }
    }

    private IEnumerator CannotInstaniateUntilTime()
    {
        hasCooledDown = false;
        yield return new WaitForSeconds(cooldownTime);
        hasCooledDown = true;
    }
}
