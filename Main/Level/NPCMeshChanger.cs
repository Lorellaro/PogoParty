using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMeshChanger : MonoBehaviour
{
    #region Variables

    #region MeshRefs
    [Header("Mesh References")] 
    
    [SerializeField] private MeshRenderer headMeshRenderer;
    
    [SerializeField] private MeshFilter glassesMeshFilter;
    [SerializeField] private MeshRenderer glassesMeshRenderer;
    
    [SerializeField] private MeshFilter headAccessoryMeshFilter;
    [SerializeField] private MeshRenderer headAccessoryMeshRenderer;

    [SerializeField] private MeshFilter lBrowMeshFilter;
    [SerializeField] private MeshRenderer lBrowMeshRenderer;
    
    [SerializeField] private MeshFilter rBrowMeshFilter;
    [SerializeField] private MeshRenderer rBrowMeshRenderer;
    
    [SerializeField] private MeshFilter beardMeshFilter;
    [SerializeField] private MeshRenderer beardMeshRenderer;
    
    [SerializeField] private MeshFilter noseMeshFilter;
    [SerializeField] private MeshRenderer noseMeshRenderer;
    #endregion

    #region MeshModels

    [Header("Models")]
    [SerializeField] private GameObject[] maleHeadAccessories;
    [SerializeField] private GameObject[] femaleHeadAccessories;
    [SerializeField] private GameObject[] glasses;
    [SerializeField] private GameObject[] beards;
    [SerializeField] private GameObject[] leftEyeBrows;
    [SerializeField] private GameObject[] rightEyeBrows;
    [SerializeField] private GameObject[] noses;

    #endregion

    #region Materials

    [Header("Materials")] 
    [SerializeField] private Material[] noseMaterials;
    [SerializeField] private Material[] headMaterials;
    [SerializeField] private Material[] beardMaterials;

    #endregion

    private bool _female;
    
    #endregion
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the seed for random so it will be synced between players
        Random.InitState(gameObject.name.Length);

        if (Random.Range(0, 100) < 50) _female = true;

        
        headMeshRenderer.sharedMaterial = headMaterials[Random.Range(0, headMaterials.Length - 1)];

        GameObject glassesGameObject = glasses[Random.Range(0, glasses.Length - 1)];
        if (glassesGameObject == null)
        {
            glassesMeshFilter.sharedMesh = null;
            glassesMeshRenderer.sharedMaterials = null;
        }
        else
        {
            glassesMeshFilter.sharedMesh = glassesGameObject.GetComponent<MeshFilter>().sharedMesh;
            glassesMeshRenderer.sharedMaterials = glassesGameObject.GetComponent<MeshRenderer>().sharedMaterials;
        }

        if (_female)
        {
            GameObject femaleHeadAccessory = femaleHeadAccessories[Random.Range(0, femaleHeadAccessories.Length - 1)];
            if (femaleHeadAccessory == null)
            {
                headAccessoryMeshFilter.sharedMesh = null;
                headAccessoryMeshRenderer.sharedMaterials = null;
            }
            else
            {
               headAccessoryMeshFilter.sharedMesh = femaleHeadAccessory.GetComponent<MeshFilter>().sharedMesh;
               headAccessoryMeshRenderer.sharedMaterials =
                   femaleHeadAccessory.GetComponent<MeshRenderer>().sharedMaterials;
            }
        }
        else
        {
            GameObject maleHeadAccessory = maleHeadAccessories[Random.Range(0, maleHeadAccessories.Length - 1)];
            if (maleHeadAccessory == null)
            {
                headAccessoryMeshFilter.sharedMesh = null;
                headAccessoryMeshRenderer.sharedMaterials = null;
            }
            else
            {
                headAccessoryMeshFilter.sharedMesh = maleHeadAccessory.GetComponent<MeshFilter>().sharedMesh;
                headAccessoryMeshRenderer.sharedMaterials =
                    maleHeadAccessory.GetComponent<MeshRenderer>().sharedMaterials;
            }
        }

        GameObject lBrowGameObject = leftEyeBrows[Random.Range(0, leftEyeBrows.Length - 1)];
        lBrowMeshFilter.sharedMesh = lBrowGameObject.GetComponent<MeshFilter>().sharedMesh;
        lBrowMeshRenderer.sharedMaterials = lBrowGameObject.GetComponent<MeshRenderer>().sharedMaterials;
        
        GameObject rBrowGameObject = rightEyeBrows[Random.Range(0, rightEyeBrows.Length - 1)];
        rBrowMeshFilter.sharedMesh = rBrowGameObject.GetComponent<MeshFilter>().sharedMesh;
        rBrowMeshRenderer.sharedMaterials = rBrowGameObject.GetComponent<MeshRenderer>().sharedMaterials;

        if (_female)
        {
            beardMeshFilter.sharedMesh = null;
            beardMeshRenderer.sharedMaterials = null;
        }
        else
        {
            GameObject beardGameObject = beards[Random.Range(0, beards.Length - 1)];
            if (beardGameObject == null)
            {
                beardMeshFilter.sharedMesh = null;
                beardMeshRenderer.sharedMaterials = null;
            }
            else
            {
                beardMeshFilter.sharedMesh = beardGameObject.GetComponent<MeshFilter>().sharedMesh;
                beardMeshRenderer.sharedMaterial = beardMaterials[Random.Range(0, beardMaterials.Length - 1)];
            }
        }

        noseMeshFilter.sharedMesh = noses[Random.Range(0, noses.Length - 1)].GetComponent<MeshFilter>().sharedMesh;
        noseMeshRenderer.sharedMaterial = noseMaterials[Random.Range(0, noseMaterials.Length - 1)];

    }
}
