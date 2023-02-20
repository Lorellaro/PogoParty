using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    /*
    // used for access in other scripts
    public static bool finished = false;
    // used to stop ring 1 from being always active
    bool started = false;

    public void Triggered(Ring ring)
    {
        for (int a = 0; a < transform.childCount; a++)
        {
            /*
            // if no timer set all rings false
            if (!Timer.start)
            {
                transform.GetChild(a).gameObject.SetActive(false);
            }
            // check if timer has started and making sure it is only run once with started
            else if (Timer.start && !started)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                started = true;
            }
            else
            {
                // end timer
                if (a == transform.childCount - 1)
                {
                    transform.GetChild(a).gameObject.SetActive(false);
                    finished = true;
                    // reset rings
                    started = false;
                    break;

                }
                // sets next active ring
                else if (transform.GetChild(a).gameObject.activeSelf)
                {
                    // sets ring false
                    transform.GetChild(a).gameObject.SetActive(false);
                    // makes next ring true
                    transform.GetChild(a + 1).gameObject.SetActive(true);
                    // escape loop so new active ring is not set false 
                    break;
                }
            }
        }
    }
/*/
}

