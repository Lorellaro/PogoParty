using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PogoPlayerShop : MonoBehaviour
{

    private Rigidbody rb;
    private Rigidbody[] _rigidbodiesRagdoll;
    [SerializeField] private GameObject llwelynModel;
    public List<Vector3> ragdollPositions;
    public bool getRagdollPositions;
    
    private void Awake()
    {
        rb = transform.GetChild(2).GetChild(0).GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbodiesRagdoll = llwelynModel.gameObject.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < _rigidbodiesRagdoll.Length; i++)
        {
            _rigidbodiesRagdoll[i].position = ragdollPositions[i];
        }
    }

    // private void Update()
    // {
    //     if (getRagdollPositions)
    //     {
    //         getRagdollPositions = false;
    //         ragdollPositions = new List<Vector3>();
    //         foreach (var bone in _rigidbodiesRagdoll)
    //         {
    //             ragdollPositions.Add(bone.position);
    //         }
    //     }
    // }

    private void FixedUpdate()
    {
        var rot2 = Quaternion.FromToRotation(transform.GetChild(2).GetChild(0).forward, Vector3.up);//get the rotation towards upwards
        rb.AddTorque(new Vector3(rot2.x, rot2.y, rot2.z) * 2.5f / 2);//rotate upwards
    }
}