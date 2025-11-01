using System;
using System.IO;
using Transporter.Data;
using UnityEngine;


public sealed class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }


        // Steam integration placeholder (wire up Steamworks.NET later)
    [SerializeField] private bool useSteamCloud = false;
    private const uint STEAM_APP_ID = 0; // set your real AppID when ready


        // Events
    public event Action<string> OnGameSaved;
    public event Action<string> OnGameLoaded;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            
            return; 
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ServiceLocator.Instance?.RegisterService(this);

            // Placeholder: decide if Steam Cloud should be used
        useSteamCloud = CheckForSteam();
    }


    private bool CheckForSteam()
    {
        // TODO: Replace with Steamworks.NET init check (e.g., SteamAPI.Init()).
        // Return true if Steam is running + user logged in for this app.
        return false;
    }


    // -------------------------------------------------------
    // Public API
    // -------------------------------------------------------

    public bool HasSave(string slotName)
    {
        string path = GetSavePath(slotName);

        return System.IO.File.Exists(path);
    }


    public void SaveGame(string slotName)
    {
        var data = CreateSaveData();

        if (useSteamCloud)
        { 
            SaveToSteamCloud(data, slotName); // placeholder
        }
        else
        { 
            SaveLocally(data, slotName);
        }

        OnGameSaved?.Invoke(slotName);
    }


    public GameData LoadGame(string slotName)
    {
        GameData loaded = useSteamCloud
            ? LoadFromSteamCloud(slotName) // placeholder
            : LoadLocally(slotName);


        if (loaded != null)
        { 
            OnGameLoaded?.Invoke(slotName);
        }

        return loaded;
    }


    public void DeleteSave(string slotName)
    {
        var path = GetSavePath(slotName);
        

        if (File.Exists(path))
        {
            File.Delete(path);

            Debug.Log($"[SaveManager] Deleted save: {slotName}");
        }
    }


        // -------------------------------------------------------
        // Local (JSON) persistence
        // -------------------------------------------------------

    private void SaveLocally(GameData data, string slotName)
    {
        try
        {
            var json = JsonUtility.ToJson(data, prettyPrint: true);
            var path = GetSavePath(slotName);
            Directory.CreateDirectory(Path.GetDirectoryName(path));


            File.WriteAllText(path, json);
            Debug.Log($"[SaveManager] Saved → {path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] Save failed: {e}");
        }
    }


    private GameData LoadLocally(string slotName)
    {
        try
        {
            var path = GetSavePath(slotName);

            if (!File.Exists(path))
            {
                Debug.LogError($"[SaveManager] Save file not found: {path}");

                return null;
            }

            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<GameData>(json);
            Debug.Log($"[SaveManager] Loaded ← {slotName}");


            return data;
        }


        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] Load failed: {e}");

            return null;
        }
    }


    private static string GetSavePath(string slotName)
    {
            // Keep saves in a dedicated folder; JSON extension for clarity
        return Path.Combine(UnityEngine.Application.persistentDataPath, $"saves/{slotName}.json");
    }


    private GameData CreateSaveData()
    {
            // Minimal snapshot to match your current behavior.
            // (We can hydrate from live systems later.)
        return new GameData
        {
            playerData = new PlayerData(),
            worldData = new WorldData(),
            saveTime = DateTime.Now,
            version = UnityEngine.Application.version
        };
    }


        // -------------------------------------------------------
        // Steam Cloud placeholders (safe no-ops for now)
        // -------------------------------------------------------

    private void SaveToSteamCloud(GameData data, string slotName)
    {
        Debug.Log($"[SaveManager] (Steam Cloud placeholder) Would save '{slotName}' to Steam Cloud.");

            // Until Steamworks.NET is wired, still persist locally as a backup
        SaveLocally(data, slotName);
    }


    private GameData LoadFromSteamCloud(string slotName)
    {
        Debug.Log($"[SaveManager] (Steam Cloud placeholder) Would load '{slotName}' from Steam Cloud.");

            // Until Steamworks.NET is wired, fall back to local
        return LoadLocally(slotName);
    }
}
