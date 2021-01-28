using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource source2D;

    private bool musicFlag = false;

    public enum ClipsTags
    {
        // Sounds
        laser,
        explosion,
        plasmaExplosion,
        hurt,
        click,
        lvlUp,
        heal,

        // Musics

        MainMenu,
        MainGame,
        Win,

    }

    [System.Serializable]
    private struct SoundClips
    {
        public string clipName;
        public AudioClip clip;
    }

    [System.Serializable]
    private struct MusicClips
    {
        public string clipName;
        public AudioClip clip;
    }

    [SerializeField] private List<SoundClips> soundClips;
    [SerializeField] private List<MusicClips> musicClips;

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

    public AudioClip GetAudioClip(ClipsTags searchedClip)
    {
        foreach(SoundClips sound in soundClips)
        {
            if (sound.clipName.Equals(searchedClip.ToString()))
                return sound.clip;
        }

        Debug.LogError(searchedClip + " not found in Audio Clips.");
        return null;
    }

    public void Play2D(ClipsTags searchedClip)
    {
        foreach(SoundClips sound in soundClips)
        {
            if (sound.clipName.Equals(searchedClip.ToString()))
            {
                source2D.PlayOneShot(sound.clip);
                return;
            }
        }

        Debug.LogError(searchedClip + " not found in Audio Clips.");
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
            foreach (MusicClips music in musicClips)
            {
                if (music.clipName.Equals(musicToPlay))
                    musicSource.clip = music.clip;
            }
            if (musicSource.clip != null)
                musicSource.Play();
            else
                Debug.LogError("Music not found for " + "\"" + GameManager.Instance.GameState + "\"" + " state of game.");
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
