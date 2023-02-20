using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantDishSliderCanvas : MonoBehaviour
{
    [SerializeField] ImageFade sliderBgd;
    [SerializeField] ImageFade sliderFill;
    [SerializeField] Slider mySlider;
    [SerializeField] float YOffsetFromPlayer = 2f;

    public RestaurantDishGiver restaurantDishGiver;
    public GameObject myPlayer;
    float maxSliderValue;

    void Update()
    {
        if(myPlayer == null) { return; }

        Vector3 newPos = new Vector3(myPlayer.transform.position.x, myPlayer.transform.position.y + YOffsetFromPlayer, myPlayer.transform.position.z);
        transform.position = newPos;

        //maxSliderValue = restaurantDishGiver.timeToTakeDish;
        float currentTimeOnHotplate = restaurantDishGiver.dishGivingTime;
        mySlider.value = currentTimeOnHotplate;

        if (currentTimeOnHotplate <= 0)
        {
            sliderBgd.fadeOut();
            sliderFill.fadeOut();
        }
        else
        {
            sliderBgd.fadeIn();
            sliderFill.fadeIn();
        }
    }
}
