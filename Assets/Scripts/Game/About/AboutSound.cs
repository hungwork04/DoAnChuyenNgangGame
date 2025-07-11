using System;
using UnityEngine;
using System.Collections;
public class AboutSound : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    public AudioClip background;
    public static AboutSound instance;
    public float duration=35;
    public void playAboutMusic()
    {
        try
        {
            musicSource.clip = background;
            musicSource.volume = 1f; // Bắt đầu với âm lượng đầy đủ
            musicSource.Play();
            StartCoroutine(FadeOutMusicAfterDelay(duration, 11f)); // Sau 5s bắt đầu giảm dần trong 3s
        }
        catch (Exception e)
        {
            Debug.Log("Không thể chơi nhạc nền: " + e.Message);
        }
    }

    void Start()
    {
        playAboutMusic();
    }

    private IEnumerator FadeOutMusicAfterDelay(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop(); // Xoá dòng này nếu chỉ muốn giảm âm lượng mà không dừng nhạc
    }
}
