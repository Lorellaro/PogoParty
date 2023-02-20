using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject fadeScreen;
    [SerializeField] Transform _playerTempPos;
    [SerializeField]private Vector3 _playerStartPos;
    [SerializeField] GameObject _player;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private PogoStickPhysics pogoStick;
    [SerializeField] private GameObject gameController;
    private GameController _gameController;

    private bool sceneUnloaded;
    
    readonly string[] scenes = {
        "Tutorial",
        "Cave",
        "Third_Level"
    };
    [Tooltip("The scene to load when the main scene is loaded")]
    [Range(0,2)]
    [SerializeField] private int sceneIndex = 0; 
    
    private void Awake()
    {
        if (fadeScreen == null)
        {
            fadeScreen = GameObject.Find("FadeScreen");
        }

        fadeScreen.SetActive(false);
        endScreen.SetActive(false);
        _gameController = gameController.GetComponent<GameController>();
        // _playerTempPos = GameObject.Find("PlayerTempPos").transform.position;
    }
    
    

    public void LoadScene(int loadIndex)
    {
        // Debug.Log("Scene loading");
        fadeScreen.SetActive(true);
        // gameController.SetActive(true);
        StartCoroutine(LoadSceneEnumerator(loadIndex));
        
    }
    


    IEnumerator LoadSceneEnumerator(int loadIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scenes[loadIndex], LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            // Debug.Log("Scene loading...");
            yield return null;
        }
        //Debug.Log("Scene Loaded");
        _playerStartPos = GameObject.FindWithTag("StartPos").transform.position;
        SaveSystem.SavePlayer(pogoStick, _playerStartPos.x, _playerStartPos.y, _playerStartPos.z);
        StartCoroutine(LiftFadeScreen());
    }

    IEnumerator LiftFadeScreen()
    {
        yield return new WaitForSeconds(1);
        _gameController.LoadPlayer();
        yield return new WaitForSeconds(2);
        fadeScreen.SetActive(false);
    }

    public void UnloadScene(int unloadIndex)
    {
        fadeScreen.SetActive(true);
        if(unloadIndex == 1) endScreen.SetActive(true);
        StartCoroutine(UnLoadSceneEnumerator(unloadIndex));
    }

    IEnumerator UnLoadSceneEnumerator(int unloadIndex)
    {
        // sceneUnloaded = false;
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scenes[unloadIndex]);
        if (asyncOperation != null)
        {
            while (!asyncOperation.isDone)
            {
                // Debug.Log("Scene loading...");
                yield return null;
            }
        }

        LoadScene(unloadIndex + 1);
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadScene(sceneIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
