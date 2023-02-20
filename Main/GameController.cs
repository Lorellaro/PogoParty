using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
// [DefaultExecutionOrder(-1)]
public class GameController : MonoBehaviour
{
    #region [Variables]
    bool menuActive = false;
    // public GameObject shop;
    // public GameObject quit;
    // public GameObject quitButton;
    InputManager _inputManager;
    public PogoStickPhysics player;
    // public GameObject tutorial1;
    // public GameObject tutorial2;
    // public GameObject tutorial3;
    // public GameObject tutorial4;
    // public GameObject ShowShop;
    // public GameObject saveIcon;
    // public MusicController music;
    // int completed = 0;
    // bool quitActive = false;
    // public GameObject shopBackButton;
    [SerializeField] private ButtonFocus _buttonFocus;
    public static string gameMode;
    #endregion
    
    private void Awake() {
        if(_inputManager == null){
            _inputManager = InputManager.Instance;
        }
        
        gameMode = SceneManager.GetActiveScene().name;
        
        // tutorial1 = GameObject.Find("tutorial");
        // tutorial2 = GameObject.Find("tutorial2");
        // tutorial3 = GameObject.Find("tutorial3");
        // tutorial4 = GameObject.Find("tutorial4");
        
        // quit = GameObject.Find("QuitScreen");

        // quitButton = GameObject.Find("No");
        
        // ShowShop = GameObject.Find("ShowShop");
        // shop = GameObject.Find("ShopUI");
        // saveIcon = GameObject.Find("Save");

        // player = GameObject.FindWithTag(Tags.Player).GetComponent<PogoStickPhysics>();
    }

    private void Start() {
        _inputManager.OnStartLoad += LoadPlayer;
        // _inputManager.OnStartShop += toggleMenu;
        _inputManager.OnStartQuit += ExitGame;
        // if (tutorial1 != null && tutorial2 != null && tutorial3 != null && tutorial4 != null)
        // {
        //     tutorial1.SetActive(false);
        //     tutorial2.SetActive(false);
        //     tutorial3.SetActive(false);
        //     tutorial4.SetActive(false);
        // }
        
        // quit.SetActive(false);
        // shop.SetActive(false);
        // ShowShop.SetActive(false);
        // saveIcon.SetActive(false);

        if(SceneManager.GetActiveScene().name == "Main"){
            if(!player.tutorialCompleted){
                // tutorial1.SetActive(true);
                SavePlayer();
            }
            else{
                LoadPlayer();
            }
        }
        
        SavePlayer();

        // GameObject.Find("MusicController").GetComponent<MusicController>().PlayMusic();
        //music.PlayMusic();
    }
    private void Update() {
        
        // if(menuActive||quitActive){
        //     // enable mouse
        //     Cursor.lockState = CursorLockMode.None;
        //     Cursor.visible = true;
        // }
        // else if(!menuActive&&!quitActive){
        //     // disable mouse
        //     Cursor.lockState = CursorLockMode.Locked;
        //     Cursor.visible = false;
        // }
        // if(!player.tutorialCompleted){
        //     StartCoroutine(tutorial());
        // }
        // if(player.inShopRange && completed == 5 && !player.tutorialCompleted){
        //     // show they can press button to open shop
        //     ShowShop.SetActive(true);
        // }
    }
   
    // shop and getting input for this
    // public void toggleMenu()
    // {
    //     if(player.inShopRange){
    //         // allow / disallow inputs
    //         menuActive = !menuActive;
    //         player.menuActive = menuActive;
    //         // Shop 
    //         gameObject.GetComponent<ShopSystem>().UpdateGemCounter();
    //         if (menuActive)
    //         {
    //             _buttonFocus.UpdateSelectedButton(shopBackButton);
    //         }
            
    //         shop.SetActive(menuActive);
    //         if (completed == 5)
    //         {
    //             ShowShop.SetActive(false);
    //             player.tutorialCompleted = true;
    //             SavePlayer();
    //         }
    //     }
    //     else
    //     {
    //         shop.SetActive(false);
    //         player.menuActive = false;
    //     }
    // }
    private void ExitGame()
    {
        // quitActive = !quitActive;
        // // quitButton.Select();
        // if (quitActive)
        // {
        //     _buttonFocus.UpdateSelectedButton(quitButton);
        // }
        // shop.SetActive(false);
        // if (tutorial1 != null && tutorial2 != null && tutorial3 != null && tutorial4 != null)
        // {
        //     tutorial1.SetActive(false);
        //     tutorial2.SetActive(false);
        //     tutorial3.SetActive(false);
        //     tutorial4.SetActive(false);
        // }

        // quit.SetActive(quitActive);
    }
    
    public void Resume(){
        // quitActive = false;
        // quit.SetActive(false);
    }
  
    public void Quit(){
        Debug.Log("Exit Game");
        Application.Quit();
    }

    // call these functions to save/load
    public void SavePlayer()
    {
        Debug.Log("Active? "+gameObject.activeInHierarchy);
        //StartCoroutine(saveIconShow());
        SaveSystem.SavePlayer(player);
    }
    public void SavePlayer(float x, float y, float z)
    {
        // Debug.Log("Active? "+gameObject.activeInHierarchy);
        //StartCoroutine(saveIconShow());
        SaveSystem.SavePlayer(player, x, y, z);
    }

    public void LoadPlayer()
    {
        player = GameObject.Find("PogoStick").GetComponent<PogoStickPhysics>();

        PlayerData data = SaveSystem.LoadPlayer();
        player.tutorialCompleted = data.tutorialCompleted;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1] + 2;
        position.z = data.position[2];
        //player.first = data.first;

        //player.transform.position = position;
        player.ResetPosition(position);
        player.SetRigidBodyVelocity(Vector3.zero);
        player.transform.rotation = Quaternion.Euler(-90,0,0);
        // player.MovePlayerToPos(position);
        // player.SetRagdoll(false);
        // StartCoroutine(DelayRagdoll());


    }
    public static List<GameObject> getPlayers(){
        List<GameObject> players = new List<GameObject>();
        PhotonView[] playerViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
        foreach(PhotonView view in playerViews){
            var player = view.Owner;
            if(player != null){
                print(view.gameObject.name);
                players.Add(view.gameObject);
            }
        }
        return players;
    }
    // IEnumerator DelayRagdoll()
    // {
    //     yield return new WaitForSeconds(0.1f);
    //     player.SetRagdoll(true);
    // }
    

    // tutorial logic
    // IEnumerator tutorial(){ 
    //     if(tutorial1&&tutorial2&&tutorial3&&tutorial4){
    //         // leaning with wasd or left stick then show jump
    //         if((_inputManager.movementInput.x > 0f || _inputManager.movementInput.x < 0f || _inputManager.movementInput.y > 0f || _inputManager.movementInput.y < 0f) && completed == 0){
    //             // show next prompt
    //             yield return new WaitForSecondsRealtime(1.5f);
    //             tutorial1.SetActive(false);
    //             yield return new WaitForSecondsRealtime(0.75f);
    //             tutorial2.SetActive(true);
    //             completed = 1;
    //         }

    //         // jump with space, south button or mouse btn 1 or rt then show look around
    //         if(_inputManager.jumpHeld && completed == 1){
    //             // show next prompt
    //             yield return new WaitForSecondsRealtime(1.75f);
    //             tutorial2.SetActive(false);
    //             yield return new WaitForSecondsRealtime(0.75f);
    //             tutorial3.SetActive(true);
    //             completed = 3;
    //         }
            
    //         // look with right stick or mouse then show quit 
    //         if(completed == 3){
    //             // show next prompt
    //             yield return new WaitForSecondsRealtime(1.5f);
    //             tutorial3.SetActive(false);
    //             yield return new WaitForSecondsRealtime(0.75f);
    //             tutorial4.SetActive(true);
    //             completed = 4;
    //         }

    //         // quit then finish
    //         if(completed == 4){
    //             // disable prompt and finish
    //             yield return new WaitForSecondsRealtime(1.5f);
    //             tutorial4.SetActive(false);
    //             yield return new WaitForSecondsRealtime(0.75f);
    //             completed = 5;
    //         }

    //         // // finish tutorial
    //         // if(completed == 5){
    //         //     // finished tutorial
    //         //     player.tutorialCompleted = true;
    //         // }
    //     }
        
    // }

    // IEnumerator saveIconShow(){
    //     saveIcon.SetActive(true);
    //     yield return new WaitForSecondsRealtime(1.5f);
    //     saveIcon.SetActive(false);
    // }
   
}