using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource source2D;

    private bool musicFlag = false;

    public string[] Sounds = new string[]
    {
        "laser",
        "boom",
        "impact",
        "hurt",
        "click",
        "lvlup",
    };

    [SerializeField]
    private List<AudioClip> audioArray;

    public string[] Musics = new string[]
    {
        "MainMenu",
        "InGame",
        "GameOver",
        "Win",
    };

    [SerializeField]
    private List<AudioClip> musicArray;

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("le sound manager est null");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        PlayBGMusic();
        musicFlag = musicSource.isPlaying;
    }

    private void Update()
    {
    }

    public void Play(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i] == name)
            {
                source.PlayOneShot(audioArray[i]);
            }
        }
    }

    public void Play2D(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i] == name)
            {
                source2D.PlayOneShot(audioArray[i]);
            }
        }
    }

    public AudioClip GetAudioCLip(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i] == name)
            {
                return audioArray[i];
            }
        }
        return null;
    }

    public void PlayBGMusic()
    {
        string musicToPlay = "";
        switch (GameManager.Instance.GameState)
        {
            case GameManager.gameState.MainMenu:
                musicToPlay = "MainMenu";
                break;
            case GameManager.gameState.InGame:
                musicToPlay = "InGame";
                break;
            case GameManager.gameState.GameOver:
                musicToPlay = "GameOver";
                break;
            case GameManager.gameState.Win:
                musicToPlay = "Win";
                break;
        }
        if (musicFlag == false)
        {
            musicFlag = true;
            for (int i = 0; i < Musics.Length; i++)
            {
                if (Musics[i] == musicToPlay)
                {
                    musicSource.PlayOneShot(musicArray[i]);
                }
            }
        }
    }

    public void StopBGMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
            musicFlag = false;
        }
    }

    public void AutomaticMusicChange()
    {
        StopBGMusic();
        PlayBGMusic();
    }
}
