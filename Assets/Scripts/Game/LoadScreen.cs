using System.Collections;
using UnityEngine;

public class LoadScreen : MonoBehaviour
{
    public Animator ani;
    public Animator thumbani;
    public float loadtime = 10f;
    private void OnEnable()
    {
        this.gameObject.SetActive(true);
    }

    private void Start()
    {

        StartCoroutine(loadSceneTime(loadtime));
    }

    public IEnumerator loadSceneTime(float loadTime)
    {
        // Chờ 1 frame để Animator chuẩn bị
        yield return null;
        if (ani != null)
        {
            ani.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        if (thumbani != null)
        {
            thumbani.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        Time.timeScale = 0f;

        if (ani != null)
        {
            ani.Play("loading", 0, 0f); // chạy từ đầu 100%
        }
        if (thumbani != null)
        {
            thumbani.Play("thumbnail1Ani", 0, 0f); // chạy từ đầu 100%
        }
        yield return new WaitForSecondsRealtime(loadTime);
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }
}
