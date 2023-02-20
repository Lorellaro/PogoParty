using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Timer myTimer;
    [SerializeField] float startTime;

    private void OnEnable()
    {
        Timer.current.onTimerFinish += endGame;
    }

    private void OnDisable()
    {
        Timer.current.onTimerFinish -= endGame;
    }

    private void Start()
    {
        myTimer.SetStartTime(startTime);
        myTimer.SetTime(startTime);
    }

    private void Update()
    {
        float currentTime = myTimer.getTime();

        //Format to alarm format (e.g. 2:30)
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.SetText(niceTime);
    }

    private void endGame()
    {
        print("Game over");
    }
}
