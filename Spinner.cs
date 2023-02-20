using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float rotationSpeed;

    Vector3 m_EulerAngleVelocity;

    // Start is called before the first frame update
    void Start()
    {
        m_EulerAngleVelocity = new Vector3(rotationSpeed, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
