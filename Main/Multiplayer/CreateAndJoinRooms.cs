using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("Race_Level");
        //PhotonNetwork.LoadLevel("Restaurant");
        //PhotonNetwork.LoadLevel("Cave");
        //PhotonNetwork.LoadLevel("KingOfTheHill1");
        //PhotonNetwork.LoadLevel("Cosmetics_Shop");
        PhotonNetwork.LoadLevel("MainHub");
        //PhotonNetwork.LoadLevel("Griefing_Test");
        //PhotonNetwork.LoadLevel("DockRace");
    }
}
