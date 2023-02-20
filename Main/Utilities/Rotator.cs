using System;
using System.Collections;
using System.Collections.Generic;
using Main.GameHandlers;
using Main.Level.Race;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField] private float speed = 5;
    [SerializeField] private RotationDirection directionToRotate;
    [SerializeField] private Transform pivot;

    private Vector3 rotDir;
    private bool isActive;
    
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (RoundManager.Instance == null)
        {
            Activate();
        }
        RoundManager.Instance.onRoundManagerReady += Activate;
        if (pivot == null)
        {
            pivot = transform;
        }
        switch (directionToRotate)
        {
            case RotationDirection.Up:
                rotDir = Vector3.up;
                break;
            case RotationDirection.Down:
                rotDir = Vector3.down;
                break;
            case RotationDirection.Left:
                rotDir = Vector3.left;
                break;
            case RotationDirection.Right:
                rotDir = Vector3.right;
                break;
            case RotationDirection.Forward:
                rotDir = Vector3.forward;
                break;
            case RotationDirection.Backward:
                rotDir = Vector3.back;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(directionToRotate), directionToRotate, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isActive) return;
        pivot.Rotate(rotDir, 5f * speed * Time.deltaTime);
    }

    private enum RotationDirection
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Backward
    }
    
    private void Activate()
    {
        isActive = true;
    }
}
