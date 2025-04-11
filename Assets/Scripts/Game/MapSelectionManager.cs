using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectionManager : MonoBehaviour
{
    public GameObject mapSelectionPanel;
    public GameObject loadingUI;


    public void LoadMap(int mapIndex)
    {
        GameDataManager.instance.gameStarted = true;
        StartCoroutine(LoadSceneWithLoading(mapIndex));
    }

    private IEnumerator LoadSceneWithLoading(int mapIndex)
    {
        loadingUI.SetActive(true);
        DontDestroyOnLoad(loadingUI);
        yield return new WaitForSecondsRealtime(3.7f);
        Time.timeScale = 0;
        SceneManager.LoadScene(mapIndex);
        Time.timeScale = 1;
    }

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