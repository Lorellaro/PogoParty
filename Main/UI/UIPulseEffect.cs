using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPulseEffect : MonoBehaviour
{
    [SerializeField] float pulseTime;
    [SerializeField] float pulseResizeScale;

    Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    public void pulse()
    {
        StartCoroutine(pulseEffect());
    }

    private IEnumerator pulseEffect()
    {
        LeanTween.scale(gameObject, startScale * pulseResizeScale, pulseTime).setEaseInOutSine();
        yield return new WaitForSeconds(pulseTime);
        LeanTween.scale(gameObject, startScale, pulseTime / 2).setEaseInOutSine();
    }
}
