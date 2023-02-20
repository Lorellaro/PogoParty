using System;
using System.Collections.Generic;
using System.ComponentModel;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Main.UI.Shop
{
    public class Shop : MonoBehaviour
    {
        public static Shop Instance;
        
        [Header("Player References")]
        [Tooltip("Reference to the player cube skin mesh")]
        [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
        [Tooltip("Reference to the backpack slot located on spine 2 of the player rig")]
        [SerializeField] private MeshFilter backpackMeshFilter;
        [Tooltip("Reference to the backpack slot located on spine 2 of the player rig")]
        [SerializeField] private MeshRenderer backpackMeshRenderer;
        [Tooltip("Reference to the main body of the pogo stick, should be the last child on the pogo stick")]
        [SerializeField] private MeshFilter pogoStickMainMeshFilter;
        [Tooltip("Reference to the main body of the pogo stick, should be the last child on the pogo stick")]
        [SerializeField] private MeshRenderer pogoStickMainMeshRenderer;
        [Tooltip("Reference to the pole part of the pogo stick, should be the child of the first bone of the pogo stick")]
        [SerializeField] private MeshFilter pogoStickPoleMeshFilter;
        [Tooltip("Reference to the pole part of the pogo stick, should be the child of the first bone of the pogo stick")]
        [SerializeField] private MeshRenderer pogoStickPoleMeshRenderer;

        private Dictionary<int, GameObject> currentItems;
        
        [Header("Item References")]
        //The arrays must match up with each other but don't need to be in the same order as the cards.
        //For example if you have an item ID of 1 and the item is a pogo stick if the item id is in array pos 0 then
        //the mesh for the pogo stick must be in array pos 0
        [Tooltip("The item ID that is on the item cards")]
        [SerializeField] private int[] itemIDs;
        [Tooltip("The item mesh that the cards represent")]
        [SerializeField] private GameObject[] itemMeshes;
        
        [Header("Main Shop Text")]
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private int currentBalance;

        [Header("Item Preview Text")] 
        [SerializeField] private TextMeshProUGUI previewBalanceText;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemRarityText;
        [SerializeField] private TextMeshProUGUI itemCostText;
        [SerializeField] private Image currencyIcon;

        [Header("Parent Game Objects")] 
        [SerializeField] private GameObject itemParent;
        [SerializeField] private GameObject itemPreviewParent;
        [SerializeField] private GameObject playerArea;

        // public GameObject test;

        private void Awake()
        {
            Instance = this;
            currentItems = new Dictionary<int, GameObject>();
            for (int i = 0; i < itemMeshes.Length; i++)
            {
                currentItems.Add(itemIDs[i], itemMeshes[i]);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            balanceText.text = currentBalance.ToString();
            // itemParent.SetActive(true);
            // itemPreviewParent.SetActive(false);
            // playerArea.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public enum ItemRarities
        {
            Common = 0,
            UnCommon = 1,
            Rare = 2,
            Epic = 3,
            Legendary = 4
        }

        public enum ItemTypes
        {
            Head,
            Top,
            Bottom,
            Backpack,
            PogoStick,
            FullBody
        }
        
        // new Color(138,43,226)


        public Sprite[] itemRarityBackgrounds;

        private void OnDestroy()
        {
            Instance = null;
        }

        public void ItemHasBeenClicked(Item item)
        {
            int itemID = item.GetItemID();
            int itemCost = item.GetItemCost();
            ItemTypes itemType = item.GetItemType();
            ItemRarities itemRarity = item.GetItemRarity();
            string itemName = item.GetItemName();
            Sprite currencyIcon = item.GetCurrencyIcon();
            
            if(!CheckBalance(itemCost)) return;
            switch (itemType)
            {
                case ItemTypes.Head:
                    Debug.Log("Head");
                    break;
                case ItemTypes.Top:
                    Debug.Log("Top");
                    break;
                case ItemTypes.Bottom:
                    Debug.Log("Bottom");
                    break;
                case ItemTypes.Backpack:
                    backpackMeshFilter.sharedMesh = currentItems[itemID].GetComponent<MeshFilter>().sharedMesh;
                    backpackMeshRenderer.sharedMaterials = currentItems[itemID].GetComponent<MeshRenderer>().sharedMaterials;
                    break;
                case ItemTypes.PogoStick:
                    pogoStickMainMeshFilter.sharedMesh = currentItems[itemID].transform.GetChild(5)
                        .GetComponent<MeshFilter>().sharedMesh;
                    pogoStickMainMeshRenderer.sharedMaterials = currentItems[itemID].transform.GetChild(5)
                        .GetComponent<MeshRenderer>().sharedMaterials;
                    pogoStickPoleMeshFilter.sharedMesh = currentItems[itemID].transform.GetChild(0).GetChild(0)
                        .GetComponent<MeshFilter>().sharedMesh;
                    pogoStickPoleMeshRenderer.sharedMaterials = currentItems[itemID].transform.GetChild(0).GetChild(0)
                        .GetComponent<MeshRenderer>().sharedMaterials;
                    break;
                case ItemTypes.FullBody:
                    playerMeshRenderer.sharedMesh = currentItems[itemID].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    playerMeshRenderer.sharedMaterials = currentItems[itemID].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
            }
        }

        private bool CheckBalance(int itemPrice)
        {
            if (currentBalance >= itemPrice) return true;
            return false;
        }
        
        
    }

    
    
}
