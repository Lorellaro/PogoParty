using System.Collections;
using System.Collections.Generic;
using Main.GameHandlers;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] private FishPath[] paths;

    // [SerializeField] private FishController[] fishes;

    [SerializeField] private FishAssignments[] fishAssignments;
    // Start is called before the first frame update
    void Start()
    {
        if(RoundManager.Instance == null) Activate();
        else RoundManager.Instance.onRoundManagerReady += Activate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Activate()
    {
        for (int i = 0; i < fishAssignments.Length; i++)
        {
            for (int j = 0; j < fishAssignments[i].fishes.Length; j++)
            {
                fishAssignments[i].fishes[j].Activate(paths[i].GetPathPoints());
            }
        }
    }
}

[System.Serializable]
public class FishAssignments
{
    public FishController[] fishes;
}

