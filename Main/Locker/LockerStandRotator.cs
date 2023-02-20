using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerStandRotator : MonoBehaviour
{
    [SerializeField] Transform pogostickTransform;
    [SerializeField] float rotateSpeed;
    [SerializeField] Vector3 standRotation;
    [SerializeField] Vector3 pogoRotation;

    Rigidbody standRB;
    InputManager _inputManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (_inputManager == null)
        {
            _inputManager = InputManager.Instance;
        }

        standRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if pushed left key
        if(_inputManager.pedestalMovementInput.x < 0)
        {
            //Rotate left
            standRB.AddTorque(standRotation * rotateSpeed * Time.deltaTime);
            pogostickTransform.Rotate(pogoRotation * rotateSpeed * Time.deltaTime);
        }

        //if pushed right key
        else if(_inputManager.pedestalMovementInput.x > 0)
        {
            //Rotate right
            standRB.AddTorque(-standRotation * rotateSpeed * Time.deltaTime);
            pogostickTransform.Rotate(-pogoRotation * rotateSpeed * Time.deltaTime);
        }
    }
}
