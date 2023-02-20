using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class SkinChanger : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
    [SerializeField] private MeshFilter backpackMeshFilter;
    [SerializeField] private MeshRenderer backpackMeshRenderer;
    [SerializeField] private MeshFilter pogoStickMainMeshFilter;
    [SerializeField] private MeshRenderer pogoStickMainMeshRenderer;
    [SerializeField] private MeshFilter pogoStickPoleMeshFilter;
    [SerializeField] private MeshRenderer pogoStickPoleMeshRenderer;

    [SerializeField] private GameObject[] skins;
    [SerializeField] Leaderboard leaderboard;

    [SerializeField] List<GameObject> playerRoots;

    private int currentSkinIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("updatePlayerRoots", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void updatePlayerRoots()
    {
        playerRoots = leaderboard.GetAllPlayers();//store all player roots
        print(playerRoots.Count);

        //Assign mesh vals for my player
        for (int i = 0; i < playerRoots.Count; i++)
        {
            if (playerRoots[i].GetComponent<PhotonView>().IsMine)//find my player root
            {
                //Cache all sections to swap
                playerMeshRenderer = playerRoots[i].transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();
                backpackMeshFilter = playerRoots[i].transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3).GetComponent<MeshFilter>();
                backpackMeshRenderer = playerRoots[i].transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3).GetComponent<MeshRenderer>();
                pogoStickMainMeshFilter = playerRoots[i].transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshFilter>();
                pogoStickMainMeshRenderer = playerRoots[i].transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>();
                pogoStickPoleMeshFilter = playerRoots[i].transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshFilter>();
                pogoStickPoleMeshRenderer = playerRoots[i].transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //change skin to chef on button press
    public void OnChangeSkin(InputValue value)
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("updatePlayerRoots", RpcTarget.AllBuffered);

        for (int i = 0; i < playerRoots.Count; i++)
        {
            if (playerRoots[i].GetComponent<PhotonView>().IsMine)//find my player root
            {
                // Player Skin
                playerMeshRenderer.sharedMesh = skins[currentSkinIndex].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                playerMeshRenderer.sharedMaterials = skins[currentSkinIndex].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
                // Backpack
                currentSkinIndex++;
                backpackMeshFilter.sharedMesh = skins[currentSkinIndex].GetComponent<MeshFilter>().sharedMesh;
                backpackMeshRenderer.sharedMaterials = skins[currentSkinIndex].GetComponent<MeshRenderer>().sharedMaterials;
                // Pogostick
                currentSkinIndex++;
                pogoStickMainMeshFilter.sharedMesh = skins[currentSkinIndex].transform.GetChild(5)
                    .GetComponent<MeshFilter>().sharedMesh;
                pogoStickMainMeshRenderer.sharedMaterials = skins[currentSkinIndex].transform.GetChild(5)
                    .GetComponent<MeshRenderer>().sharedMaterials;
                pogoStickPoleMeshFilter.sharedMesh = skins[currentSkinIndex].transform.GetChild(0).GetChild(0)
                    .GetComponent<MeshFilter>().sharedMesh;
                pogoStickPoleMeshRenderer.sharedMaterials = skins[currentSkinIndex].transform.GetChild(0).GetChild(0)
                    .GetComponent<MeshRenderer>().sharedMaterials;
                //Pogostick VFX
                Transform pogostickTransform = pogoStickPoleMeshRenderer.transform.parent.parent;
                pogostickTransform.GetChild(21).gameObject.SetActive(true);
                pogostickTransform.GetChild(22).gameObject.SetActive(true);
                pogostickTransform.GetChild(18).gameObject.SetActive(false);
                pogostickTransform.GetChild(16).gameObject.SetActive(false);
            }
        }
    }
}
