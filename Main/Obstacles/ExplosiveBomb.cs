using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ExplosiveBomb : MonoBehaviour
{
    [SerializeField] VisualEffect sparksVFX;


    private void Awake()
    {

        playSparks();
        Destroy(gameObject, 1.5f);
    }

    public void playSparks()
    {
        sparksVFX.Play();
    }
}
