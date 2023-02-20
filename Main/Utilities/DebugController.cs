using System.Collections;
using System.Collections.Generic;
using GriefingSystem;
using Main.GameHandlers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Utilities
{
    public class DebugController : MonoBehaviour
    {
        public static DebugController Instance;
        private bool showConsole;
        private bool showHelp;

        private string input;
        private string _commandOutput;
        private bool showCommandOutput;


        public static DebugCommand Help;
        public static DebugCommand CheckpointCount;
        public static DebugCommand<int> TeleportToCheckpoint;
        public static DebugCommand<string> TeleportToCheckpoint2;
        public static DebugCommand StartRace;
        public static DebugCommand UpdateRaceData;
        public static DebugCommand DisplayRaceData;
        public static DebugCommand<int> TakeScreenshot;
        public static DebugCommand<string> ChangeSkin;
        public static DebugCommand<string> ChangeOutfit;
        public static DebugCommand<string> ChangeBackpack;
        public static DebugCommand<string> ChangePogo;
        public static DebugCommand<string> GivePowerup;
        public static DebugCommand<int> CamSpeed;
        public static DebugCommand<string> ChangePlatterColour;

        public List<object> commandList;

        private Vector2 scroll;

        private InputManager _inputManager;

        private int _viewID;
        private GameObject _player;

        [Header("Power Up Variables (Must be in same order)")]
        [SerializeField] private GameObject[] powerups;
        [SerializeField] private Sprite[] powerupIcons;
        [Tooltip("Names need to be lowercase and one word")]
        [SerializeField] private string[] powerupNames;

        public void OnToggleDebug(InputValue value)
        {
            showConsole = !showConsole;
            if (showConsole)
            {
                _inputManager.AllowInput(false);
            }
            else _inputManager.AllowInput(true);

            if (!showConsole) showHelp = false;
        }

        public void OnReturn(InputValue value)
        {
            if(!showConsole) return;
            HandleInput();
            input = "";
            if (showCommandOutput)
            {
                StartCoroutine(HideCommandOutput());
            }

            if (showHelp) return;
            OnToggleDebug(value);
        }
        private void Awake()
        {
            Instance = this;
        
            Help = new DebugCommand("help", "Shows all commands", "help", () =>
            {
                Debug.Log("Help Command Called");
                showHelp = !showHelp;
            });

            CheckpointCount = new DebugCommand("checkpoint_count", "Gets the number of checkpoints in the level.",
                "checkpoint_count",
                () =>
                {
                    DisplayCommandOutput("Number of checkpoints in level: " +
                                         CheckpointManager.Instance.GetNumberOfCheckpoints());
                });

            TeleportToCheckpoint = new DebugCommand<int>("teleport_to_checkpoint",
                "Teleports the player to the defined checkpoint", "teleport_to_checkpoint <checkpoint_id>", (x) =>
                {
                    CheckpointManager.Instance.GoToCheckpoint(x);
                });
        
            TeleportToCheckpoint2 = new DebugCommand<string>("teleport_to_checkpoint",
                "Teleports the player to the defined checkpoint", "teleport_to_checkpoint <checkpoint_name>", (x) =>
                {
                    Debug.Log("Hello");
                    CheckpointManager.Instance.GoToCheckpoint(x);
                });

            StartRace = new DebugCommand("start_race", "Gets all players in level and stores in race manager",
                "start_race",
                () =>
                {
                    RoundManager.Instance.CallGetAllPlayers();
                });
        
            UpdateRaceData = new DebugCommand("update_race_data", "Updates race data in race manager",
                "update_race_data",
                () =>
                {
                    RoundManager.Instance.CallUpdatePlayerRoundData();
                });
        
            DisplayRaceData = new DebugCommand("display_race_data", "Logs Race managers data to console",
                "display_race_data",
                () =>
                {
                    RoundManager.Instance.DisplayPlayers();
                });

            TakeScreenshot = new DebugCommand<int>("take_screenshot",
                "Takes screenshot and saves it to the persistant path", "take_screenshot <size>", i =>
                {
                    Screenshot.Instance.TakeScreenshot(i);
                });

            ChangeSkin = new DebugCommand<string>("change_skin", "Changes the players skin", "change_skin <name>", s =>
            {
                SkinManager.Instance.CallChangeSkin(_viewID, s);
            });
        
            ChangeOutfit = new DebugCommand<string>("change_outfit", "Changes the players outfit", "change_outfit <name>", s =>
            {
                SkinManager.Instance.CallChangeOutfit(_viewID, s);
            });
        
            ChangeBackpack = new DebugCommand<string>("change_backpack", "Changes the players backpack", "change_backpack <name>", s =>
            {
                SkinManager.Instance.CallChangeBackpack(_viewID, s);
            });
        
            ChangePogo = new DebugCommand<string>("change_pogo", "Changes the players pogo", "change_pogo <name>", s =>
            {
                SkinManager.Instance.CallChangePogo(_viewID, s);
            });

            GivePowerup = new DebugCommand<string>("give_powerup", "Gives the player the specified powerup",
                "give_powerup <name>",
                s =>
                {
                    GameObject powerupUI = GameObject.FindGameObjectWithTag("PowerupUI");
                    Transform playerPogostick = _player.transform.GetChild(2).GetChild(0);

                    PowerupHolder playerPowerupHolder = playerPogostick.GetComponent<PowerupHolder>();

                    int powerupIndex = 0;
                    for (int i = 0; i < powerupNames.Length; i++)
                    {
                        if (powerupNames[i] == s.ToLower())
                        {
                            powerupIndex = i;
                        }
                    }
                
                    playerPowerupHolder.SetPowerupHeld(powerups[powerupIndex]);
                    playerPowerupHolder.SetPowerupHeldUI(powerupIcons[powerupIndex]);

                    //check if player collided with is mine
                    if (_player.transform.root.GetComponent<PhotonView>().IsMine && powerupUI)
                    {
                        powerupUI.GetComponent<PowerupUIController>().playUIVFX();
                    }
                });

            CamSpeed = new DebugCommand<int>("cam_speed", "Sets spectator cam speed", "cam_speed <newSpeed>", i =>
            {
                CameraSpectatorHandler.Instance.moveSpeed = i;
            });

            ChangePlatterColour = new DebugCommand<string>("change_platter_colour",
                "Sets the platter colour in the resturant", "change_platter_colour <teamname>",
                s =>
                {
                    GameObject[] platters = GameObject.FindGameObjectsWithTag("Platter");
                    foreach (var platter in platters)
                    {
                        PhotonView pv = platter.GetComponent<PhotonView>();
                        if (pv.IsMine)
                        {
                            platter.GetComponent<PlatterHolder>().CallChangePlatterColour(_viewID, s);
                        }
                    }
                });

            commandList = new List<object>
            {
                Help,
                CheckpointCount,
                TeleportToCheckpoint,
                TeleportToCheckpoint2,
                StartRace,
                UpdateRaceData,
                DisplayRaceData,
                TakeScreenshot,
                ChangeSkin,
                ChangeOutfit,
                ChangeBackpack,
                ChangePogo,
                GivePowerup,
                CamSpeed,
                ChangePlatterColour
            };

            _inputManager = InputManager.Instance;


        }

        private void Start()
        {
            if(RoundManager.Instance == null)
            {
                StartCoroutine(DelayedActivate());
                return;
            }
            RoundManager.Instance.onRoundManagerReady += Activate;
        }

        private void Activate()
        {
            Debug.Log("Activate");
            GameObject[] playerRoots = GameObject.FindGameObjectsWithTag("PlayerRoot");
            foreach (var player in playerRoots)
            {
                Debug.Log(player.GetComponent<PhotonView>().ViewID);
                if (player.GetComponent<PhotonView>().IsMine)
                {
                    _viewID = player.GetComponent<PhotonView>().ViewID;
                    _player = player;
                }
            }
        }

        private IEnumerator HideCommandOutput()
        {
            yield return new WaitForSeconds(5);
            showCommandOutput = false;
            _commandOutput = "";
        }

        private void OnGUI()
        {
            if(!showConsole) return;

            float y = 0f;

            if (showHelp)
            {
                GUI.Box(new Rect(0,y,Screen.width, 100), "");

                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

                for (int i = 0; i < commandList.Count; ++i)
                {
                    DebugCommandBase command = commandList[i] as DebugCommandBase;

                    string label = $"{command.commandFormat} - {command.commandDescription}";

                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                
                    GUI.Label(labelRect, label);
                }
            
                GUI.EndScrollView();
            
                y += 100;
            }

            if (showCommandOutput)
            {
                Rect viewport = new Rect(0, 0, Screen.width - 30, 20);
                Rect labelRect = new Rect(0, 0, viewport.width - 100, 20);
                
                GUI.Label(labelRect, _commandOutput);

                y += 30;
            }

            GUI.Box(new Rect(0,y,Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);


        }
    
        private void HandleInput()
        {
            string[] properties = input.Split(new char[] {' '}, 2);
            bool commandSuccess = false;
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

                if (input.Contains(commandBase.commandId))
                {
                    commandSuccess = true;
                   
                    if (properties.Length == 1)
                    {
                        (commandList[i] as DebugCommand)?.Invoke();
                        return;
                    }
                    if (properties[1] == "")
                    {
                        (commandList[i] as DebugCommand)?.Invoke();
                        return;
                    }
                    bool isNumber = int.TryParse(properties[1], out int n);
                    if(isNumber) (commandList[i] as DebugCommand<int>)?.Invoke(n);
                    else (commandList[i] as DebugCommand<string>)?.Invoke(properties[1]);
                
                }
            }
            if(!commandSuccess) DisplayCommandOutput("Command: " + properties[0] + " not found!");
        }

        private IEnumerator DelayedActivate()
        {
            yield return new WaitForSeconds(2);
            Activate();
        }

        public void DisplayCommandOutput(string commandOutput)
        {
            _commandOutput = commandOutput;
            showCommandOutput = true;
        }
    }
}