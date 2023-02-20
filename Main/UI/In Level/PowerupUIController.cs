using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUIController : MonoBehaviour
{
    [SerializeField] Transform offscreenPos;
    [SerializeField] Transform onscreenPos;
    [SerializeField] Image itemImg;
    [SerializeField] GameObject itemBgd;
    [SerializeField] GameObject iconCover;
    [SerializeField] Transform iconCoverOffscreenPos;
    [SerializeField] float transitionSpeed;

    Vector3 startSize;
    Vector3 iconCoverOnscreenPos;

    // Start is called before the first frame update
    void Awake()
    {
        startSize = itemBgd.transform.localScale;
        itemBgd.transform.localScale = Vector3.zero;
        iconCoverOnscreenPos = Vector3.zero;
    }

    public void playUIVFX()
    {
        StartCoroutine(tweenInUIVFX());
    }

    private IEnumerator tweenInUIVFX()
    {
        itemBgd.transform.position = offscreenPos.position;
       // iconCover.transform.localPosition = iconCoverOnscreenPos;

        //bounce in small
        LeanTween.scale(itemBgd, startSize / 4, 0.001f);
        LeanTween.move(itemBgd, onscreenPos, 0.3f).setEaseInOutBounce();

        yield return new WaitForSeconds(0.3f);

        //grow
        LeanTween.scale(itemBgd, startSize, 0.5f).setEaseInExpo();

        yield return new WaitForSeconds(0.5f);

        LeanTween.moveX(iconCover, iconCover.transform.position.x - 10, 0f);
        LeanTween.moveY(iconCover, iconCover.transform.position.y - 10, 0f);

        LeanTween.scale(iconCover, new Vector3(1f, 1f, 1f), 0.1f).setEaseInOutSine().setLoopPingPong(4);
        LeanTween.moveX(iconCover, iconCover.transform.position.x + 20, 0.09f).setEaseInOutSine().setLoopPingPong(4);
        LeanTween.moveY(iconCover, iconCover.transform.position.y + 20, 0.11f).setEaseInOutSine().setLoopPingPong(4);

        yield return new WaitForSeconds(0.4f);

        //remove cover
        LeanTween.move(iconCover, iconCover.transform.position + (Vector3.left * 250f), 1.3f).setEaseInOutBounce();//.setEaseShake();
    }

    public void playUIOutVFX()
    {
        StartCoroutine(tweenOutUIVFX());
    }

    private IEnumerator tweenOutUIVFX()
    {
        //bounce in small
        LeanTween.scale(itemBgd, startSize / 4, 0.1f);
        LeanTween.move(itemBgd, offscreenPos, 0.3f).setEaseInOutBounce();

        yield return new WaitForSeconds(0.3f);

        //reset scale
        LeanTween.scale(itemBgd, startSize, 0.0001f);
    }

    public void SetItemImage(Sprite _itemImage)
    {
        itemImg.sprite = _itemImage;
    }
}
