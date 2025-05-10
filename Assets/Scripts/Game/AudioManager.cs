using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    // [SerializeField] AudioSource SFXSource;
    public AudioClip background;
    // public AudioClip death;
    // public AudioClip checkpoint;
    // public AudioClip wallTouch;
    // public AudioClip portalIn;
    // public AudioClip portalOut;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void playBackGMusic(){
        try{
            musicSource.clip = background;
            musicSource?.Play();
        }catch(Exception e){
            Debug.Log("Không thể chơi nhạc nền: " + e.Message);
        }

    }
}
