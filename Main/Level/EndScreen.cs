using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{

    public GameObject endScreen;

    private void Awake()
    {
        endScreen = GameObject.Find("EndScreen");
    }

    private void Start()
    {
        endScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            endScreen.SetActive(true);
        }
    }
}
