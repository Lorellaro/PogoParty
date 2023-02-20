using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AirshipMinigameController : MonoBehaviour
{
    [SerializeField] TextMeshPro pointsText;

    private int points = 0;

    public void setPoints(int pointsToSetTo)
    {
        points = pointsToSetTo;
        pointsText.text = points.ToString();
    }

    public int getPoints()
    {
        return points;
    }

    void Start()
    {
        pointsText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
