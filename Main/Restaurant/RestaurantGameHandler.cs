using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.GameHandlers;

public class RestaurantGameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RoundManager.Instance.onRoundManagerReady += startRound;
    }

    private void startRound()
    {
        print("start game");
    }
}
