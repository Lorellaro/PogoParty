using Photon.Pun;
using UnityEngine;
using Main.GameHandlers;

namespace Main.Level.Race
{
    public class RacePosCheckpoint : MonoBehaviour
    {
        [SerializeField] private int checkpointID;
        [SerializeField] private bool isActive;
        private GameController _gameController;
        [SerializeField] private bool allowSave = true;
        private void Start()
        {
            RoundManager.Instance.onRoundManagerReady += Activate;
            _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!isActive) return;
            GameObject playerObj = other.transform.root.gameObject;
            PhotonView view = playerObj.GetComponent<PhotonView>();
            if (!view.IsMine) return;

            if (other.CompareTag(Tags.Player))
            {
                RoundManager.Instance.CallUpdateRoundValue(view.ViewID, checkpointID);
                if(!allowSave) return;
                var position = transform.position;
                _gameController.SavePlayer(position.x, position.y, position.z);
                
            }
        }

        private void Activate()
        {
            isActive = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            // Vector3 size = new Vector3(1 * transform.localScale.x, 1 * transform.localScale.y, 1 * transform.localScale.z);
            // Gizmos.DrawWireCube(transform.position, size);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero,Vector3.one);
        }
    }
}