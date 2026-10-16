using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance { get; private set; }

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    [Header("UI References")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            LoadVolumes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        BindSliders();
        ApplyVolumes();
    }

    void OnEnable()
    {
        BindSliders();
    }

    void OnDisable()
    {
        UnbindSliders();
    }

    private void BindSliders()
    {
        if (masterSlider != null)
        {
            masterSlider.value = masterVolume;
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    private void UnbindSliders()
    {
        if (masterSlider != null)
            masterSlider.onValueChanged.RemoveListener(SetMasterVolume);

        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(MASTER_KEY, masterVolume);
        ApplyVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(MUSIC_KEY, musicVolume);
        ApplyVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SFX_KEY, sfxVolume);
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        // Global master volume via AudioListener
        AudioListener.volume = masterVolume;

        // Apply category-specific volumes to all active audio sources
        AudioSource[] allSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in allSources)
        {
            if (IsMusicSource(source))
            {
                source.volume = musicVolume;
            }
            else
            {
                source.volume = sfxVolume;
            }
        }
    }

    private bool IsMusicSource(AudioSource source)
    {
        // Heuristic: longer clips or looping sources are likely music
        if (source.clip != null)
        {
            if (source.clip.length > 10f || source.loop)
                return true;
        }

        // Check if attached to known music managers
        if (source.GetComponent<MenuMusic>() != null)
            return true;
        if (source.GetComponent<PlayerSound>() != null && source.loop)
            return true;
        if (source.GetComponent<BossMusic>() != null)
            return false; // Boss music is typically SFX/combat

        return false;
    }

    private void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
    }

    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;

    // Call this from other scripts to register dynamically created audio sources
    public void RegisterAudioSource(AudioSource source)
    {
        if (source == null) return;

        if (IsMusicSource(source))
            source.volume = musicVolume;
        else
            source.volume = sfxVolume;
    }
}

