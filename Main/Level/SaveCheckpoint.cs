using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SaveCheckpoint : MonoBehaviour
{
    [FormerlySerializedAs("_gameController")] [SerializeField] private GameController gameController;
    [FormerlySerializedAs("_checkPointTriggered")] public bool checkPointTriggered;

    [Header("BeaconObjs")]
    [SerializeField] MeshRenderer cylinder0;
    [SerializeField] MeshRenderer cylinder2;

    [SerializeField] List<ParticleSystemRenderer> checkpointVFXs;

    [Header("Materials")]
    [SerializeField] Material emissiveInactive;
    [SerializeField] Material emissiveActive;
    [SerializeField] Material activeVFX;
    [SerializeField] Material inactiveVFX;

    AudioSource flagReachedSound;
    ParticleSystem pSystem;
    bool loaded = false;
    public GameObject loadUI;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        flagReachedSound = GetComponent<AudioSource>();
        pSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject playerObj = other.transform.root.gameObject;
        PhotonView view = playerObj.GetComponent<PhotonView>();
        if (!view.IsMine) return;
        
        if (other.CompareTag(Tags.Player) && !checkPointTriggered)
        {
            //Reset Checkpoint
            if (loadUI)
            {
                StartCoroutine(showLoadUI());
            }

            var position = transform.position;
            gameController.SavePlayer(position.x, position.y, position.z);
            checkPointTriggered = true;

            flagReachedSound.Play();
            pSystem.Play();

            // Swap Materials
            swapMaterials();

        }
    }

    private void swapMaterials()//swaps mesh and particle system materials
    {
        List<Material> tempList = new List<Material>();

        tempList.Add(emissiveActive);
        tempList.Add(activeVFX);
        //pos 0 is colour for beam
        //pos 1 is colour for vfx

        //mesh

        //Assign materials

        //cylinder 0
        Material[] tempMaterials = cylinder0.materials;

        tempMaterials[2] = tempList[0]; //change emissive's pos to  emissive material

        cylinder0.materials = tempMaterials;

        //cylinder 2
        tempMaterials = cylinder2.materials;
        tempMaterials[0] = tempList[1];

        cylinder2.materials = tempMaterials;

        //particles

        for (int i = 0; i < checkpointVFXs.Capacity; i++)
        {
            tempMaterials = checkpointVFXs[i].sharedMaterials;
            tempMaterials[0] = tempList[1];

            checkpointVFXs[i].sharedMaterials = tempMaterials;
        }
    }

    IEnumerator showLoadUI(){
        loadUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        loadUI.SetActive(false);
    } 
}
