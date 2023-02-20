using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkinManager : MonoBehaviourPun
{
    public static SkinManager Instance;

    [SerializeField] private String[] names;
    [SerializeField] public GameObject[] skins;

    private void Awake()
    {
        Instance = this;
    }

    #region OLD
    // public void CallChangeSkin(int photonID, string skinIndex)
    // {
    //     photonView.RPC("ChangeSkin", RpcTarget.All, photonID, skinIndex);
    // }

    // [PunRPC]
    // public void ChangeSkin(int photonID, int startSkinIndex)
    // {
    //     Debug.Log(startSkinIndex);
    //     GameObject[] playerRoots = GameObject.FindGameObjectsWithTag("PlayerRoot");
    //     foreach (var playerRoot in playerRoots)
    //     {
    //         PhotonView pv = playerRoot.GetComponent<PhotonView>();
    //         if (pv.ViewID == photonID)
    //         {
    //             
    //             int currentSkinIndex = startSkinIndex;
    //             // Player Mesh
    //             playerRoot.transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh =
    //                 skins[currentSkinIndex].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>()
    //                     .sharedMesh;
    //             playerRoot.transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMaterials =
    //                 skins[currentSkinIndex].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>()
    //                     .sharedMaterials;
    //             currentSkinIndex++;
    //             // Player Backpack
    //             if (skins[currentSkinIndex] == null)
    //             {
    //                 playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3)
    //                     .GetComponent<MeshFilter>().sharedMesh = new Mesh();
    //                 // playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3)
    //                 //     .GetComponent<MeshRenderer>().sharedMaterials = new Material[new Material()];
    //                 return;
    //             }
    //
    //             playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3)
    //                     .GetComponent<MeshFilter>().sharedMesh =
    //                 skins[currentSkinIndex].GetComponent<MeshFilter>().sharedMesh;
    //             playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3)
    //                     .GetComponent<MeshRenderer>().sharedMaterials =
    //                 skins[currentSkinIndex].GetComponent<MeshRenderer>().sharedMaterials;
    //         }
    //     }
    // }
    #endregion

    private GameObject GetPlayerRoot(int photonID)
    {
        GameObject[] playerRoots = GameObject.FindGameObjectsWithTag("PlayerRoot");
        foreach (var playerRoot in playerRoots)
        {
            PhotonView pv = playerRoot.GetComponent<PhotonView>();
            if (pv.ViewID == photonID)
            {
                return playerRoot;
            }
        }

        return null;
    }

    private int GetStartIndexForSkin(string skinName)
    {
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] == skinName)
            {
                return i * 3;
            }
        }

        return -1;
    }
    
    public void CallChangeSkin(int photonID, string skinName)
    {
        photonView.RPC("ChangeSkin", RpcTarget.All, photonID, skinName);
    }

    [PunRPC]
    private void ChangeSkin(int photonID, string skinName)
    {
        CallChangeOutfit(photonID, skinName);
        CallChangeBackpack(photonID, skinName);
        CallChangePogo(photonID, skinName);
    }

    public void CallChangeOutfit(int photonID, string outfitName)
    {
        photonView.RPC("ChangeOutfit", RpcTarget.All, photonID, outfitName);
    }

    [PunRPC]
    private void ChangeOutfit(int photonID, string outfitName)
    {
        GameObject playerRoot = GetPlayerRoot(photonID);
        int skinIndex = GetStartIndexForSkin(outfitName);
        
        if (playerRoot == null || skinIndex == -1)
        {
            Debug.LogError("Outfit or Player not found!");
            return;
        }

        if (skins[skinIndex] == null)
        {
            Debug.LogError($"No Outfit for {outfitName}");
            return;
        }

        Debug.Log(skins[skinIndex].gameObject.name);
        
        playerRoot.transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh =
            skins[skinIndex].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>()
                .sharedMesh;
        
        playerRoot.transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMaterials =
            skins[skinIndex].transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>()
                .sharedMaterials;
    }
    
    public void CallChangeBackpack(int photonID, string backpackName)
    {
        photonView.RPC("ChangeBackpack", RpcTarget.All, photonID, backpackName);
    }
    
    [PunRPC]
    private void ChangeBackpack(int photonID, string backpackName)
    {
        GameObject playerRoot = GetPlayerRoot(photonID);
        int skinIndex = GetStartIndexForSkin(backpackName);
        skinIndex += 1;
        
        if (playerRoot == null || skinIndex == -1)
        {
            Debug.LogError("Backpack or Player not found!");
            return;
        }
        
        if (skins[skinIndex] == null)
        {
            Debug.LogError($"No Backpack for {backpackName}");
            playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3)
                .GetComponent<MeshFilter>().sharedMesh = new Mesh();
            return;
        }
        
        playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3)
                .GetComponent<MeshFilter>().sharedMesh =
            skins[skinIndex].GetComponent<MeshFilter>().sharedMesh;
        
        playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetChild(3)
                .GetComponent<MeshRenderer>().sharedMaterials =
            skins[skinIndex].GetComponent<MeshRenderer>().sharedMaterials;
    }
    
    public void CallChangePogo(int photonID, string pogoName)
    {
        photonView.RPC("ChangePogo", RpcTarget.All, photonID, pogoName);
    }
    
    [PunRPC]
    private void ChangePogo(int photonID, string pogoName)
    {
        GameObject playerRoot = GetPlayerRoot(photonID);
        int skinIndex = GetStartIndexForSkin(pogoName);
        skinIndex += 2;
        
        if (playerRoot == null || skinIndex == -1)
        {
            Debug.LogError("Pogostick or Player not found!");
            return;
        }
        
        if (skins[skinIndex] == null)
        {
            Debug.LogError($"No PogoStick for {pogoName}");
            return;
        }

        // Pogostick Main Mesh
        playerRoot.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshFilter>().sharedMesh =
            skins[skinIndex].transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        
        playerRoot.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>()
            .sharedMaterials = skins[skinIndex].transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>()
            .sharedMaterials;
        
        // Pogostick Pole Mesh
        playerRoot.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0)
            .GetComponent<MeshFilter>().sharedMesh = skins[skinIndex].transform.GetChild(0).GetChild(0)
            .GetComponent<MeshFilter>().sharedMesh;
        
        playerRoot.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0)
            .GetComponent<MeshRenderer>().sharedMaterials = skins[skinIndex].transform.GetChild(0).GetChild(0)
            .GetComponent<MeshRenderer>().sharedMaterials;
        
        // Smoke Particle Systems
        
        // Left
        playerRoot.transform.GetChild(2).GetChild(0).GetChild(18).GetComponent<ParticleSystemRenderer>().sharedMaterial = skins[skinIndex].transform.GetChild(5)
            .GetComponent<ParticleSystemRenderer>().sharedMaterial;
        
        var leftMain = playerRoot.transform.GetChild(2).GetChild(0).GetChild(18).GetComponent<ParticleSystem>().main;
        var skinLeftMain = skins[skinIndex].transform.GetChild(5).GetComponent<ParticleSystem>().main;
        leftMain.gravityModifier = skinLeftMain.gravityModifier;
        
        // Right
        playerRoot.transform.GetChild(2).GetChild(0).GetChild(16).GetComponent<ParticleSystemRenderer>().sharedMaterial = skins[skinIndex].transform.GetChild(6)
            .GetComponent<ParticleSystemRenderer>().sharedMaterial;;
        
        var rightMain = playerRoot.transform.GetChild(2).GetChild(0).GetChild(16).GetComponent<ParticleSystem>().main;
        var skinRightMain = skins[skinIndex].transform.GetChild(6).GetComponent<ParticleSystem>().main;
        rightMain.gravityModifier = skinRightMain.gravityModifier;

    }
    
}