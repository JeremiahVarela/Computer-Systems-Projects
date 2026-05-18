using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;   // <-- add this

public class SimpleGameOptions : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;  // <-- TMP version
    public Slider volumeSlider;   // 0–1

    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;

    void Start()
    {
        SetupResolutions();
        SetupInitialUIValues();
        HookUpEvents();
    }

    void SetupResolutions()
    {
        // Get all supported resolutions, ordered by size
        resolutions = Screen.resolutions
            .OrderBy(r => r.width * r.height)
            .ThenBy(r => r.refreshRateRatio.value)
            .ToArray();

        if (resolutionDropdown == null) return;

        resolutionDropdown.ClearOptions();
        var options = resolutions
            .Select(r => $"{r.width} x {r.height}")
            .ToList();

        // Find current resolution in the list
        Resolution current = Screen.currentResolution;
        currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == current.width &&
                resolutions[i].height == current.height)
            {
                currentResolutionIndex = i;
                break;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void SetupInitialUIValues()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = Screen.fullScreen;

        if (volumeSlider != null)
            volumeSlider.value = AudioListener.volume;
    }

    void HookUpEvents()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);

        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ------- Callbacks -------

    void OnFullscreenToggled(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    void OnResolutionChanged(int index)
    {
        if (resolutions == null || resolutions.Length == 0) return;

        currentResolutionIndex = Mathf.Clamp(index, 0, resolutions.Length - 1);
        Resolution r = resolutions[currentResolutionIndex];

        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }

    void OnVolumeChanged(float value)
    {
        // Slider should be 0–1
        AudioListener.volume = Mathf.Clamp01(value);
    }
}
