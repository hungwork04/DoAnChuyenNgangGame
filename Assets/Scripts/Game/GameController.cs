using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button resetGamebtn;
    public Button menuGamebtn;

    private void Start()
    {
        resetGamebtn.onClick.AddListener(() => { SceneManager.LoadScene(1); });
        menuGamebtn.onClick.AddListener(() => { SceneManager.LoadScene(0); });
    }
    public void loadSceneZero()
    {
        var playerInputManager = FindObjectOfType<PlayerInputManager>();
        //var Gamedata = FindObjectOfType<GameDataManager>();
        if (playerInputManager != null)
        {
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;

            //Debug.Log("Chuyển sang chế độ Join Players Manually.");
        }
        
        //StartCoroutine(SetJoinPlayerManually());
        SceneManager.LoadScene(0);
    }
}
