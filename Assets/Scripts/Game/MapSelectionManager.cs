using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectionManager : MonoBehaviour
{
    public GameObject mapSelectionPanel;
    //public GameObject loadingUI;
    //private void OnEnable()
    //{
    //    loadingUI.GetComponent<Canvas>().worldCamera = Camera.main;
    //}
    //private void Start()
    //{
    //    if (loadingUI)
    //    {
    //        loadingUI.SetActive(false);
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //}

    public void LoadMap(int mapIndex)
    {
        GameDataManager.instance.gameStarted = true;
        SceneManager.LoadScene(mapIndex);
        //StartCoroutine(LoadSceneWithLoading(mapIndex));
    }

    //private IEnumerator LoadSceneWithLoading(int mapIndex)
    //{
    //    loadingUI.SetActive(true);
    //    Time.timeScale = 0;
    //    SceneManager.LoadScene(mapIndex);
    //    yield return new WaitForSecondsRealtime(3.7f);
    //    Time.timeScale = 1;
    //    loadingUI.SetActive(false);
    //    this.gameObject.SetActive(false);
    //    Debug.Log("chay game");
    //}

    public void CloseMapSelection()
    {
        if (mapSelectionPanel != null)
            mapSelectionPanel.SetActive(false);
    }

    public void ShowMapSelection()
    {
        if (mapSelectionPanel != null)
            mapSelectionPanel.SetActive(true);
    }
}