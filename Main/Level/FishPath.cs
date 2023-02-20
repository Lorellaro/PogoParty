using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPath : MonoBehaviour
{
    [SerializeField] private Transform[] pathPoints;

    [SerializeField] private bool reverse;

    public Transform[] GetPathPoints()
    {
        return pathPoints;
    }

    public bool IsReversed()
    {
        return reverse;
    }

    private void OnDrawGizmos()
    {
        if(pathPoints.Length < 2) return; 
        Gizmos.color = Color.green;
        for (int i = 0; i < pathPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(pathPoints[i].position, 1);
            if(i + 1 >= pathPoints.Length) Gizmos.DrawLine(pathPoints[i].position, pathPoints[0].position);
            else Gizmos.DrawLine(pathPoints[i].position, pathPoints[i+1].position);
        }
        // Gizmos.DrawSphere(pathPoints[0].position, 1);
    }
}
