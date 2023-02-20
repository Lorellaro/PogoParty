using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFade : MonoBehaviour
{
    [SerializeField] float fadeTime;
    [SerializeField, Range(0, 1)] float imageColourMaxAlpha = 1;

    // the image you want to fade, assign in inspector
    public Image img;
    float currentAlpha;
    bool fadeAway;

    private void Update()
    {
        if (fadeAway)
        {
            if (img.color.a < imageColourMaxAlpha)//Fade in if not fully faded in
            {
                currentAlpha += Time.deltaTime * fadeTime;
                img.color = new Color(1, 1, 1, currentAlpha);
            }
        }
        else
        {
            if (img.color.a > 0)//fade out if not fully faded out
            {
                currentAlpha -= Time.deltaTime * fadeTime;
                img.color = new Color(1, 1, 1, currentAlpha);
            }
        }
    }

    public void fadeIn()
    {
        fadeAway = true;
    }

    public void fadeOut()
    {
        fadeAway = false;
    }

}