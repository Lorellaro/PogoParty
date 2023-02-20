using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliderAfterXTime : MonoBehaviour
{
    [SerializeField] float timeUntilDisable;
    [SerializeField] Collider col;

    private void Start()
    {
        StartCoroutine(disableTimer());
    }

    private IEnumerator disableTimer()
    {
        yield return new WaitForSeconds(timeUntilDisable);
        col.enabled = false;
    }
}
