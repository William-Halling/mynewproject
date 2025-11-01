// GameSettings.cs
using UnityEngine;
using System.IO;


[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    public float masterVolume = 0.8f;
    public bool fullscreen = true;
    public int resolutionIndex = 0;
    public int qualityLevel = 2;


    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "settings.json");


    public void Apply()
    {
            // Apply audio
        AudioListener.volume = masterVolume;
           
            // Apply resolution and full screen
        Resolution res = Screen.resolutions[Mathf.Clamp(resolutionIndex, 0, Screen.resolutions.Length - 1)];
        Screen.SetResolution(res.width, res.height, fullscreen);
            
            // Apply quality
        QualitySettings.SetQualityLevel(qualityLevel);
    }


    public void Save()
    {
        File.WriteAllText(SavePath, JsonUtility.ToJson(this, prettyPrint: true));
    }


    public void Load()
    {
        if (File.Exists(SavePath))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(SavePath), this);
        }

        
        Apply();
    }
}
