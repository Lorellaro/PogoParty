using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    // Create a bond between 2 points
    // cylinderRef is a gameObject mesh cylinder with the cylinder pivot at the base pointing along the +Z.
     
    // var cylinderRef : Transform;
    // var aTarget : Transform;
    // var spawn : Transform;
    [SerializeField] private Transform cylinderRef, aTarget, spawn;
     
    void Start() {
        // Find the distance between 2 points
        Vector3 newScale = cylinderRef.transform.localScale;
        newScale.y = Vector3.Distance(spawn.position,aTarget.position) / 2;
        cylinderRef.transform.localScale = newScale;


        // cylinderRef.localScale = new Vector3(cylinderRef.localScale.x, dist, cylinderRef.localScale.z);
        Vector3 newPos = new Vector3(spawn.position.x, spawn.position.y + newScale.y, spawn.position.z);
        cylinderRef.position = newPos;        // place bond here
        cylinderRef.LookAt(aTarget);            // aim bond at atom
    }

}
