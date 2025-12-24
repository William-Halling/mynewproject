using System;
using UnityEngine;
using Transporter.Data;
using Transporter.Gameplay;

public sealed class GameManager : MonoBehaviour
{
    public  static GameManager Instance { get; private set; }
    private static GameData    pendingLoadData;


    [SerializeField] private GameState currentState = GameState.MainMenu;
    public GameState CurrentState => currentState;

    private const string AUTOSAVE_SLOT = "autosave";


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
        GameEventBus.Instance?.AddListener(GameEventType.SceneLoaded, OnSceneLoaded);
        Debug.Log("GameManager Awake");
    }


    public void StartNewGame()
    {
        pendingLoadData = new GameData
        {
            sceneName = "Game_Main",
            version = Application.version,
            saveTime = DateTime.Now,
            playerData = new PlayerData(),
            worldData = new WorldData { seed = UnityEngine.Random.Range(0, 999999) }
        };

        SetState(GameState.Loading);
        SceneLoader.Instance.LoadScene("Game_Main");
    }


    public void ContinueGame()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("[GameManager] SaveManager not available.");

            return;
        }

        SaveIndex index = SaveManager.Instance.GetIndexSnapshot();
        SaveSlotMeta meta = index.GetMostRecent();

        if (meta == null)
        {
            Debug.Log("No save available to continue.");

            return;
        }

        LoadGame(meta.slotId);
    }


    public void LoadGame(string slot)
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("[GameManager] SaveManager missing");

            return;
        }

        pendingLoadData = SaveManager.Instance.LoadGameData(slot);


        if (pendingLoadData == null)
        {
            Debug.LogWarning($"[GameManager] No save found for slot: {slot}");

            return;
        }

        SceneLoader.Instance.LoadScene(pendingLoadData.sceneName);
        SetState(GameState.Loading);
    }


    private void OnSceneLoaded(object payload)
    {
        if (pendingLoadData == null)
        {
            return;
        }

        WorldManager.Instance?.LoadState(pendingLoadData.worldData);
        PlayerManager.Instance?.LoadState(pendingLoadData.playerData);

        pendingLoadData = null;
        SetState(GameState.Play);
    }


    public void TogglePause()
    {
        if (currentState == GameState.Play)
        {
            SetState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            SetState(GameState.Play);
        }
    }


    public void SaveGame(string slotId)
    {
        GameData data = new GameData
        {
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            version = Application.version,
            saveTime = DateTime.Now,
            playerData = PlayerManager.Instance.SaveState(),
            worldData = WorldManager.Instance.SaveState()
        };

        SaveManager.Instance.SaveSlot( slotId, "Autosave", data);
    }
    

    [SerializeField] private bool isMultiplayer;
    public void SetMultiplayerMode(bool value)
    {
        isMultiplayer = value;
    }


    private void SetState(GameState newState)
    {
        currentState = newState;
        Time.timeScale = newState == GameState.Paused ? 0f : 1f;
        GameEventBus.Instance?.Publish(GameEventType.GameStateChanged, newState);  
    }
}
