using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockerDescriptionChanger : MonoBehaviour
{
    [Header("Locker Objs")]
    [SerializeField] GameObject lockerDescription;
    [SerializeField] TextMeshProUGUI itemNameUI;
    [SerializeField] TextMeshProUGUI itemDescriptionUI;
    [SerializeField] TextMeshProUGUI itemTypeUI;
    [SerializeField] TextMeshProUGUI itemRarityUI;
    [SerializeField] Color itemRarityColor;
    [SerializeField] Sprite bannerRarityDescImg;

    [Header("Item Details")]
    [SerializeField] string itemName;
    [TextArea]
    [SerializeField] string itemDescription;
    [SerializeField] itemTypes itemType;
    [SerializeField] rarities rarity;

    Image lockerImage;
    string rarityText;
    string itemTypeText;

    private void Awake()
    {
        lockerImage = lockerDescription.GetComponent<Image>();

        //Set rarity text
        switch (rarity)
        {
            case rarities.GigaRare:
                rarityText = "Giga-Rare";
                break;
            case rarities.Epic:
                rarityText = "Epic";
                break;
            case rarities.Rare:
                rarityText = "Rare";
                break;
            case rarities.Uncommon:
                rarityText = "Uncommon";
                break;
            case rarities.Common:
                rarityText = "Common";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);

        }

        //Set item type text
        switch (itemType)
        {
            case itemTypes.outfit:
                itemTypeText = "Outfit";
                break;
            case itemTypes.pogostick:
                itemTypeText = "Pogo Stick";
                break;
            case itemTypes.backpack:
                itemTypeText = "Backpack";
                break;
            case itemTypes.scarf:
                itemTypeText = "Scarf";
                break;
        }
    }

    //changes bgd and all text elements inside
    public void changeDescSprite()
    {
        lockerImage.sprite = bannerRarityDescImg;
        itemNameUI.SetText(itemName);
        itemDescriptionUI.SetText(itemDescription);
        itemTypeUI.SetText(itemTypeText);
        itemRarityUI.SetText(rarityText);
        itemRarityUI.color = itemRarityColor;
    }

    private enum rarities
    {
        GigaRare,
        Epic,
        Rare,
        Uncommon,
        Common
    }

    private enum itemTypes
    {
        outfit,
        pogostick,
        backpack,
        scarf
    }
}
