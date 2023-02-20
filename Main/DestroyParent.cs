using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    [SerializeField] float time = 1f;

    private void Start()
    {
        Destroy(gameObject, time);
        Destroy(gameObject.transform.parent.gameObject, time + 0.2f);
    }
}
