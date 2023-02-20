using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    //This is to be put ontop of ui buttons to allow for tweening and handling specific inputs

    [Header("Hover")]
    [SerializeField] float buttonHoverResizeMultipler;
    [SerializeField] float hoverTransitionTime;
    [SerializeField] AudioSource defaultButtonSelect;

    [Header("Clicked")]
    [SerializeField] float buttonClickedResizeMultipler;
    [SerializeField] float clickedTransitionTime;
    [SerializeField] GameObject buttonClickPrefab;

    [Header("Optional Settings")]
    //If true an orange border will follow the ui icon hovered over
    [SerializeField] bool canHaveSelectionBorder;
    [SerializeField] GameObject selectionBorder;
    [SerializeField] Vector3 offsetBorderPos = new Vector3(0,0,0);
    [SerializeField] float selectionBorderTransitionSpeed;

    Image myImage;
    Vector3 initSize;
    Vector3 selectionBorderInitSize;

    private void Awake()
    {
        initSize = transform.localScale;

        if (canHaveSelectionBorder)
        {
            selectionBorderInitSize = selectionBorder.transform.localScale;
        }
    }

    public void HoverSelectEnter()
    {
        LeanTween.scale(gameObject, initSize * buttonHoverResizeMultipler, hoverTransitionTime).setEaseOutSine();

        if (canHaveSelectionBorder)
        {
            //LeanTween.moveLocal(selectionBorder, gameObject.transform.localPosition + offsetBorderPos, selectionBorderTransitionSpeed).setEaseInOutSine();
            LeanTween.move(selectionBorder, gameObject.transform.position + offsetBorderPos, selectionBorderTransitionSpeed).setEaseInOutSine();
        }

        defaultButtonSelect.Play();
    }

    public void HoverSelectExit()
    {
        LeanTween.scale(gameObject, initSize, hoverTransitionTime).setEaseInSine(); 
        LeanTween.scale(selectionBorder, selectionBorderInitSize, hoverTransitionTime).setEaseInSine(); 
    }

    public void buttonClicked()
    {
        Instantiate(buttonClickPrefab, transform.position, Quaternion.identity);
        StartCoroutine(buttonClickedEnum());
    }

    public IEnumerator buttonClickedEnum()
    {
        //Main Image
        LeanTween.scale(gameObject, initSize * buttonClickedResizeMultipler, clickedTransitionTime).setEaseInOutSine();

        yield return new WaitForSeconds(clickedTransitionTime);
        //Main Image
        LeanTween.scale(gameObject, initSize, clickedTransitionTime).setEaseInOutSine();
    }
}
