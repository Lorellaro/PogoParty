using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStopper : MonoBehaviour
{
    [Header("Define Self")]
    [SerializeField] Spinner spinner;
    [SerializeField] SwingingAxe swingingAxe;
    [SerializeField] Cannon cannon;
    [SerializeField] ZoneScorerMover zoneScorerMover;


    public void EnableScript()
    {
        if (spinner)
        {
            spinner.enabled = true;
        }

        else if (swingingAxe)
        {
            swingingAxe.enabled = true;
        }

        else if (cannon)
        {
            cannon.enabled = true;
        }

        else if (zoneScorerMover)
        {
            zoneScorerMover.enabled = true;
        }
    }
}
