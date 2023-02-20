using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTransform : MonoBehaviour
{
    [SerializeField] float turnSpeed;
    [SerializeField] Vector3 rotateAxis;
    [SerializeField] bool pingPongScale;

    Vector3 initScale;

    private void Start()
    {
        if (pingPongScale)
        {
            initScale = transform.localScale;
            LeanTween.scale(gameObject, initScale * 1.5f, 0.3f).setEaseInOutSine().setLoopPingPong();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAxis * turnSpeed * Time.deltaTime);
    }
}
