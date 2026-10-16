using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic Instance { get; private set; }

    [Header("Menu Music")]
    public AudioClip bgMusicClip;

    [Header("Button SFX")]
    public AudioClip buttonClickClip;

    private AudioSource bgSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            

            bgSource = GetComponent<AudioSource>();
            if (bgSource == null)
                bgSource = gameObject.AddComponent<AudioSource>();

            sfxSource = gameObject.AddComponent<AudioSource>();

            if (bgMusicClip != null)
            {
                bgSource.clip = bgMusicClip;
                bgSource.loop = false;
                bgSource.Play();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // Stop menu music when entering RPG game scene
        if (sceneName.Contains("RPG"))
        {
            if (bgSource != null && bgSource.isPlaying)
            {
                bgSource.Stop();
            }
        }
        // Restart menu music when returning to Main Menu
        else if (sceneName.Contains("Main Menu"))
        {
            if (bgSource != null && !bgSource.isPlaying && bgMusicClip != null)
            {
                bgSource.clip = bgMusicClip;
                bgSource.Play();
            }
        }
    }

    public static void PlayButtonSFX()
    {
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.PauseBgDuringSFX());
        }
    }

    private IEnumerator PauseBgDuringSFX()
    {
        if (bgSource != null && bgSource.isPlaying)
        {
            bgSource.Pause();
        }

        if (buttonClickClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(buttonClickClip);
        }

        yield return new WaitWhile(() => sfxSource != null && sfxSource.isPlaying);

        if (bgSource != null)
        {
            bgSource.UnPause();
        }
    }
}
