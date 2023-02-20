using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] GameObject pickupVFX;
    [SerializeField] GameObject pickupSFX;
    public GameObject[] items;
    private bool once = false;
    GameObject powerupUI;

    private void Awake()
    {
        powerupUI = GameObject.FindGameObjectWithTag("PowerupUI");
    }

    private void OnTriggerEnter(Collider other) {
        // player hit item giver
        if(other.gameObject.tag == "Player"){
            // only run one time
            if(!once){
                once = true;
                // root of player
                Transform root = other.gameObject.transform.root;
                // get attach point of player
                Transform attachPointObject = root.GetChild(2).GetChild(0).Find("attachPoint");
                print(attachPointObject);
                // item already on the attach point of player
                if(attachPointObject.childCount > 0){
                    playBoxVFX();
                    Destroy(gameObject, 0.1f);
                    return;
                }
                // get random item in list
                // GameObject item = items[Random.Range(0, items.Length)]; 
                GameObject item = items[0];

                // create the item at the players attach point
                GameObject newItem = PhotonNetwork.Instantiate(item.name, attachPointObject.position, Quaternion.identity);

                // give attach point and player view to the item      
                newItem.GetComponentInChildren<Item>().setAttachPoint(attachPointObject);
                print(newItem);

                //VFX + shrink object
                playBoxVFX();
                //check if player collided with is mine
                if (other.transform.root.GetComponent<PhotonView>().IsMine && powerupUI)
                {
                    powerupUI.GetComponent<PowerupUIController>().playUIVFX();
                }

                Destroy(gameObject, 0.1f);
            }
        }
    }
    
    private void playBoxVFX()
    {
        //SFX
        PhotonNetwork.Instantiate(pickupSFX.name, transform.position, Quaternion.identity);

        //VFX
        PhotonNetwork.Instantiate(pickupVFX.name, transform.position, Quaternion.identity);
        LeanTween.scale(gameObject, Vector3.zero, 0.1f).setEaseInOutBounce();
    }
}
