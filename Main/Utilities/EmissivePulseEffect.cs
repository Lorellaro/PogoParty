using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissivePulseEffect : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Color startColor;
    [SerializeField] float pulseTime;
    [SerializeField] float emissionGain;
    [SerializeField] float startIntensity;
    [SerializeField] float fadeSpeed = 1.5f;

    float intensity;
    bool up;
    bool faded;

    private void Awake()
    {
        material.DisableKeyword("_EMISSION");
        material.EnableKeyword("_EMISSION");
        StartCoroutine(moveFloatUP());
        intensity = startIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (up)
        {
            intensity += emissionGain * Time.deltaTime;
        }
        else
        {
            intensity -= emissionGain * Time.deltaTime;

        }

        material.SetColor("_EmissionColor", startColor * intensity);
    }

    private IEnumerator moveFloatUP()
    {
        up = true;
        yield return new WaitForSeconds(pulseTime);
        up = false;
        yield return new WaitForSeconds(pulseTime);
        StartCoroutine(moveFloatUP());
    }

    public void fadeMaterialOut()
    {
        if (faded) { return; }
        faded = true;
        StartCoroutine(AlphaFadeOut());
    }

    // This method fades only the alpha.
    IEnumerator AlphaFadeOut()
    {
        // Alpha start value.
        float alpha = 1.0f;

        // Loop until aplha is below zero (completely invisalbe)
        while (alpha > 0.0f)
        {
            // Reduce alpha by fadeSpeed amount.
            alpha -= fadeSpeed * Time.deltaTime;

            // Create a new color using original color RGB values combined
            // with new alpha value. We have to do this because we can't 
            // change the alpha value of the original color directly.
            material.color = (new Color(material.color.r, material.color.g, material.color.b, alpha));

            yield return null;
        }
    }

    // This method fades only the alpha.
    IEnumerator AlphaFadeIn()
    {
        // Alpha start value.
        float alpha = 0f;

        // Loop until aplha is below zero (completely invisalbe)
        while (alpha < 1.0f)
        {
            // Reduce alpha by fadeSpeed amount.
            alpha += fadeSpeed * Time.deltaTime;

            // Create a new color using original color RGB values combined
            // with new alpha value. We have to do this because we can't 
            // change the alpha value of the original color directly.
            material.color = (new Color(material.color.r, material.color.g, material.color.b, alpha));

            yield return null;
        }
    }

    public void fadeMaterialIn()
    {
        if (!faded) { return; }
        faded = false;
        StartCoroutine(AlphaFadeIn());
    }
}
