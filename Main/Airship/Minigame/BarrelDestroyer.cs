using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BarrelDestroyer : MonoBehaviour
{
    [SerializeField] int pointsToGive = 100;
    [SerializeField] AirshipMinigameController airshipMinigameController;
    [SerializeField] GameObject crackedBarrel;
    [SerializeField] PositionResetter playerPositionReseter;

    private void OnTriggerEnter(Collider other)
    {
        //If object is player
        if (other.gameObject.CompareTag("Player"))
        {
            //Reset position
            Transform playerTransform = other.gameObject.transform.root;
            playerPositionReseter.ResetPlayerPosition(playerTransform);
        }



        //If object is barrel
        if (other.gameObject.CompareTag("MinigameBarrel"))
        {
            MinigameBarrel otherBarrel = other.gameObject.GetComponent<MinigameBarrel>();

            //multiply this points by barrel's multiplier gained from bouncing
            int pointsEarned = Mathf.RoundToInt(pointsToGive * otherBarrel.GetMultiplier());

            //Set points to prev point count + amount from 'this' destruction
            airshipMinigameController.setPoints(airshipMinigameController.getPoints() + pointsEarned);

            //PlayVFX & SFX
            GameObject explosionObj = other.gameObject.transform.GetChild(0).gameObject;
            explosionObj.GetComponent<ParticleSystem>().Play();
            explosionObj.GetComponent<AudioSource>().Play();

            //Disable off mesh and coll
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<MeshCollider>().enabled = false;

            //Swap to cracked barrel
            GameObject newCrackedBarrel = PhotonNetwork.Instantiate(crackedBarrel.name, other.transform.position, other.transform.rotation, 0);

            //Get Child Transforms
            Transform[] newBarrelMeshes = newCrackedBarrel.transform.GetChild(0).GetComponent<DestrucibleObject>().GetTransforms();

            //Set all child materials to the same strength as the barrel that was just destroyed
            for(int i = 0; i < newBarrelMeshes.Length; i++)
            {
                Material newBarrelMaterial = newBarrelMeshes[i].GetComponent<MeshRenderer>().material;
                newBarrelMaterial.SetFloat("_ColourStrength", otherBarrel.GetColorStrength());
            }

            //Destroy
            Destroy(other.gameObject, 2f);
        }
    }
}
