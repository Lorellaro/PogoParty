using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBarrel : MonoBehaviour
{
    [SerializeField] AudioSource myAudioSource;

    [Tooltip("Bounce multiplier when hitting bounce flooring")]
    [SerializeField] float bounceMultiplier;

    [SerializeField] float bouncePitchIncrease;
    Material barrelMainMaterial;

    float multiplier = 1f;

    float colourStrength = 0;

    private void Start()
    {
        colourStrength = 0;
        var renderer = GetComponent<MeshRenderer>();
        barrelMainMaterial = GetComponent<MeshRenderer>().material;
        renderer.material = barrelMainMaterial;
    }

    public float GetMultiplier()
    {
        return multiplier;
    }

    public float GetColorStrength()
    {
        return colourStrength;
    }

    private void OnDestroy()
    {
        if(barrelMainMaterial != null)
        {
            Destroy(barrelMainMaterial);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BarrelMinigameFlooring"))
        {
            multiplier += bounceMultiplier;
            myAudioSource.pitch += bouncePitchIncrease;

            //make material brighter
            colourStrength += 0.1f;

            barrelMainMaterial.SetFloat("_ColourStrength", colourStrength);
        }

        myAudioSource.Play();
    }
}
