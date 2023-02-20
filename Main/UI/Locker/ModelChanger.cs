using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelChanger : MonoBehaviour
{
    [SerializeField] private ItemTypes itemType;

    [SerializeField] private GameObject modelPrefab;
    private SkinnedMeshRenderer _playerMeshRenderer;
    private MeshFilter _backpackMeshFilter;
    private MeshRenderer _backpackMeshRenderer;
    private MeshFilter _pogoStickMainMeshFilter;
    private MeshRenderer _pogoStickMainMeshRenderer;
    private MeshFilter _pogoStickPoleMeshFilter;
    private MeshRenderer _pogoStickPoleMeshRenderer;
    private ParticleSystemRenderer _pogoStickSmokeLeftPSR;
    private ParticleSystemRenderer _pogoStickSmokeRightPSR;
    private ParticleSystem _pogoStickSmokeLeftPS;
    private ParticleSystem _pogoStickSmokeRightPS;
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerRoot = GameObject.FindWithTag("PlayerRoot");
        _playerMeshRenderer = playerRoot.transform.GetChild(0).GetChild(0)
            .GetComponent<SkinnedMeshRenderer>();
        _backpackMeshFilter = playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0)
            .GetChild(3)
            .GetComponent<MeshFilter>();
        _backpackMeshRenderer = playerRoot.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(2).GetChild(0)
            .GetChild(3)
            .GetComponent<MeshRenderer>();
        _pogoStickMainMeshFilter = playerRoot.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0)
            .GetComponent<MeshFilter>();
        _pogoStickMainMeshRenderer = playerRoot.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0)
            .GetComponent<MeshRenderer>();
        _pogoStickPoleMeshFilter = playerRoot.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0)
            .GetComponent<MeshFilter>();
        _pogoStickPoleMeshRenderer = playerRoot.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0)
            .GetComponent<MeshRenderer>();
        _pogoStickSmokeLeftPSR = playerRoot.transform.GetChild(2).GetChild(0).GetChild(10).GetComponent<ParticleSystemRenderer>();
        _pogoStickSmokeRightPSR = playerRoot.transform.GetChild(2).GetChild(0).GetChild(9).GetComponent<ParticleSystemRenderer>();
        _pogoStickSmokeLeftPS = playerRoot.transform.GetChild(2).GetChild(0).GetChild(10).GetComponent<ParticleSystem>();
        _pogoStickSmokeRightPS = playerRoot.transform.GetChild(2).GetChild(0).GetChild(9).GetComponent<ParticleSystem>();
    }

    public void ChangeModel()
    {
        switch (itemType)
        {
            case ItemTypes.Outfit:
                _playerMeshRenderer.sharedMesh = modelPrefab.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                _playerMeshRenderer.sharedMaterials = modelPrefab.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
                break;
            case ItemTypes.Backpack:
                _backpackMeshFilter.sharedMesh = modelPrefab.GetComponent<MeshFilter>().sharedMesh;
                _backpackMeshRenderer.sharedMaterials = modelPrefab.GetComponent<MeshRenderer>().sharedMaterials;
                break;
            case ItemTypes.PogoStick:
                _pogoStickMainMeshFilter.sharedMesh = modelPrefab.transform.GetChild(1).GetChild(0)
                    .GetComponent<MeshFilter>().sharedMesh;
                _pogoStickMainMeshRenderer.sharedMaterials = modelPrefab.transform.GetChild(1).GetChild(0)
                    .GetComponent<MeshRenderer>().sharedMaterials;
                _pogoStickPoleMeshFilter.sharedMesh = modelPrefab.transform.GetChild(0).GetChild(0)
                    .GetComponent<MeshFilter>().sharedMesh;
                _pogoStickPoleMeshRenderer.sharedMaterials = modelPrefab.transform.GetChild(0).GetChild(0)
                    .GetComponent<MeshRenderer>().sharedMaterials;
                _pogoStickSmokeLeftPSR.sharedMaterial = modelPrefab.transform.GetChild(5)
                    .GetComponent<ParticleSystemRenderer>().sharedMaterial;
                _pogoStickSmokeRightPSR.sharedMaterial = modelPrefab.transform.GetChild(6)
                    .GetComponent<ParticleSystemRenderer>().sharedMaterial;
                var pogoStickLeftSmokeMain = _pogoStickSmokeLeftPS.main;
                var pogoStickRightSmokeMain = _pogoStickSmokeRightPS.main;
                var modelSmokeLeft = modelPrefab.transform.GetChild(5).GetComponent<ParticleSystem>().main;
                var modelSmokeRight = modelPrefab.transform.GetChild(6).GetComponent<ParticleSystem>().main;
                pogoStickLeftSmokeMain.gravityModifier = modelSmokeLeft.gravityModifier;
                pogoStickRightSmokeMain.gravityModifier = modelSmokeRight.gravityModifier;
                
                    // modelPrefab.transform.GetChild(6)
                    // .GetComponent<ParticleSystem>().main.gravityModifier;
                break;
            case ItemTypes.Scarf:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
        }
    }
    
    private enum ItemTypes
    {
        Outfit,
        Backpack,
        PogoStick,
        Scarf
    }
}
