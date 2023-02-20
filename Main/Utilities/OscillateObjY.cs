using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateObjY : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDistance;

    float elapsedTime;

    Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        pos.y = pos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = pos; // new position
    }
}
