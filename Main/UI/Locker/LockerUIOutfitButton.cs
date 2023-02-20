using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerUIOutfitButton : MonoBehaviour
{
    [SerializeField] GameObject myEquippedUIObj;
    [SerializeField] int equippedUIObjChildIndex = 2;

    private List<GameObject> allEquippedObjs = new List<GameObject>();

    private void Start()
    {
        //Create list of all other gameobjects
        int numOfChildrenInList = transform.parent.childCount;
        for(int i = 0; i < numOfChildrenInList; i++)
        {
            allEquippedObjs.Add(transform.parent.GetChild(i).gameObject);
        }
    }

    public void setEquipActive()
    {
        //Disable all other equipped Can find a more effecient way of doing this
        for(int i = 0; i < allEquippedObjs.Count; i++)
        {
            allEquippedObjs[i].transform.GetChild(equippedUIObjChildIndex).gameObject.SetActive(false);
        }
        myEquippedUIObj.SetActive(true);
    }


}
