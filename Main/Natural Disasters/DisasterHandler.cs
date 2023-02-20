using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisasterHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> disasterObjs;
    [SerializeField] DisasterWarningUI disasterWarningUI;
    [SerializeField] [Range(0, 100)] float disasterChance;
    [SerializeField] Vector2 minMaxSpawnTime;
    [SerializeField] float spawnRadius;
    [SerializeField] float warningUIFinishTime = 7f;
    [SerializeField] float warningTime = 3f;
    [SerializeField] float YSpawnPos;

    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();

        Random.InitState(Mathf.RoundToInt(GetComponent<RandomSeedGenerator>().randomSeed));
        int randNum = Random.Range(0, 100);

        if(randNum < disasterChance)
        {
            float randomTime = Random.Range(minMaxSpawnTime.x, minMaxSpawnTime.y);
            StartCoroutine(startDisasterAfterTime(randomTime));
        }
    }

    private IEnumerator startDisasterAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //Start warning
        disasterWarningUI.enableWarningUI();
        myAudioSource.Play();

        yield return new WaitForSeconds(warningTime);

        //Start disaster
        int randomDisasterObjIndex = Random.Range(0, disasterObjs.Count);

        Vector3 randomPosAroundCircleDiameter = Random.onUnitSphere * spawnRadius;

        //var angle = Mathf.PI * 2;
        //Vector3 pos = new Vector3(Mathf.Cos(angle), YSpawnPos, Mathf.Sin(angle)) * spawnRadius;

        Vector3 randomSpawnPos = new Vector3(randomPosAroundCircleDiameter.x, YSpawnPos, randomPosAroundCircleDiameter.z);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(disasterObjs[randomDisasterObjIndex].name, randomSpawnPos, Quaternion.Euler(Vector3.zero));
        }

        yield return new WaitForSeconds(warningUIFinishTime - warningTime);

        disasterWarningUI.disableWarningUI();
        myAudioSource.Stop();
    }

    //Visualise spawn diameter
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
