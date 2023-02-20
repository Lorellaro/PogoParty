using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public GameObject[] roots;
    // Start is called before the first frame update
    private void Awake() {
        foreach (GameObject root in roots)
        {
            //DontDestroyOnLoad(root.transform.gameObject);
        }
    }
}
