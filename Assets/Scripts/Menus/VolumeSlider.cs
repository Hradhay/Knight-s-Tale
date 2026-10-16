using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType
    {
        Master,
        Music,
        SFX
    }

    [Header("Volume Type")]
    [SerializeField] private VolumeType volumeType = VolumeType.Master;

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("VolumeSlider: No Slider component found on this GameObject.");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        // Initialize slider to current volume
        float currentValue = volumeType switch
        {
            VolumeType.Master => VolumeManager.Instance != null ? VolumeManager.Instance.GetMasterVolume() : 1f,
            VolumeType.Music => VolumeManager.Instance != null ? VolumeManager.Instance.GetMusicVolume() : 1f,
            VolumeType.SFX => VolumeManager.Instance != null ? VolumeManager.Instance.GetSFXVolume() : 1f,
            _ => 1f
        };

        slider.value = currentValue;

        // Add listener
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (VolumeManager.Instance == null) return;

        switch (volumeType)
        {
            case VolumeType.Master:
                VolumeManager.Instance.SetMasterVolume(value);
                break;
            case VolumeType.Music:
                VolumeManager.Instance.SetMusicVolume(value);
                break;
            case VolumeType.SFX:
                VolumeManager.Instance.SetSFXVolume(value);
                break;
        }
    }
}

