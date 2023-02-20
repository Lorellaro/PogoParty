using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlatterHolder : MonoBehaviour
{
    public GameObject myPlayer;
    public bool isHoldingDishes;
    public GameObject platterCollectorZone;
    [SerializeField] public List<GameObject> currentDishes;
    [SerializeField] float maxFoodHoldRadius;
    [SerializeField] float timeAfterDishesFallOffFailure = 2f;

    Coroutine setIsHoldingDishesCoroutine;
    RestaurantDishGiver restaurantDishGiver;
    MeshRenderer platterCollecterZoneMeshRenderer;
    PhotonView photonView;

    [SerializeField] private Material[] teamColours;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine) { GetComponent<Rigidbody>().isKinematic = true; return;}
        StartCoroutine(delme());
        restaurantDishGiver = platterCollectorZone.GetComponent<RestaurantDishGiver>();
        platterCollecterZoneMeshRenderer = platterCollectorZone.GetComponent<MeshRenderer>();
        // StartCoroutine(ChangePlatterColour());
    }

    private void Update()
    {
        if (!photonView.IsMine) { return; }

        if (!isHoldingDishes)
        {
            //platterCollecterZoneMeshRenderer.enabled = true;
            //platterCollectorZone.transform.GetChild(0).gameObject.SetActive(true);
            platterCollectorZone.GetComponent<EmissivePulseEffect>().fadeMaterialIn();
            restaurantDishGiver.DisableTableHighlighter();
        }
        else
        {
            //platterCollecterZoneMeshRenderer.enabled = false;
            //platterCollectorZone.transform.GetChild(0).gameObject.SetActive(false);
            platterCollectorZone.GetComponent<EmissivePulseEffect>().fadeMaterialOut();
            restaurantDishGiver.setPlatterIsInTriggerFalse();
            restaurantDishGiver.EnableTableHighlighter();
        }

        if (currentDishes == null) { return; }

        int dishesOutOfRange = 0;
        //check if there are any dishes within a certain radius of the platter
        for (int i = 0; i < currentDishes.Count; i++)
        {
            if (Vector3.Distance(transform.position, currentDishes[i].transform.position) > maxFoodHoldRadius)
            {
                dishesOutOfRange++;
            }
        }

        if(dishesOutOfRange == currentDishes.Count)
        {
            //no dish in range
            if(setIsHoldingDishesCoroutine != null) { return; }
            setIsHoldingDishesCoroutine = StartCoroutine(setIsHoldingDishesFalse());
        }
        else
        {
            //dish is now in range
            isHoldingDishes = true;

            if(setIsHoldingDishesCoroutine == null) { return; }

            StopCoroutine(setIsHoldingDishesCoroutine);
            setIsHoldingDishesCoroutine = null;
        }
    }

    private IEnumerator setIsHoldingDishesFalse()
    {
        yield return new WaitForSeconds(timeAfterDishesFallOffFailure);
        isHoldingDishes = false;
        StopCoroutine(setIsHoldingDishesCoroutine);
        setIsHoldingDishesCoroutine = null;

        if (restaurantDishGiver == null) { restaurantDishGiver = platterCollectorZone.GetComponent<RestaurantDishGiver>(); }

        restaurantDishGiver.setActiveTableAvailable();
    }

    //TEMP
    private IEnumerator delme()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(13f);
        GetComponent<Collider>().enabled = true;
    }

    public void CallChangePlatterColour(int photonID ,string teamName)
    {
        photonView.RPC("ChangePlatterColour", RpcTarget.All, photonID, teamName);
    }
    
    
    [PunRPC]
    private void ChangePlatterColour(int photonID, string teamName)
    {
        GameObject[] playerRoots = GameObject.FindGameObjectsWithTag("PlayerRoot");
        foreach (var playerRoot in playerRoots)
        {
            PhotonView pv = playerRoot.GetComponent<PhotonView>();
            if (pv.ViewID == photonID)
            {
                if (teamName == "T1")
                {
                    GetComponent<MeshRenderer>().sharedMaterial = teamColours[0];
                    return;
                }

                if (teamName == "T2")
                {
                    GetComponent<MeshRenderer>().sharedMaterial = teamColours[1];
                    return;
                }
            }
        }

    }
}
