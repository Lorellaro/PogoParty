using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(BoxCollider))]
public class RespawnTrigger : MonoBehaviour
{
    private GameController gameController;
    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject playerObj = other.transform.root.gameObject;
        PhotonView view = playerObj.GetComponent<PhotonView>();
        if (!view.IsMine) return;
        if (other.CompareTag(Tags.Player))
        {
            gameController.LoadPlayer();
        }
    }
}
