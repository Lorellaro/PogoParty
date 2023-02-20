using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrucibleObject : MonoBehaviour
{
    [SerializeField] Transform[] childrenTransforms;
    [SerializeField] Rigidbody[] childrenRigidbodies;

    [SerializeField] float strength;
    [SerializeField] float scaleScalar = 0.05f;

    private void Start()
    {
        foreach (Rigidbody child in childrenRigidbodies)
        {
            child.AddForce(Random.onUnitSphere.normalized * strength * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < childrenTransforms.Length; i++)
        {
            if (childrenTransforms[i].localScale.x < 0.1f)
            {
                Destroy(gameObject, 0);
            }

            childrenTransforms[i].localScale -= new Vector3(scaleScalar, scaleScalar, scaleScalar) * Time.deltaTime;
        }
    }

    public Transform[] GetTransforms()
    {
        return childrenTransforms;
    }
}
