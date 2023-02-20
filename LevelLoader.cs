using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    /*
    [Header("Button Text Fonts")]
    [SerializeField] TMP_FontAsset boldFont;
    [SerializeField] TMP_FontAsset standardFont;
    [SerializeField] List<GameObject> hoverBoxes;
    */
    public GameObject optionUI;
    public GameObject titleUI;
    public AudioMixerGroup master;
    public AudioMixerGroup music;
    public AudioMixerGroup sfx;
    public Button[] btns;
    public Slider sldr;
    private void Start() {
        btns = FindObjectsOfType<Button>();
        btns[0].Select();
    }
    public void Quit(){
        Application.Quit();
    }
    public void loadNextScene()
    {
        SceneManager.LoadSceneAsync(1); 
    }
    public void Options(){
        foreach(Button b in btns){
            b.transform.GetChild(1).gameObject.SetActive(false);
        }
        optionUI.SetActive(true);
        titleUI.SetActive(false);
        sldr.Select();
    }

    public void hoverEnterButton(GameObject button)//increases button size and enables child
    {
        GameObject currentHoverBox = button.transform.GetChild(1).gameObject;
        currentHoverBox.SetActive(true);

        button.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        button.GetComponent<AudioSource>().Play();//play selected SFX
        //  button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().font = boldFont; //check first child for textmeshpro
    }

    public void hoverExitButton(GameObject button)//decreases button size and disables child
    {
        GameObject currentHoverBox = button.transform.GetChild(1).gameObject;
        currentHoverBox.SetActive(false);
        button.transform.localScale = new Vector3(1f, 1f, 1f);

       // button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().font = standardFont; //check first child for textmeshpro
    }

    public void back(){
        optionUI.SetActive(false);
        titleUI.SetActive(true);
        btns[0].Select();
    }
    public void changeMasterVolume(float vol){master.audioMixer.SetFloat("Vol1", vol);}
    public void changeMusicVolume(float vol){music.audioMixer.SetFloat("Vol2", vol);}
    public void changeSFXVolume(float vol){sfx.audioMixer.SetFloat("Vol3", vol);}
}
