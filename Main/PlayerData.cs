using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// https://www.youtube.com/watch?v=XOjd_qU2Ido&t=325s Save system 
public class PlayerData
{
    // data to be stored
    public float[] position;
    public bool tutorialCompleted;

    public PlayerData(PogoStickPhysics player)
    {
        // store players data
        // can only store primitive data types
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        // store whether the player has completed the tutorial or not
        tutorialCompleted = player.tutorialCompleted;
    }
}
