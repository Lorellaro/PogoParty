using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjLookAtCam : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] bool invert = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (invert)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        }
        else
        {
            transform.LookAt(mainCam.transform.position);
        }
    }
}
