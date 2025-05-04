using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class SelectCharaterUI : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    [Header("Choosing")]
    public Image PlayerCharacter;//đổi thành position
    public Button changeUpBtn;
    public Button changeDownBtn;
    public TextMeshProUGUI PlayerIndex;
    public int currentCharractIndex = 0;
    public PlayerJoinManager playerJM;
    public List<Color> dsColor = new List<Color>();
    public List<Button> dsButtonColor = new List<Button>();
    public Button sellectBtn;

    [Header("All Players")]
    //public List<GameObject> PlayersInScreen = new List<GameObject>();
    public Button resetBtn;
    public List<Button> playerBtn;
    public GameObject selectedPlayerBtn;
    public Button turnOffUIbtn;
    private void OnEnable()
    {
        ResetPlayersList();
    }
    private void Awake()
    {
        if(!playerJM) playerJM = FindAnyObjectByType<PlayerJoinManager>();
        this.gameObject.SetActive(false);
    }
    private void Start()
    {
        PlayerCharacter.sprite = sprites[currentCharractIndex];
        changeUpBtn.onClick.AddListener(() => { changeCharacteBTN(1); });
        changeDownBtn.onClick.AddListener(() => { changeCharacteBTN(-1); });
        resetBtn.onClick.AddListener(ResetPlayersList);
        sellectBtn.onClick.AddListener(chooseCharacter);
        for (int i = 0; i < playerBtn.Count; i++)
        {
            int index = i;
            playerBtn[i].onClick.AddListener(() => { selectCharacter(index); });
        }
        for (int i = 0; i< dsButtonColor.Count; i++)
        {
            int index = i;
            dsButtonColor[i].onClick.AddListener(() => { selectColor(index); });
        }
        turnOffUIbtn.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }
    public void ResetPlayersList()
    {
        for (int i = 0; i < playerJM.playerDevicesInfo.Count; i++)
        {
            playerBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = "Player" + playerJM.playerDevicesInfo[i].playerIndex;
            playerBtn[i].transform.Find("plimg").GetComponent<Image>().sprite = sprites[playerJM.playerDevicesInfo[i].characterType];
            //Debug.Log(playerJM.playerDevicesInfo[i].playerIndex+"__ "+ playerJM.playerDevicesInfo[i].characterType);
        }
         selectCharacter(0);
    }
    private void changeCharacteBTN(int i)
    {
        if (i > 0)
        {
            currentCharractIndex++;
            if (currentCharractIndex > 5)
            {
                currentCharractIndex = 0;
            }
            PlayerCharacter.sprite = sprites[currentCharractIndex];
        }
        else
        {
            currentCharractIndex--;
            if (currentCharractIndex < 0)
            {
                currentCharractIndex = 5;
            }
            PlayerCharacter.sprite = sprites[currentCharractIndex];

        }
    }

    public void selectCharacter(int i)//chọn chưa chốt
    {
        if (playerBtn[i].GetComponentInChildren<TextMeshProUGUI>().text == "NULL") return;
        if (playerBtn[i].gameObject.transform.Find("plimg").GetComponent<Image>().sprite )
        {
            
            PlayerCharacter.sprite = playerBtn[i].gameObject.transform.Find("plimg").GetComponent<Image>().sprite;// có thể bỏ
            PlayerIndex.text = playerBtn[i].GetComponentInChildren<TextMeshProUGUI>().text;
            selectedPlayerBtn = playerBtn[i].gameObject;
            try
            {
                playerAvatar ava = playerJM.danhsach[i].GetComponentInChildren<playerAvatar>();
                currentCharractIndex = ava.index;
            }
            catch (ArgumentOutOfRangeException) {
                loadAllPlayerOnScreen();
                Debug.Log("chua ton tai!!");
                return;
            }
            //Debug.Log("hể");
        }
    }
    public void loadAllPlayerOnScreen()
    {
        PlayerHealth[] screenPlayer = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
        Debug.Log("load!!");
        var playerlist = PlayerJoinManager.instance.danhsach;
        for (int i = 0; i < screenPlayer.Length; i++)
        {
            if (!playerlist.Contains(screenPlayer[i].gameObject))
            {
                playerlist.Add(screenPlayer[i].gameObject);
                Debug.Log("Thêm player lỗi");

            }
        }
    }


    public void chooseCharacter()
    {
        if (selectedPlayerBtn == null || selectedPlayerBtn.GetComponentInChildren<TextMeshProUGUI>().text=="NULL") return;
        selectedPlayerBtn.transform.Find("plimg").GetComponent<Image>().sprite= sprites[currentCharractIndex];
        int dex = playerBtn.IndexOf(selectedPlayerBtn.GetComponent<Button>());
        selectedPlayerBtn.transform.Find("plimg").GetComponent<Image>().color = PlayerCharacter.GetComponent<Image>().color;
        try {
            CharacterSkillManager chaSkillMana = playerJM.danhsach[dex].GetComponentInChildren<CharacterSkillManager>();
            playerAvatar ava = playerJM.danhsach[dex].GetComponentInChildren<playerAvatar>();
            ava.plcl = PlayerCharacter.GetComponent<Image>().color;
            ava?.setAva(currentCharractIndex);
            playerJM.playerDevicesInfo[dex].characterType = currentCharractIndex;
            playerJM.playerDevicesInfo[dex].playercolor = PlayerCharacter.GetComponent<Image>().color;
            chaSkillMana.currentCharacter =ava.GetComponentInChildren<ImpactOnPlayer>().whichPlayer;
            chaSkillMana.cooldownTimers[0] = chaSkillMana.currentCharacter.skills[0].cooldown;
            chaSkillMana.cooldownTimers[1] = chaSkillMana.currentCharacter.skills[1].cooldown;
            //Debug.Log(PlayerCharacter.GetComponent<Image>().color);
        }
        catch (ArgumentOutOfRangeException  )
        {
            Debug.Log("k toonf tai!!");
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("skill2 k cos");
        }
        //đổi characterType
    }
    public void selectColor(int i)//chọn chưa chốt
    {
        PlayerCharacter.GetComponent<Image>().color = dsColor[i];
    }
    public void changeColor()
    {
        if (selectedPlayerBtn == null) return;
        
    }
}
