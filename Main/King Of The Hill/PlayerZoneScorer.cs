using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerZoneScorer : MonoBehaviour
{
    //THIS SCRIPT SHOULD ALWAYS BE ADDED TO POGOSTICK DURING RUNTIME CURRENTLY THROUGH THE KING OF THE HILL SPAWNER

    [SerializeField] int points;

    public Leaderboard leaderboard;
    public int index;
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "KingOfTheHill1")
        {
            GetComponent<PlayerZoneScorer>().enabled = false;
            return;
        }

        scoreText = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<TextMeshProUGUI>();

        if(GameObject.FindGameObjectWithTag("Leaderboard") == null) { return; }
        else
        {
            leaderboard = GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<Leaderboard>();
        }

        //new game both players have index = 1 due to 
        index = Mathf.RoundToInt(transform.root.GetComponent<PhotonView>().ViewID / 1000) - 1;

    }

    public int GetPoints()
    {
        return points;
    }

    public void SetPoints(int _points)
    {
        points = _points;

        //Index is incorrect for users that werent the first to join

        //update leaderboard info

        //leaderboard.gameObject.GetComponent<PhotonView>().RPC("setPlayerScore", RpcTarget.AllBuffered, index, points);

        leaderboard.setPlayerScore(index, points);
        scoreText.SetText(points.ToString());
    }
}
