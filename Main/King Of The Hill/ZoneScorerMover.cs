using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ZoneScorerMover : MonoBehaviourPunCallbacks
{
    [SerializeField] float moveSpeed;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;
    [SerializeField] List<Transform> destinations;
    [SerializeField] List<int> destinationIndexOrder;

    Transform currentDestination;
    Vector3 startPosition;

    bool startGame = true;

    float elapsedTime;

    int currentDestinationIndex;
    int prevDestinationIndex;
    int recursionIndex;
    public float randomSeed;


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonView photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)//(PhotonNetwork.IsMasterClient)
        {
            //generate seed
            //Can be dependant on duration of match
            //Random.InitState(System.DateTime.Now.Millisecond);
            //randomSeed = Mathf.RoundToInt(Random.Range(1111111111111, 9999999999999));
        }
        else
        {
            //this.photonView.RPC("SetRandomSeed", RpcTarget.AllBuffered, randomSeed);
            calculateDestinationIndexes();
        }
        //PhotonView photonView = PhotonView.Get(this);

        base.OnPlayerEnteredRoom(newPlayer);
    }

    [PunRPC]
    public void SetRandomSeed(float _randomSeed)
    {
        randomSeed = _randomSeed;
        calculateDestinationIndexes();
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)//(PhotonNetwork.IsMasterClient)
        {
            //generate seed
            //Can be dependant on duration of match
            Random.InitState(System.DateTime.Now.Millisecond);
            randomSeed = Mathf.Round(Random.Range(0, 9999999999999999));//1111111111111
            this.photonView.RPC("SetRandomSeed", RpcTarget.AllBuffered, randomSeed);

        }
        else
        {
            //photonView.RPC("SetRandomSeed", RpcTarget.AllBuffered, randomSeed);
        }

        calculateDestinationIndexes();

        startPosition = transform.position;
        currentDestination = transform;

        StartCoroutine(DestinationHandler());
    }

    private void calculateDestinationIndexes()
    {
        string seedString = randomSeed.ToString();//Convert to string
        char[] digits = seedString.ToCharArray();//Convert to array of characters

        int prevDigit = -1;

        for(int i = 0; i < digits.Length; i++)
        {
            if (!(prevDigit == digits[i] % destinations.Count))//Makes it impossible to have same index multiple times
            {
                int nextDigit = digits[i] % destinations.Count;
                destinationIndexOrder.Add(nextDigit);//create indexes % by how many destinations there are
                prevDigit = nextDigit;
            }
        }
    }

    //Recursively finds new random locations
    private IEnumerator DestinationHandler()
    {
        //Don't start moving until start game is true
        while (!startGame) { yield return null; }

        elapsedTime = 0;

        prevDestinationIndex = currentDestinationIndex;

        //set destination to random seed
        currentDestinationIndex = destinationIndexOrder[recursionIndex % destinationIndexOrder.Count];

        //Set new destination
        currentDestination = destinations[currentDestinationIndex];

        startPosition = transform.position;

        //time till it next moves
        float waitTime = Random.Range(minWaitTime, maxWaitTime);

        yield return new WaitForSeconds(waitTime);

        recursionIndex++;
        StartCoroutine(DestinationHandler());
    }

    private void Update()
    {
        //Move
        elapsedTime += Time.deltaTime;

        float percentComplete = elapsedTime / moveSpeed;

        transform.position = Vector3.Lerp(startPosition, currentDestination.position, Mathf.SmoothStep(0, 1, percentComplete));
    }
}
