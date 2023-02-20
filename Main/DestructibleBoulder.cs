using UnityEngine;
using System.Collections;

public class DestructibleBoulder : MonoBehaviour
{
    public float strength;
    public float scaleScalar = 0.05f;
    Transform[] transforms;

    private void Start() {
        transforms = GetComponentsInChildren<Transform>();
        Rigidbody[] children = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody child in  children){
            child.AddForce(Random.onUnitSphere.normalized * strength);
        }
    }
    private void FixedUpdate() {
        for (int i = transforms.Length-1; i >= 1; i--){
            if(transforms[i].localScale.x < 0.1f){
                Destroy(gameObject, 0);
            }
            transforms[i].localScale -= new Vector3(scaleScalar, scaleScalar, scaleScalar);
        }
    }
}