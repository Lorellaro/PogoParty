using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectSpawner : MonoBehaviour
{
    [Tooltip("object that will be spawned repeatedly")]
    public GameObject obj;
    public bool stopSpawning;
    public float startTime;
    public float delay;
    public bool rand = true;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public Vector3 spawnPoint;
    RaycastHit hit;
    float height;
    MeshFilter mesh;
    int childHighest = -1;

    private void Start()
    {
        if (!rand && spawnPoint != null) { spawnPoint = Vector3.zero; }
        TryGetComponent<MeshFilter>(out mesh);
        if (mesh != null) { height = mesh.sharedMesh.bounds.extents.y * mesh.gameObject.transform.lossyScale.y + mesh.gameObject.transform.position.y; }
        else if (mesh == null)
        {
            float highest = -1;
            for (int child = 0; child < obj.transform.childCount; child++)
            {
                if (obj.transform.GetChild(child).GetComponent<MeshFilter>() != null)
                {
                    // get mesh 
                    mesh = obj.transform.GetChild(child).GetComponent<MeshFilter>();
                    // mesh height
                    height = mesh.sharedMesh.bounds.extents.y * mesh.gameObject.transform.lossyScale.y + mesh.gameObject.transform.position.y;
                    if (height > highest)
                    {
                        highest = height;
                        childHighest = child;
                    }
                }
            }
        }
        InvokeRepeating("Spawn", startTime, delay);
    }

    public void Spawn()
    {
        if (stopSpawning)
        {
            CancelInvoke("Spawn");
        }
        if (rand)
        {
            Vector3 pos = new Vector3(Random.Range(minX, maxX), 2, Random.Range(minZ, maxZ));
            if (Physics.Raycast(pos + new Vector3(0, 100, 0), Vector3.down, out hit, 200.0f, LayerMask.GetMask("Ground")))
            {
                if(childHighest > 0){
                    mesh = obj.transform.GetChild(childHighest).GetComponent<MeshFilter>();
                    height = mesh.sharedMesh.bounds.extents.y * mesh.gameObject.transform.lossyScale.y + mesh.gameObject.transform.position.y;
                }
                PhotonNetwork.Instantiate(obj.name, hit.point + new Vector3(0, height + 0.03f, 0), transform.rotation);
                
            }
            else
            {
                // no ground 
                print("no ground, try adjusting bounds");
                return;
            }
        }
        else
        {
            PhotonNetwork.Instantiate(obj.name, spawnPoint, transform.rotation);
        }
    }
}
