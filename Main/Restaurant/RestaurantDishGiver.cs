using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RestaurantDishGiver : MonoBehaviour
{
    [SerializeField] CreatePlayerTeams createPlayerTeams;
    [SerializeField] RestaurantTableHandler tableHandler;
    [SerializeField] GameObject tableHighlighter;
    [SerializeField] List<GameObject> dishesPrefabs;
    [SerializeField] GameObject platePrefab;
    [SerializeField] GameObject poofVFXPrefab;
    [SerializeField] GameObject poofSFXPrefab;
    [SerializeField] public float timeToTakeDish;
    [SerializeField] float timeDiminshSpeed;

    Vector3 tableHighlighterInitScale;

    int currentTableIndex;
    public float dishGivingTime;
    bool platformIsInTrigger;

    private void Start()
    {
        tableHighlighterInitScale = tableHighlighter.transform.localScale;
        tableHighlighter.transform.localScale = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Platter") && other.GetComponent<PhotonView>().IsMine)
        {
            //dishGivingTime = 0;
            /*
            //check that don't already have a dish
            PlatterHolder platterHolder = other.GetComponent<PlatterHolder>();
            if (platterHolder.isHoldingDishes) { return; }

            //Find table not in use
            int availableIndex = tableHandler.GetAvailableTableIndex();
            currentTableIndex = availableIndex;

            tableHighlighter.transform.position = tableHandler.GetTablePosition(availableIndex);

            //assign self to table
            PhotonView tableHandlerView = tableHandler.gameObject.GetComponent<PhotonView>();
            tableHandlerView.RPC("SetTableUnavailable", RpcTarget.All, availableIndex);

            List<GameObject> newDishSet = new List<GameObject>();

            //aquire items
            for(int i = 0; i < 4; i++)
            {
                //Calc dish and position
                int randNum = Random.Range(0, dishesPrefabs.Count);
                Vector3 spawnPos = other.transform.GetChild(i).position;
                Vector3 spawnPosYOffset = new Vector3(spawnPos.x, spawnPos.y + 0.1f, spawnPos.z);

                //Spawn dish & plate
                GameObject newDish = PhotonNetwork.Instantiate(dishesPrefabs[randNum].name, spawnPosYOffset, Quaternion.identity);
                PhotonNetwork.Instantiate(platePrefab.name, spawnPos, Quaternion.identity);
                newDishSet.Add(newDish);
            }

            platterHolder.currentDishes = newDishSet;
            platterHolder.isHoldingDishes = true;
            */
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Platter") && other.GetComponent<PhotonView>().IsMine)
        {
            //check that don't already have a dish
            PlatterHolder platterHolder = other.GetComponent<PlatterHolder>();
            if (platterHolder.isHoldingDishes) { return; }

            platformIsInTrigger = true;

            if (dishGivingTime < timeToTakeDish) { return; }

            //Find table not in use
            int availableIndex = tableHandler.GetAvailableTableIndex();
            currentTableIndex = availableIndex;

            tableHighlighter.transform.position = tableHandler.GetTablePosition(availableIndex);

            //assign self to table
            PhotonView tableHandlerView = tableHandler.gameObject.GetComponent<PhotonView>();
            tableHandlerView.RPC("SetTableUnavailable", RpcTarget.All, availableIndex);

            List<GameObject> newDishSet = new List<GameObject>();

            //aquire items
            for (int i = 0; i < 4; i++)
            {
                //Calc dish and position
                int randNum = Random.Range(0, dishesPrefabs.Count);
                Vector3 spawnPos = other.transform.GetChild(i).position;
                Vector3 spawnPosYOffset = new Vector3(spawnPos.x, spawnPos.y + 0.5f, spawnPos.z);
                Vector3 spawnPosYOffsetSmall = new Vector3(spawnPos.x, spawnPos.y + 0.15f, spawnPos.z);

                //Spawn dish & plate
                GameObject newDish = PhotonNetwork.Instantiate(dishesPrefabs[randNum].name, spawnPosYOffset, Quaternion.identity);
                newDish.GetComponent<RestaurantScoreGiver>().setTeamIndex(createPlayerTeams.GetMyPlayerTeamIndex()); // Give dish player's team index 
                newDish.GetComponent<RestaurantScoreGiver>().tableIndex = tableHandler.GetAvailableTableIndex();
                newDish.GetComponent<RestaurantScoreGiver>().dishTablePositionIndex = i;
                newDish.GetComponent<RestaurantScoreGiver>().activeTableHandler = tableHandler;

                GameObject newPlate = PhotonNetwork.Instantiate(platePrefab.name, spawnPosYOffsetSmall, Quaternion.identity);
                newDish.GetComponent<RestaurantScoreGiver>().myPlate = newPlate;

               PhotonNetwork.Instantiate(poofVFXPrefab.name, spawnPosYOffsetSmall, Quaternion.identity);
                PhotonNetwork.Instantiate(poofSFXPrefab.name, spawnPosYOffsetSmall, Quaternion.identity);
                newDishSet.Add(newDish);
            }

            platterHolder.currentDishes = newDishSet;
            platterHolder.isHoldingDishes = true;
            dishGivingTime = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Platter") && other.GetComponent<PhotonView>().IsMine)
        {
            platformIsInTrigger = false;
        }
    }

    private void Update()
    {
        if (!platformIsInTrigger && dishGivingTime > 0)
        {
            dishGivingTime -= Time.deltaTime * timeDiminshSpeed;
        }
        else
        {
            dishGivingTime += Time.deltaTime;
        }
    }

    public void setActiveTableAvailable()
    {
        PhotonView tableHandlerView = tableHandler.gameObject.GetComponent<PhotonView>();
        tableHandlerView.RPC("SetTableAvailable", RpcTarget.All, currentTableIndex);
    }

    public void EnableTableHighlighter()
    {
        tableHighlighter.SetActive(true);
        LeanTween.scale(tableHighlighter, tableHighlighterInitScale, 0.5f).setEaseInOutSine();
    }

    /*
    private IEnumerator enableTableHighlight()
    {
        tableHighlighter.SetActive(true);
        LeanTween.scale(tableHighlighter, tableHighlighterInitScale, 0.3f);
        yield return null;
    }
    */
    public void DisableTableHighlighter()
    {
        StartCoroutine(disableTableHighlight());
    }

    private IEnumerator disableTableHighlight()
    {
        LeanTween.scale(tableHighlighter, Vector3.zero, 0.5f).setEaseInOutSine();
        yield return new WaitForSeconds(0.6f);
        tableHighlighter.SetActive(false);
    }

    public void setPlatterIsInTriggerFalse()
    {
        platformIsInTrigger = false;
    }

}
