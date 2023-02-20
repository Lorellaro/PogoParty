using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TornadoMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float yOffset;
    [SerializeField] LayerMask raycastIgnoreLayers;

    [SerializeField] List<Vector3> waypoints;

    public int randomSeed;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //get all positions of children in disaster waypoints
        Transform disasterWaypointsParent = GameObject.FindGameObjectWithTag("DisasterWaypoints").transform;

        PhotonView photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)//(PhotonNetwork.IsMasterClient)
        {
            //generate seed
            //Can be dependant on duration of match
            Random.InitState(System.DateTime.Now.Millisecond);
            randomSeed = Mathf.RoundToInt(Random.Range(0, 9999999999999999));//1111111111111
            GetComponent<PhotonView>().RPC("SetRandomSeed", RpcTarget.AllBuffered, randomSeed);

        }

        int children = disasterWaypointsParent.childCount;
        for (int i = 0; i < children; ++i)
        {
            waypoints.Add(disasterWaypointsParent.GetChild(i).position);
        }

        KillSelf(60f);

        StartCoroutine(moveToNextWaypoint());
    }

    [PunRPC]
    public void SetRandomSeed(int _randomSeed)
    {
        randomSeed = _randomSeed;
    }

    //Recursively move to a random waypoint
    IEnumerator moveToNextWaypoint()
    {
        Random.InitState(randomSeed);
        int randomWaypoint = Random.Range(0, waypoints.Count);

        Vector3 currentWaypoint = waypoints[randomWaypoint];
        waypoints.Remove(waypoints[randomWaypoint]);

        while (Vector3.Distance(transform.position, currentWaypoint) > 1)
        {
            Vector3 target = (currentWaypoint - transform.position).normalized;


            target = new Vector3(target.x, target.y, target.z);

            rb.velocity = target * moveSpeed * Time.fixedDeltaTime;
            yield return null;
        }

        StartCoroutine(moveToNextWaypoint());
    }

    private void KillSelf(float _time)
    {
        Destroy(gameObject, _time);
    }
}
