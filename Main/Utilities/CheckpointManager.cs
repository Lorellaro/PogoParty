using System;
using System.Collections.Generic;
using Main.Utilities;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> checkpoints = new List<GameObject>();
    private GameController _gameController;
    public static CheckpointManager Instance;

    private void Awake()
    {
        Instance = this;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Flag");
        foreach (var item in gameObjects)
        {
            checkpoints.Add(item);
        }
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        checkpoints.Sort(Utilities.SortByName);
    }

    public int GetNumberOfCheckpoints()
    {
        return checkpoints.Count;
    }

    public void GoToCheckpoint(int checkpointID)
    {
        if (checkpointID > checkpoints.Count)
        {
            DebugController.Instance.DisplayCommandOutput("No checkpoint found with ID: " + checkpointID);
            Debug.LogError("No checkpoint found with ID: " + checkpointID);
        }
        else
        {
            var checkpointPos = checkpoints[checkpointID].transform.position;
            _gameController.SavePlayer(checkpointPos.x, checkpointPos.y, checkpointPos.z);
            _gameController.LoadPlayer();
        }
    }

    public void GoToCheckpoint(string checkpointName)
    {
        foreach (var checkpoint in checkpoints)
        {
            if (checkpoint.name == checkpointName)
            {
                var checkpointPos = checkpoint.transform.position;
                _gameController.SavePlayer(checkpointPos.x, checkpointPos.y, checkpointPos.z);
                _gameController.LoadPlayer();
                return;
            }
        }
        DebugController.Instance.DisplayCommandOutput("No checkpoint found with the name: " + checkpointName);
        Debug.LogError("No checkpoint found with the name: " + checkpointName);
    }
    
}