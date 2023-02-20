using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeController : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLook;

    public void HoldShake(float amp)
    {
        setShake(amp);
    }

    public IEnumerator BurstShake(float amp, float shakeTime)
    {
        setShake(amp);
        yield return new WaitForSeconds(shakeTime);
        DisableShake();
    }

    private void DisableShake()
    {
        setShake(0);
    }

    private void setShake(float amp)
    {
        for (int i = 0; i < 3; i++)
        {
            freeLook.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amp;
        }
    }
}
