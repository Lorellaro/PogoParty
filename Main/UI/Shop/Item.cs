using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Main.UI.Shop
{
    public class Item : MonoBehaviour
    {
        #region variables

        [Header("Item UI Variables")]
        [SerializeField] private TextMeshProUGUI itemTitle, itemValue;

        [SerializeField] private Image itemImage, currencyImage, itemBackgroundImage;

        [Header("Item Values")]
        [SerializeField] private int itemIDNumber;
        [SerializeField] private string itemName;
        [SerializeField] private int itemCost;
        [SerializeField] private Sprite itemSprite, currencySprite;
        [SerializeField] private Shop.ItemRarities itemRarity;
        [SerializeField] private Shop.ItemTypes itemType;

        
        #endregion

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // Debug.Log(itemRarity);
            // Debug.Log(Shop.Instance.itemRaritieColors[(int)itemRarity]);
            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateUI()
        {
            itemTitle.text = itemName;
            itemValue.text = itemCost.ToString();
            itemImage.sprite = itemSprite;
            currencyImage.sprite = currencySprite;
            itemBackgroundImage.sprite = Shop.Instance.itemRarityBackgrounds[(int)itemRarity];
        }

        public void ItemClicked()
        {
            Shop.Instance.ItemHasBeenClicked(this);
        }

        public int GetItemID()
        {
            return itemIDNumber;
        }

        public string GetItemName()
        {
            return itemName;
        }

        public int GetItemCost()
        {
            return itemCost;
        }

        public Sprite GetCurrencyIcon()
        {
            return currencySprite;
        }

        public Shop.ItemRarities GetItemRarity()
        {
            return itemRarity;
        }

        public Shop.ItemTypes GetItemType()
        {
            return itemType;
        }
        
        
        
        
    }
}
