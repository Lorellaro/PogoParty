using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisasterWarningUI : MonoBehaviour
{
    [SerializeField] float flashTime;
    [SerializeField] float pulseSize;

    Vector3 initObjScale;
    Image warningImage;

    // Start is called before the first frame update
    void Start()
    {
        initObjScale = transform.localScale;
        warningImage = GetComponent<Image>();
        LeanTween.scale(gameObject, initObjScale * pulseSize, flashTime).setEaseInOutBounce().setLoopPingPong();
    }

    public void disableWarningUI()
    {
        warningImage.enabled = false;
    }

    public void enableWarningUI()
    {
        warningImage.enabled = true;
    }
}
