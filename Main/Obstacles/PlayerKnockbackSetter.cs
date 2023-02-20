using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerKnockbackSetter : MonoBehaviour
{
    [SerializeField] Rigidbody pogoRB;

    //Needs a position to calculate the opposite from
    [PunRPC]
    public void oppositeKnockback(Vector3 _knockBackDirection, float knockBackForce, float upwardsKnockBackForce)
    {
        pogoRB.velocity = (_knockBackDirection.normalized * knockBackForce + Vector3.up * upwardsKnockBackForce) * Time.deltaTime;
    }
}
