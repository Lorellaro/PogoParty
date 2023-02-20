using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Trigger : MonoBehaviour
{
    [SerializeField] private MovingPlatform _movingPlatform;
    private bool _platformCalled;
    [HideInInspector]
    [SerializeField] BoxCollider _boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        if (_movingPlatform == null)
        {
            Debug.LogError("Moving Platoform is not referenced");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_movingPlatform == null) return;
        if(_platformCalled) return;
        _movingPlatform.StartPlatform();
        _platformCalled = true;

    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _boxCollider.size);
    }
}
