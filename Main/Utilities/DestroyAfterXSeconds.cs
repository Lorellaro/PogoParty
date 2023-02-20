using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterXSeconds : MonoBehaviour
{
    [SerializeField] float time = 1f;
    private void Start()
    {
        Destroy(gameObject, time);
    }
}
