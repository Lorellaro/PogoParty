using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Main.GameHandlers.Teams;
using TMPro;

public class CreatePlayerTeams : MonoBehaviour
{
    [SerializeField] Leaderboard leaderboard;
    [SerializeField] float GRACEPERIOD = 5f;// Temporary needs to be replace with all players connected event later on down the road
    [SerializeField] List<GameObject> allPlayers;// can have serialize removed once known it working
    [SerializeField] Teams playerTeams = new Teams();
    [SerializeField] int myPlayerTeamIndex;
    [SerializeField] TextMeshProUGUI myTeamCash;
    [SerializeField] TextMeshProUGUI enemyTeamCash; //gonna need to  be  changed to allow for more than 2 teams

    void Start()
    {
        StartCoroutine(createTeams());
    }

    private IEnumerator createTeams()
    {
        yield return new WaitForSeconds(GRACEPERIOD);
        allPlayers = leaderboard.GetAllPlayers();

        for(int i = 0; i < allPlayers.Count; i++)
        {
            //i % team count gives number between 0 to how many teams there are 
            playerTeams.team[i % playerTeams.team.Count].players.Add(allPlayers[i]);

            if (allPlayers[i].transform.root.GetComponent<PhotonView>().IsMine)
            {
                myPlayerTeamIndex = i % playerTeams.team.Count;
            }
        }
    }

    public int GetMyPlayerTeamIndex()
    {
        return myPlayerTeamIndex;
    }

    [PunRPC]
    public void UpdateCashUI()
    {
        myTeamCash.SetText("$ " + playerTeams.team[myPlayerTeamIndex].score.ToString());
        enemyTeamCash.SetText("$ " + playerTeams.team[(myPlayerTeamIndex + 1) % 2].score.ToString());
    }

    public Teams GetPlayerTeams()
    {
        return playerTeams;
    }
}