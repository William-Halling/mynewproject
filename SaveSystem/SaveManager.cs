using System;
using UnityEngine;
using Transporter.Data;

public sealed class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Header("Storage")]
    [SerializeField] private bool useSteamCloud = false;


        // file keys (inside storage root)
    private const string INDEX_KEY = "save_index.json";

        // local root directory
    private string LocalRoot => System.IO.Path.Combine(Application.persistentDataPath, "saves");

    private ISaveStorage storage;
    private SaveIndex indexCache;


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

        storage = useSteamCloud ? (ISaveStorage) new SteamCloudStorage() : new LocalFileStorage(LocalRoot);

        LoadIndex();
    }


        // -------------------
        // Public API (UI uses these)
        // -------------------
    public bool HasAnySave()
    {
        return indexCache != null && indexCache.slots.Count > 0;
    }


    public bool HasSave(string slotId)
    {
        return storage.Exists(SlotKey(slotId));
    }


    public SaveIndex GetIndexSnapshot()
    {
        var snap = new SaveIndex();

        foreach (var s in indexCache.slots)
        {
            snap.slots.Add(s);
        }

        return snap;
    }


    public SaveSlotMeta GetMeta(string slotId)
    {
        return indexCache.Get(slotId);
    }


    public void DeleteSlot(string slotId)
    {
        storage.Delete(SlotKey(slotId));
        indexCache.Remove(slotId);

        SaveIndexFile();
    }


    public void SaveSlot(string slotId, string displayName, GameData data)
    {
        if (data == null)
        {
            Debug.LogError("[SaveManager] SaveSlot called with null GameData.");
           
            return;
        }


            // Write the full save
        string json = JsonUtility.ToJson(data, true);
        storage.WriteText(SlotKey(slotId), json);


            // Update index metadata
        var meta = SaveSlotMeta.FromGameData(slotId, displayName, data);
        indexCache.Upsert(meta);
        SaveIndexFile();


        OnGameSaved?.Invoke(slotId);
    }


    public GameData LoadGameData(string slotId)
    {
        if (!storage.Exists(SlotKey(slotId)))
        {
            return null;
        }
        
        string json = storage.ReadText(SlotKey(slotId));
        

        if (string.IsNullOrWhiteSpace(json))
        { 
            return null;
        }

        var data = JsonUtility.FromJson<GameData>(json);


        return data;
    }


        // -------------------
        // Internals
        // -------------------
    private string SlotKey(string slotId) => $"{slotId}.json";


    private void LoadIndex()
    {
        indexCache = new SaveIndex();

        if (!storage.Exists(INDEX_KEY))
        {
            SaveIndexFile();

            return;
        }

        string json = storage.ReadText(INDEX_KEY);


        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                indexCache = JsonUtility.FromJson<SaveIndex>(json) ?? new SaveIndex();
            }
            catch
            {
                indexCache = new SaveIndex();
            }
        }
    }


    private void SaveIndexFile()
    {
        string json = JsonUtility.ToJson(indexCache, true);

        storage.WriteText(INDEX_KEY, json);
    }
}
