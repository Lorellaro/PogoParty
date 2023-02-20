using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpHoldUIController : MonoBehaviour
{
    [SerializeField] PogoStickPhysics pogoStickPhysics;
    [SerializeField] ImageFade radialBgd;
    [SerializeField] ImageFade radialFill;

    Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentJumpHoldCounter = pogoStickPhysics.jumpHoldCounter;
        mySlider.value = currentJumpHoldCounter;

        if (currentJumpHoldCounter <= 0)
        {
            radialBgd.fadeOut();
            radialFill.fadeOut();
        }
        else
        {
            radialBgd.fadeIn();
            radialFill.fadeIn();
        }

    }
}
