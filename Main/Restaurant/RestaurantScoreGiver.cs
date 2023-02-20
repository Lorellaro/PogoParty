using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Main.GameHandlers.Teams;

public class RestaurantScoreGiver : MonoBehaviour
{
    [SerializeField] int pointsToGive;
    [SerializeField] GameObject caChingCanvas;
    [SerializeField] GameObject caChingSFX;
    [SerializeField] GameObject poofVFX;

    CreatePlayerTeams createPlayerTeamsScript;
    Teams teams;

    public int tableIndex;
    public int dishTablePositionIndex;
    public GameObject myPlate;
    public RestaurantTableHandler activeTableHandler;

    int teamIndex;
    Rigidbody rb;

    bool scored = false;

    private void Awake()
    {
        createPlayerTeamsScript = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<CreatePlayerTeams>();
        teams = createPlayerTeamsScript.GetPlayerTeams();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (scored) { return; }
        if (other.gameObject.CompareTag("Scorer"))
        {
            //addScore
            gameObject.GetComponent<PhotonView>().RPC("addScoreToTeam", RpcTarget.AllBuffered, teamIndex);
            //gameObject.GetComponent<BoxCollider>().enabled = false;
            scored = true;
            //playScoredVFX
            StartCoroutine(scoredVFX());

            //Destroy(gameObject, 3f);
        }
    }

    public void setTeamIndex(int _teamIndex)
    {
        teamIndex = _teamIndex;
    }

    private IEnumerator scoredVFX()
    {
        float randomTime = Random.Range(0.01f, 0.15f);
        yield return new WaitForSeconds(randomTime);
        Instantiate(caChingCanvas, transform.position, Quaternion.identity);
        Instantiate(caChingSFX, transform.position, Quaternion.identity);
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        //myPlate.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        myPlate.GetComponent<HoldRBtoRB>().enabled = false;
        GetComponent<HoldRBtoRB>().enabled = false;
        //Move plate and dish to them

        //poof at old position
        PhotonNetwork.Instantiate(poofVFX.name, transform.position, Quaternion.identity);
        PhotonNetwork.Instantiate(poofVFX.name, myPlate.transform.position, Quaternion.identity);
        //Move
        Vector3 newPos = activeTableHandler.GetTableDishPosition(tableIndex, dishTablePositionIndex);
        Vector3 newPosWithOffset = new Vector3(newPos.x, newPos.y + 0.1f, newPos.z);

        transform.position = newPosWithOffset;
        myPlate.transform.position = newPos;

        PhotonNetwork.Instantiate(poofVFX.name, transform.position, Quaternion.identity);
        //poof at new position
    }

    [PunRPC]
    public void addScoreToTeam(int _teamIndex)
    {
        teams.team[_teamIndex].score += pointsToGive;
        createPlayerTeamsScript.gameObject.GetComponent<PhotonView>().RPC("UpdateCashUI", RpcTarget.All);
    }
}
