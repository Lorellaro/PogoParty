using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperController : MonoBehaviour
{
    [SerializeField] Vector3 flipDirection;
    [Tooltip("Must be half of flip time away from time held up")]
    [SerializeField] float startDelay;
    [SerializeField] float timeHeldUp;
    [SerializeField] float timeHeldDown;
    [SerializeField] float flipTime;

    Vector3 startFlipDirection;

    // Start is called before the first frame update
    void Start()
    {
        startFlipDirection = transform.rotation.eulerAngles;
        StartCoroutine(flipSelf());
    }

    //infinitely flip self
    private IEnumerator flipSelf()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            //flip down
            LeanTween.rotate(gameObject, flipDirection, flipTime).setEaseInOutSine();

            yield return new WaitForSeconds(flipTime + timeHeldDown);

            //Flip up
            LeanTween.rotateX(gameObject, startFlipDirection.x, flipTime).setEaseInOutSine();

            yield return new WaitForSeconds(flipTime + timeHeldUp);
        }
    }
}
