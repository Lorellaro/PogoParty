using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public Button[] buttons;
    // public GameObject shopButton;
    // public GameObject backButton;
    public int value;
    public PogoStickPhysics player;
    public float bounceForceIncr = 0.3f;
    public float bounceFuelCostIncr = 0.5f;
    public float chargeTimeIncr = 0.3f;
    public float maxBounceForce = 20.0f;
    public float minBounceFuelCost = 1.0f;
    public float maxChargeTime = 10.0f;
    public GameController gameController;
    public TMP_Text gemCounter;
    
    private void Start() {   
        // shopButton.Select();
        UpdateGemCounter();
    }
    // press button to access shop
    private void Update()
    {
        foreach (Button b in buttons){

            b.interactable = true;
            if(player.total - value < 0){
                b.interactable = false;
            }
        }
        // if(player.total - value < 0){
        //     _buttonFocus.UpdateSelectedButton(backButton);
        // }
    }

    // jump force increase 
    public void increaseJump()
    {
        float bounceForce = player.getBounceForce();
        if(bounceForce + bounceForceIncr > maxBounceForce) return;
        player.total -= value;
        UpdateGemCounter();
        bounceForce += bounceForceIncr;
        player.setBounceForce(bounceForce);
    }
    // fuel consumption decrease
    public void decreaseConsumption()
    {
        /*
        float bounceFuelCost = player.getBounceFuelCost();
        if(bounceFuelCost - bounceFuelCostIncr < minBounceFuelCost) return;
        player.total -= value;
        UpdateGemCounter();
        bounceFuelCost -= bounceFuelCostIncr;
        player.setBounceFuelCost(bounceFuelCost);
        */
    }
    // faster charge up time to jump
    public void decreaseChargeUp(){
        float chargeTime = player.getChargeTime();
        if(chargeTime + chargeTime > maxChargeTime) return;
        player.total -= value;
        UpdateGemCounter();
        chargeTime += chargeTimeIncr;
        player.setChargeTime(chargeTime);
    }

    public void back(){
        //gameController.toggleMenu();
        gameController.SavePlayer();
    }

    public void UpdateGemCounter()
    {
        gemCounter.text = player.total.ToString();
    }
    
}
