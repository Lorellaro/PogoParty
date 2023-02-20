using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowThenShrink : MonoBehaviour
{
    [SerializeField] float growTime = 0.1f;
    [SerializeField] float shrinkTime = 0.2f;
    [SerializeField] Vector3 GrowScale;
    [SerializeField] bool playOnStart = true;
    [SerializeField] bool destroyOnFinish = true;


    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart)
        {
            StartCoroutine(animate());
        }
    }

    public void GrowThenShrinkSelf()
    {
        StartCoroutine(animate());
    }

    private IEnumerator animate()
    {
        LeanTween.scale(gameObject, GrowScale, growTime).setEaseInExpo();

        yield return new WaitForSeconds(growTime);

        //Pause
        LeanTween.scale(gameObject, GrowScale * 0.95f, growTime);
        yield return new WaitForSeconds(growTime);

        LeanTween.scale(gameObject, Vector3.zero, shrinkTime);
        yield return new WaitForSeconds(shrinkTime);
        if (destroyOnFinish)
        {
            Destroy(gameObject);
        }
    }
}
