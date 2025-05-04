using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameCtrl : MonoBehaviour
{
    public Button playAgainbtn;
    public Button ExitToMenubtn;
    public GameObject endgameUI;
    public static EndGameCtrl instance;
    // Start is called before the first frame update
    public List<GameObject> players;
    [SerializeField]private bool gameStopped = false;
    private void Start()
    {
        instance = this;
        endgameUI.SetActive(false);
        playAgainbtn.onClick.AddListener(() => { 
            SceneManager.LoadScene(1);
            Time.timeScale = 1f;
        });
        ExitToMenubtn.onClick.AddListener(() => { 
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        });
    }
    
    private void Update()
    {
        if (players.Count <= 1 && gameStopped == false)
        {
            StartCoroutine(StopGameAfterDelay(0.75f));
        }
    }
    private IEnumerator StopGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Debug.LogWarning("hể");
        endgameUI?.SetActive(true);
        //Time.timeScale = 0f;
        gameStopped = true;
    }
}
