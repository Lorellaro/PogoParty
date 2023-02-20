using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadowDecal : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        var position = player.position;
        transform.position = new Vector3(position.x + offset.x, position.y + offset.y,
            position.z + offset.z);
    }
}