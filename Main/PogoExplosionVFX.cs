using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PogoExplosionVFX : MonoBehaviour
{
    [SerializeField] VisualEffect pogoExplosionVFX1;
    [SerializeField] VisualEffect pogoExplosionVFX2;
    [SerializeField] List<ParticleSystem> webGLExplosions;
    [SerializeField] bool isWebGLBuild = true;

    public void explode()
    {
        if (!isWebGLBuild)
        {
            pogoExplosionVFX1.Play();
            pogoExplosionVFX2.Play();
        }
        else
        {
            for(int i = 0; i < webGLExplosions.Capacity; i++)
            {
                webGLExplosions[i].Play();
            }

        }

    }
}
