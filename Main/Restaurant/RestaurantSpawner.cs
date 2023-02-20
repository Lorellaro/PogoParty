using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RestaurantSpawner : PlayerSpawnerBase
{
    [SerializeField] GameObject _platterCollectorZone;
    [SerializeField] GameObject platter;
    [SerializeField] RestaurantDishSliderCanvas DishFillCanvas;
    [SerializeField] RestaurantDishGiver restaurantDishGiver;
    [SerializeField] float platterYOffset;

    Vector3 platterRotation = new Vector3(-90,0,0);

    public override void Start()
    {
        //Spawn player setup leaderboard
        base.Start();

        //setup platter

        Vector3 platterPos = new Vector3(randomPos.x, randomPos.y + platterYOffset, randomPos.z);
        GameObject newPlatter = PhotonNetwork.Instantiate(platter.name, platterPos, Quaternion.Euler(platterRotation));
        newPlatter.GetComponent<FixedJoint>().connectedBody = spawnedPlayer.transform.root.GetChild(2).GetChild(0).GetComponent<Rigidbody>();
        newPlatter.GetComponent<PlatterHolder>().myPlayer = spawnedPlayer;
        newPlatter.GetComponent<PlatterHolder>().platterCollectorZone = _platterCollectorZone;

        //GameObject newDishFillCanvas = Instantiate(DishFillCanvas);
        DishFillCanvas.restaurantDishGiver = restaurantDishGiver;
        DishFillCanvas.myPlayer = spawnedPlayer.transform.root.GetChild(2).GetChild(0).gameObject;
    }
}
