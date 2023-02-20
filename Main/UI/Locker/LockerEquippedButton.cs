using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerEquippedButton : MonoBehaviour
{
    [SerializeField] Image myImg;
    [SerializeField] Image myRarity;

    //Set equip icon to match selection
    public void UpdateEquippepdIcon(Sprite _newImg)
    {
        myImg.sprite = _newImg;
    }

    public void UpdateEquippedRarity(Sprite _newRarity)
    {
        myRarity.sprite = _newRarity;
    }
}
