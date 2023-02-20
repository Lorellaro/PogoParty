using System;
using System.Collections;
using System.Collections.Generic;
using Main.GameHandlers;
using Main.Level.Race;
using UnityEngine;

public class SwingingAxe : MonoBehaviour
{

    [SerializeField] private Quaternion _start, _end;

    [SerializeField, Range(0.0f, 360f)] private float angle = 90.0f;

    [SerializeField, Range(0.0f, 5.0f)] private float speed = 2.0f;

    [SerializeField, Range(0.0f, 10.0f)] private float startTime = 0.0f;

    [SerializeField] private Transform pivot;

    [SerializeField] AudioSource swingAudioSource;

    private bool isActive;
    bool hasPlayedSfx;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Activate();

        if (RoundManager.Instance == null)
        {
            Activate();

            if (pivot == null)
            {
                pivot = transform;
            }
            _start = AxeRotation(angle);
            _end = AxeRotation(-angle);
        }

        RoundManager.Instance.onRoundManagerReady += Activate;
        if (pivot == null)
        {
            pivot = transform;
        }
        _start = AxeRotation(angle);
        _end = AxeRotation(-angle);
    }

    private void FixedUpdate()
    {
        if (!isActive) return;
        startTime += Time.deltaTime;
        pivot.rotation = Quaternion.Lerp(_start, _end, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1.0f) / 2.0f);

        if (Quaternion.Angle(_start, pivot.rotation) < 5 || Quaternion.Angle(_end, pivot.rotation) < 5)
        {
            if (!hasPlayedSfx)
            {
                hasPlayedSfx = true;
                //swingAudioSource.Play();
                //StartCoroutine(canPlaySFXAgain());
            }
        }
    }

    private IEnumerator canPlaySFXAgain()
    {
        yield return new WaitForSeconds(1f);
        hasPlayedSfx = false;
    }

    private void ResetTimer()
    {
        startTime = 0.0f;
    }

    private Quaternion AxeRotation(float rotAngle)
    {
        var axeRotation = pivot.rotation;
        var angleZ = axeRotation.eulerAngles.z + rotAngle;

        if (angleZ > 180)
            angleZ -= 360;
        else if (angleZ < -180)
            angleZ += 360;
        axeRotation.eulerAngles = new Vector3(axeRotation.eulerAngles.x, axeRotation.eulerAngles.y, angleZ);
        return axeRotation; 
    }

    private void Activate()
    {
        isActive = true;
        //swingAudioSource.Play();
    }
}
