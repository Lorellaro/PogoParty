using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public LevelManager levelManager;

    // public GameObject fadeScreen;
    //
    //
    // readonly string[] scenes = {
    //      "Tutorial",
    //      "Cave",
    //      "Third_Level"
    //  };
    public int index;

    private bool triggered = false;
    //
    private void Awake()
    {
        // if (fadeScreen == null)
        // {
        //     fadeScreen = GameObject.Find("FadeScreen");
        // }
        //
        // fadeScreen.SetActive(false);
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }
    //
    // private void Start() {
    //     for(int i = 0; i < scenes.Length; i++){
    //         if(scenes[i] == SceneManager.GetActiveScene().name){
    //             index = i;
    //         }
    //     }
    // }
    private void OnTriggerEnter(Collider other) {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player" && !triggered)
        {
            // fadeScreen.SetActive(true);
            // SceneManager.LoadScene(scenes[index+1 % scenes.Length]);
            triggered = true;
            levelManager.UnloadScene(index);
            
        }
    }
}
