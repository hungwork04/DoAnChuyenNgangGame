using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerJoinManager : MonoBehaviour
{
    public static PlayerJoinManager instance;
    public Dictionary<InputDevice, GameObject> players = new Dictionary<InputDevice, GameObject>();
    public HashSet<InputDevice> usedDevices = new HashSet<InputDevice>();
    [SerializeField] private int maxPlayers = 6;
    public int currentPlayerCount = 0;
    public List<PlayerDeviceInfo> playerDevicesInfo = new List<PlayerDeviceInfo>();
    public Button startBtn;
    public Button selectCharacterUI;
    public Button Howtoplaybtn;
    public GameObject UIMapSelection;

    public GameObject UISelectCharacter;
    public GameObject UIHowToplay;

    public List<GameObject> danhsach = new List<GameObject>();
    public SelectCharaterUI selectCharaterUI;
    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject); // Giữ lại khi load Scene mới
        }
        //else
        //{
        //    Destroy(gameObject); // Xóa bớt nếu đã có một instance tồn tại
        //    return;
        //}

        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }


    void Start()
    {
        if (warnText != null)
        {
            warnText.text = string.Empty;
        }
        //Debug.Log("Nhấn nút xác nhận trên thiết bị để tham gia!");
        if (startBtn == null || selectCharacterUI == null) return;
        startBtn.onClick.AddListener(StartGame);
        selectCharacterUI.onClick.AddListener(() => { UISelectCharacter.SetActive(true); });
        Howtoplaybtn.onClick.AddListener(() =>
        {
            //Debug.Log("Button Clicked");
            if (UIHowToplay != null)
            {
                UIHowToplay.SetActive(true);
                Debug.Log("UIHowToPlay is set to active");
            }
            else
            {
                Debug.LogError("UIHowToPlay is not assigned in the Inspector");
            }
        });
    }
    public void ExitFullScreen()
    {
        Screen.fullScreen = false; // Thoát khỏi chế độ fullscreen
    }


    private Coroutine oldco;
    public TextMeshProUGUI warnText;
    public void StartGame()
    {
        if (currentPlayerCount > 0)
        {
            //Debug.Log("Game bắt đầu! Không thể thêm người chơi mới.");

            GameDataManager.instance.SetPlayerDevicesInfo(playerDevicesInfo);
            var playerInputManager = FindAnyObjectByType<PlayerInputManager>();

            if (playerInputManager != null)
            {
                playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;

                //Debug.Log("Chuyển sang chế độ Join Players Manually.");
            }
            //StartCoroutine(SetJoinPlayerManually());
            danhsach.Clear();
            UIMapSelection.SetActive(true);
        }
        else
        {
            if (oldco != null)
            {
                StopCoroutine(oldco);
            }
            Debug.LogWarning("Không thể bắt đầu game mà không có đủ người chơi!");
            warnText.text = "We need 2 or more players to start the game. Please connect additional devices.";
            oldco = StartCoroutine(resetwarntext());
            return;
        }
    }

    private IEnumerator resetwarntext()
    {
        yield return new WaitForSeconds(3.5f);
        warnText.text = string.Empty;
        oldco = null;

    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (currentPlayerCount >= maxPlayers)
        {
            Debug.LogWarning("Đã đạt giới hạn số người chơi!");
            return;
        }

        InputDevice device = playerInput.devices[0];
        Debug.Log($"New Player Joined! Device: {device}");

        if (!players.ContainsKey(device))
        {
            int index;
            players[device] = playerInput.gameObject;
            usedDevices.Add(device);
            if (currentPlayerCount == 0)
            {
                index = 0;
            }
            else
            {
                index = playerDevicesInfo[playerDevicesInfo.Count - 1].playerIndex + 1;
            }
            playerDevicesInfo.Add(new PlayerDeviceInfo(device, 0, index));
            currentPlayerCount++;
        }
    }


    private void WhenPlayerLeft(PlayerInput playerInput)
    {
        if (playerInput.devices.Count == 0)
        {
            Debug.LogWarning("PlayerInput has no devices attached!");
            return;
        }

        InputDevice device = playerInput.devices[0];
        if (!players.ContainsKey(device))
        {
            Debug.LogWarning($"Device {device} not found in dictionary, skipping removal.");
            return;
        }

        //Debug.Log($"Player Left! Removing Device: {device}");

        GameObject playerObject = players[device].transform.parent.gameObject;
        players.Remove(device);
        usedDevices.Remove(device);
        danhsach.Remove(playerObject);
        //Debug.Log("Before removal: " + playerDevicesInfo.Count);
        foreach (var info in playerDevicesInfo)
        {
            Debug.Log($"Device: {info.device}, Index: {info.playerIndex}");
        }

        int index = playerDevicesInfo.FindIndex(p => p.device == device);
        if (index != -1)
        {
            playerDevicesInfo.RemoveAt(index);
        }


        Destroy(playerObject);
        currentPlayerCount--;
    }


    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Removed:
                Debug.Log($"Device removed: {device}");

                if (players.ContainsKey(device) && !GameDataManager.instance.gameStarted)
                {
                    PlayerInput playerInput = players[device].GetComponent<PlayerInput>();
                    if (playerInput != null)
                    {
                        WhenPlayerLeft(playerInput);

                    }
                }
                break;
            case InputDeviceChange.Added:
                Debug.Log($"Device add: {device}");
                break;

        }
    }
}

public class PlayerDeviceInfo
{
    public InputDevice device;
    public int characterType { get; set; }
    public int playerIndex;
    public Color playercolor = Color.white;
    public PlayerDeviceInfo(InputDevice device, int characterType, int playerIndex)
    {
        this.device = device;
        this.characterType = characterType;
        this.playerIndex = playerIndex;
    }
}