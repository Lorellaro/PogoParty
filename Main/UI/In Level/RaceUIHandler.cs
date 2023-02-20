using System.Collections;
using System.Collections.Generic;
using Main.GameHandlers;
using Main.Level.Race;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace Main.UI.Race
{
    public class RaceUIHandler : MonoBehaviour
    {
        [SerializeField] private UIPulseEffect placeRankingUIPulser;
        [SerializeField] private UIPulseEffect countDownUIPulser;
        [SerializeField] private AudioSource raceStartCountDown;
        [SerializeField] private Sprite[] racePosImages;
        [SerializeField] private Image placeIcon;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private GameObject startCountdownObject;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private GameObject rankingInfoObject;
        [SerializeField] private List<GiveRandomAudioClip> giveRandomAudioClip = new List<GiveRandomAudioClip>();

        private int _playerViewID;
        private int prevRacePos;
        
        // Start is called before the first frame update
        void Start()
        {
            RoundManager.Instance.onRoundManagerReady += RaceManagerReady;
            RoundManager.Instance.onRoundUpdateUI += UpdateUI;
            
            GameObject[] allPlayerObjects = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < allPlayerObjects.Length / 16; ++i)
            {
                GameObject player = allPlayerObjects[i * 16].transform.root.gameObject;
                if (player.GetComponent<PhotonView>().IsMine)
                {
                    _playerViewID = player.GetComponent<PhotonView>().ViewID;
                }
            }
            loadingScreen.SetActive(true);
        }
        
        private void RaceManagerReady()
        {
            prevRacePos = RoundManager.Instance.GetRoundPosition(_playerViewID) - 1;
            StartCoroutine(RaceStartSequence());
        }

        private IEnumerator RaceStartSequence()
        {
            yield return new WaitForSeconds(2f);
            loadingScreen.SetActive(false);
            // Debug.Log("Loading Screen Lifted");
            yield return new WaitForSeconds(0.5f);
            raceStartCountDown.Play();
            startCountdownObject.SetActive(true);
            countDownUIPulser.pulse();
            yield return new WaitForSeconds(1f);
            countdownText.text = "2";
            countDownUIPulser.pulse();

            yield return new WaitForSeconds(1f);
            countdownText.text = "1";
            countDownUIPulser.pulse();

            yield return new WaitForSeconds(1f);
            countdownText.text = "GO!";
            countDownUIPulser.pulse();

            InputManager.Instance.AllowInput(true);
            yield return new WaitForSeconds(1f);
            startCountdownObject.SetActive(false);
            rankingInfoObject.SetActive(true);
            UpdateUI();
        }

        private void UpdateUI()
        {
            int racePos = RoundManager.Instance.GetRoundPosition(_playerViewID) - 1;
            
            if (racePos == prevRacePos) { return; }

            //Change UI
            placeIcon.sprite = racePosImages[racePos];

            //Play Vfx
            placeRankingUIPulser.pulse();

            //Sfx
            if(racePos >= prevRacePos)
            {
                //Play has Overtaken sfx
                giveRandomAudioClip[0].changeAndPlayClip();
            }
            else
            {
                //Play has been overtaken sfx
                giveRandomAudioClip[1].changeAndPlayClip();
            }

            prevRacePos = racePos;
        }
    }
}
