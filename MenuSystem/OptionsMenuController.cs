// OptionsMenuController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private GameSettings settings;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;


    private Resolution[] resolutions;


    private void Awake()
    {
        settings.Load();

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();


        for (int i = 0; i < resolutions.Length; i++)
        {
            var res = resolutions[i];

            options.Add($"{res.width} x {res.height} @ {res.refreshRate}Hz");
        }
        resolutionDropdown.AddOptions(options);


            // Populate quality dropdown
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));


            // Set initial UI values
        volumeSlider.value = settings.masterVolume;
        fullscreenToggle.isOn = settings.fullscreen;
        resolutionDropdown.value = settings.resolutionIndex;
        qualityDropdown.value = settings.qualityLevel;


            // Add listeners
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }


    private void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
        
        resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
        qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);
    }


    private void OnVolumeChanged(float value)
    {
        settings.masterVolume = value;

        settings.Apply();
        settings.Save();
    }


    private void OnFullscreenChanged(bool value)
    {
        settings.fullscreen = value;

        settings.Apply();
        settings.Save();
    }


    private void OnResolutionChanged(int index)
    {
        settings.resolutionIndex = index;

        settings.Apply();
        settings.Save();
    }


    private void OnQualityChanged(int index)
    {
        settings.qualityLevel = index;

        settings.Apply();
        settings.Save();
    }


    public void OnBack()
    {
        settings.Save();

        gameObject.SetActive(false);
    }
}
