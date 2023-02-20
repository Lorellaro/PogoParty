using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Leaderboard : MonoBehaviour
{
    [Header("Crowns")]
    [SerializeField] GameObject goldCrown;
    [SerializeField] GameObject silverCrown;
    [SerializeField] GameObject bronzeCrown;
    [SerializeField] Vector3 crownScale;
    [SerializeField] Vector3 crownRot;
    [SerializeField] Vector3 crownPos;

    [SerializeField] List<int> playerScores;
    [SerializeField] List<string> playerNames;
    //[SerializeField] KingOfTheHillSpawner kingOfTheHillSpawner;

    //OBJECT AND TRANSFORM LISTS BOTH POINT TO THE WRONG OBJECT ON THE SECOND USER'S GAME
    [SerializeField] public List<GameObject> playersObjs;

    [SerializeField] public List<Transform> pogostickPhysTransform;

    [SerializeField] List<int> topThreeIndexs;

    [Tooltip("Allows enabling and disabling of the gold silver and bronze crowns")]
    [SerializeField] bool isCrownActive = true;

    private void Update()
    {

        if (!isCrownActive) { return; }

        //Get highest value and assign them crown

        int[] ThreeHighestPointIndexes = (Utilities.GetMaxThreeArrayElement(playerScores.ToArray()));

        //when in second user's world  gold crown is given to who ever is second and silver crown is given to who ever is first

        //Return if there is no points gained so far
        if(playerScores[ThreeHighestPointIndexes[0]] <= 0) { return; }

        //Gold Crown
        //only reset parent and position if it has not already done so

        
        if (goldCrown.transform.parent != pogostickPhysTransform[ThreeHighestPointIndexes[0]]) // index point 0 is representative of the highest value in the array
        {
            //SetParent
            goldCrown.transform.SetParent(pogostickPhysTransform[ThreeHighestPointIndexes[0]]);

            //Set Transform info
            setupCrownPos(goldCrown);
        }

        //return if only one user has points
        if (playerScores[ThreeHighestPointIndexes[1]] <= 0) { return; }

        //Silver Crown
        //only reset parent and position if it has not already done so
        if (silverCrown.transform.parent != pogostickPhysTransform[ThreeHighestPointIndexes[1]])// index point 1 is representative of the second highest value in the array
        {
            //SetParent
            silverCrown.transform.SetParent(pogostickPhysTransform[ThreeHighestPointIndexes[1]]);

            //Set Transform info
            setupCrownPos(silverCrown);
        }

        //return if only two users have points and if there are three users
        if (playerScores[ThreeHighestPointIndexes[2]] <= 0 || playersObjs.Count < 3) { return; }

        //Bronze Crown
        //only reset parent and position if it has not already done so
        if (bronzeCrown.transform.parent != pogostickPhysTransform[ThreeHighestPointIndexes[2]])// index point 2 is representative of the third highest value in the array
        {
            //SetParent
            bronzeCrown.transform.SetParent(pogostickPhysTransform[ThreeHighestPointIndexes[2]]);

            //Set Transform info
            setupCrownPos(bronzeCrown);
        }
    }

    private void setupCrownPos(GameObject crown)
    {
        //Set Transform info
        crown.transform.localScale = crownScale;
        crown.transform.localRotation = Quaternion.Euler(crownRot);
        crown.transform.localPosition = crownPos;
    }

    [PunRPC]
    public void addToLeaderboardList(string playerName, int scoreIndex)
    {
        //Could be put to 0
        playerScores.Add(0);
        playerNames.Add(playerName);
        //playerTransforms.Add(playerRagdollRootTransform);
    }

    [PunRPC]
    public void searchForNewPlayerGameObjects()
    {

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("PlayerRoot");

        //DIVIDED BY 16 AS THATS HOW MANY PLAYER TAGS THERE ARE ON PER PLAYER
        //print(allPlayers.Length / 16);

        //Most recently added elemnet
        int lastElementInList = allPlayers.Length -1;

        //Check if already present in list if not then add it
        for(int i = 0; i < allPlayers.Length; i++)
        {
            //If player objs is empty
            if (playersObjs.Count <= 0) 
            {
                //add and return
                playersObjs.Add(allPlayers[i].gameObject.transform.root.gameObject);

                //SORT LIST BY PHOTON VIEW ID

                //Update transform list
                pogostickPhysTransform.Add(allPlayers[i].gameObject.transform.root.GetChild(2).GetChild(0));

                return;
            }

            //check if player object is already present
            if (allPlayers[lastElementInList].gameObject.transform.root.gameObject != playersObjs[(i)].transform.root.gameObject)
            {
                //add new
                playersObjs.Add(allPlayers[lastElementInList].gameObject.transform.root.gameObject);

                //SORT LIST BY PHOTON VIEW ID

                //Bubble sort based off of photon view ids

                List<GameObject> newPlayerObjs = new List<GameObject>();

                //fill list with empty elements of size equal to playerObjs
                for(int j = 0; j < playersObjs.Count; j++)
                {
                    newPlayerObjs.Add(gameObject);
                }

                //sort
                for(int j = 0; j < playersObjs.Count; j++)
                {
                    int correctIndex = Mathf.RoundToInt(playersObjs[j].GetComponent<PhotonView>().ViewID / 1000) - 1;
                    newPlayerObjs[correctIndex] = playersObjs[j];
                }

                //replace
                playersObjs = newPlayerObjs;

                //Update transform list
                pogostickPhysTransform.Add(allPlayers[lastElementInList].gameObject.transform.root.GetChild(2).GetChild(0));

                List<Transform> newPlayerTrans = new List<Transform>();

                for(int j = 0; j < playersObjs.Count; j++)
                {
                     newPlayerTrans.Add(playersObjs[j].transform.GetChild(2).GetChild(0));
                }

                pogostickPhysTransform = newPlayerTrans;

                return;
            }
        }

        //ONCE WE HAVE THE POGOSTICKPHYS TRANSFORM LIST BUILT WE MUST THEN SORT IT SO THAT USERS WITH THE HIGHEST PHOTON VIEW ID ARE AT THE TOP!!!
        
    }

    public int GetPlayerCount()
    {
        return playerScores.Count;
    }

    //[PunRPC]
    public void setPlayerScore(int index, int scoreToAdd)
    {
        playerScores[index] = scoreToAdd;
    }

    //returns all player roots
    public List<GameObject> GetAllPlayers()
    {
        return playersObjs;
    }

    public void updateLeaderboardUIElement(int index)
    {

    }
}
