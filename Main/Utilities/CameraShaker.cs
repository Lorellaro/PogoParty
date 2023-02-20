using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    CameraShakeController cameraShakeController;

    void Start()
    {
        cameraShakeController = GameObject.FindGameObjectWithTag("Game Controller").GetComponent<CameraShakeController>();
    }

    public void shakeCamera(float amp, float duration)
    {
        StartCoroutine(cameraShakeController.BurstShake(amp, duration));
    }
}
