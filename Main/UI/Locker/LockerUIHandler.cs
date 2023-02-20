using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.UI;

public class LockerUIHandler : MonoBehaviour
{
    [Header("Main ingame UI Elements")]

    [Header("Equipment")]
    [SerializeField] GameObject equippedObjs;
    [SerializeField] GameObject equippedObjsFirstButton;
    [SerializeField] Transform equippedObjsOnscreenPos;
    [SerializeField] float equipmentLoadTransitionTime;

    [Header("Outfits")]
    [SerializeField] GameObject outfits;
    [SerializeField] GameObject outfitsFirstButton;
    [SerializeField] GameObject outfitsLeftPanel;
    [SerializeField] Transform outfitsLeftPanelOnScreenPos;
    [SerializeField] GameObject outfitsRightPanel;
    [SerializeField] Transform outfitsRightPanelOnScreenPos;
    [SerializeField] float outfitLoadTransitionTime;

    [Header("PogoSticks")]
    [SerializeField] GameObject pogoSticks;
    [SerializeField] GameObject pogoSticksFirstButton;
    [SerializeField] GameObject pogoSticksLeftPanel;
    [SerializeField] Transform pogoSticksLeftPanelOnScreenPos;
    [SerializeField] GameObject pogoSticksRightPanel;
    [SerializeField] Transform pogoSticksRightPanelOnScreenPos;
    [SerializeField] float pogoSticksLoadTransitionTime;

    [Header("Selection Border")]
    [SerializeField] GameObject selectionBorder;
    [SerializeField] Sprite verticalSelectionBorder;
    [SerializeField] Sprite squareSelectionBorder;
    [SerializeField] Sprite horizontalSelectionBorder;
    [SerializeField] Vector3 selectionBorderOutfitScale;
    [SerializeField] Vector3 selectionBorderBackScale;
    [SerializeField] Vector3 offsetBorderPos;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera mainCam;
    [SerializeField] CinemachineVirtualCamera outfitCam;

    Vector3 selectionBorderStartSize;
    Image selectionBorderImg;
    Sprite prevBorderSelectedSprite;
    Vector3 prevBorderSelectedScale;

    //Equipment Pos
    Vector3 equipmentStartPos;

    //Outfit pos
    Vector3 outfitsLeftPanelStartPos;
    Vector3 outfitsRightPanelStartPos;

    //PogoStick pos
    Vector3 pogoSticksLeftPanelStartPos;
    Vector3 pogoSticksRightPanelStartPos;

    private void Awake()
    {
        //Border
        selectionBorderStartSize = selectionBorder.transform.localScale;
        selectionBorderImg = selectionBorder.GetComponent<Image>();
        prevBorderSelectedSprite = squareSelectionBorder;
        prevBorderSelectedScale = selectionBorderStartSize;

        //Equipment
        equipmentStartPos = equippedObjs.transform.position;

        //Outfits
        outfitsLeftPanelStartPos = outfitsLeftPanel.transform.position;
        outfitsRightPanelStartPos = outfitsRightPanel.transform.position;

        //PogoSticks
        pogoSticksLeftPanelStartPos = pogoSticksLeftPanel.transform.position;
        pogoSticksRightPanelStartPos = pogoSticksRightPanel.transform.position;

        //Start off level by loading in equipment ui
        loadEquippedUI();
    }

    //Equipped UI Logic
    public void loadEquippedUI()
    {
        //minimiseAll();
        StartCoroutine(tweenEquipablesIn());

        //Load in equipment
        equippedObjs.SetActive(true);

        //square selection border
        selectionBorder.transform.localScale = selectionBorderStartSize;
        selectionBorderImg.sprite = squareSelectionBorder;

        //Set selected Obj
        EventSystem.current.SetSelectedGameObject(equippedObjsFirstButton);

        //Change cam
        outfitCam.m_Priority = 0;
        mainCam.m_Priority = 10;

        //Store as previously selected, makes it easy for back button to change when deselecting it

        prevBorderSelectedScale = selectionBorderStartSize;
        prevBorderSelectedSprite = squareSelectionBorder;
    }

    private IEnumerator tweenEquipablesIn()
    {
        LeanTween.move(equippedObjs, equippedObjsOnscreenPos.transform.position, equipmentLoadTransitionTime).setEaseInOutSine();
        yield return new WaitForSeconds(equipmentLoadTransitionTime);

        //square selection border
        selectionBorder.transform.localScale = selectionBorderStartSize;
        selectionBorderImg.sprite = squareSelectionBorder;
    }

    private IEnumerator tweenEquipablesOut()
    {
        LeanTween.move(equippedObjs, equipmentStartPos, equipmentLoadTransitionTime).setEaseInOutSine();
        yield return new WaitForSeconds(equipmentLoadTransitionTime);
        equippedObjs.SetActive(false);
    }
    //End of equippable logic



    //OUTFITS TWEENING LOGIC
    public void loadOutfitsUI()
    {
        //minimiseAll();
        //Load Equipment out
        StartCoroutine(tweenEquipablesOut());

        //Load in outfits UI

        StartCoroutine(tweenInOutfitsUI());    

        //selection border
        selectionBorder.transform.localScale = selectionBorderOutfitScale;
        selectionBorderImg.sprite = verticalSelectionBorder;

        //Set selected Obj
        EventSystem.current.SetSelectedGameObject(outfitsFirstButton);

        //Change cam
        outfitCam.m_Priority = 10;
        mainCam.m_Priority = 0;

        //Store as previously selected, makes it easy for back button to change when deselecting it
        prevBorderSelectedScale = selectionBorderOutfitScale;
        prevBorderSelectedSprite = verticalSelectionBorder;

    }

    public void minimiseOutfitsUI()
    {
        StartCoroutine(tweenOutOutfitsUI());
    }

    private IEnumerator tweenInOutfitsUI()
    {
        outfits.SetActive(true);
        //Move left side in
        LeanTween.move(outfitsLeftPanel, outfitsLeftPanelOnScreenPos, outfitLoadTransitionTime).setEaseInOutSine();
        //Move right side in
        LeanTween.move(outfitsRightPanel, outfitsRightPanelOnScreenPos, outfitLoadTransitionTime).setEaseInOutSine();
        yield return new WaitForSeconds(outfitLoadTransitionTime);
        selectionBorder.transform.localScale = selectionBorderOutfitScale;
        //Set selected Obj
        EventSystem.current.SetSelectedGameObject(outfitsFirstButton);
    }

    private IEnumerator tweenOutOutfitsUI()
    {
        //Move left side out
        LeanTween.move(outfitsLeftPanel, outfitsLeftPanelStartPos, outfitLoadTransitionTime).setEaseInOutSine();        
        //Move right side out
        LeanTween.move(outfitsRightPanel, outfitsRightPanelStartPos, outfitLoadTransitionTime).setEaseInOutSine();
        yield return new WaitForSeconds(outfitLoadTransitionTime);
        outfits.SetActive(false);
    }
    //OUTFITS END



    //Pogostick Begin

    public void loadPogoSticksUI()
    {
        //minimiseAll();
        //Load Equipment out
        StartCoroutine(tweenEquipablesOut());

        //Load in outfits UI

        StartCoroutine(tweenInPogoSticksUI());

        //selection border
        selectionBorder.transform.localScale = selectionBorderOutfitScale;
        selectionBorderImg.sprite = verticalSelectionBorder;

        //Set selected Obj
        EventSystem.current.SetSelectedGameObject(pogoSticksFirstButton);

        //Change cam
        outfitCam.m_Priority = 10;
        mainCam.m_Priority = 0;

        //Store as previously selected, makes it easy for back button to change when deselecting it
        prevBorderSelectedScale = selectionBorderOutfitScale;
        prevBorderSelectedSprite = verticalSelectionBorder;

    }

    public void minimisePogoSticksUI()
    {
        StartCoroutine(tweenOutPogoSticksUI());
    }

    private IEnumerator tweenInPogoSticksUI()
    {
        pogoSticks.SetActive(true);
        //Move left side in
        LeanTween.move(pogoSticksLeftPanel, pogoSticksLeftPanelOnScreenPos, pogoSticksLoadTransitionTime).setEaseInOutSine();
        //Move right side in
        LeanTween.move(pogoSticksRightPanel, pogoSticksRightPanelOnScreenPos, pogoSticksLoadTransitionTime).setEaseInOutSine();
        yield return new WaitForSeconds(pogoSticksLoadTransitionTime);
    }

    private IEnumerator tweenOutPogoSticksUI()
    {
        //Move left side out
        LeanTween.move(pogoSticksLeftPanel, pogoSticksLeftPanelStartPos, pogoSticksLoadTransitionTime).setEaseInOutSine();
        //Move right side out
        LeanTween.move(pogoSticksRightPanel, pogoSticksRightPanelStartPos, pogoSticksLoadTransitionTime).setEaseInOutSine();
        yield return new WaitForSeconds(pogoSticksLoadTransitionTime);
        pogoSticks.SetActive(false);
    }

    //Pogostick End


        //SELECTION BORDER NONSENSE
    public void BackButtSelected()
    {
        selectionBorder.transform.localScale = selectionBorderBackScale;
        selectionBorderImg.sprite = horizontalSelectionBorder;
    }

    public void BackButtDeselected()
    {
        selectionBorder.transform.localScale = prevBorderSelectedScale;
        selectionBorderImg.sprite = prevBorderSelectedSprite;
    }

    public void lockerButtonPressed()
    {
        StartCoroutine(buttonClickedEnum());
    }

    public void OutfitHoverSelectEnter(GameObject _gameObject)
    {
        LeanTween.scale(selectionBorder, selectionBorderOutfitScale * 1.1f, 0.1f).setEaseOutSine();

        LeanTween.move(selectionBorder, _gameObject.transform.position + offsetBorderPos, 0.1f).setEaseInOutSine();

    }

    public void OutfitHoverSelectExit()
    {
        //0.1 is transition time
        LeanTween.scale(selectionBorder, selectionBorderOutfitScale, 0.1f).setEaseInSine();
    }

    public IEnumerator buttonClickedEnum()
    {

        //Border 1.1 is size increase on click, 0.1 is transition time
        LeanTween.scale(selectionBorder, selectionBorderOutfitScale * 1.2f, 0.1f).setEaseInOutSine();

        yield return new WaitForSeconds(0.1f);
        //Main Image
        LeanTween.scale(selectionBorder, selectionBorderOutfitScale, 0.1f).setEaseInOutSine();
    }

    private void minimiseAll()
    {
        //Minimise all Ui
        equippedObjs.SetActive(false);
        outfits.SetActive(false);

        //Clear selected Obj
        EventSystem.current.SetSelectedGameObject(null);
    }
}
