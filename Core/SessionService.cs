using System;
using UnityEngine;
using Transporter.Data;

public sealed class SessionService : MonoBehaviour
{
    public static SessionService Instance { get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this) 
        { 
            Destroy(gameObject); 
            
            return; 
        }

        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ServiceLocator.Instance?.RegisterService(this);
    }


    public void StartNewGame()
    {
        var data = new GameData
        {
            playerData = new PlayerData { playerName = "Captain", credits = 1000f, position = Vector3.zero },
            worldData  = new WorldData  { worldSeed = UnityEngine.Random.Range(0, 1_000_000), gameTime = 0f },
            saveTime   = DateTime.Now,
            version    = Application.version
        };


        if (WorldClock.Instance) 
        {
            WorldClock.Instance.SetGameTime(0f);
        }
        SceneFlowService.Instance.GoToGame();
    }


    public void LoadGame(string slot)
    {
        var loaded = SaveManager.Instance?.LoadGame(slot);
        
        if (loaded == null) 
        {
            return;
        }

        ApplyLoadedData(loaded);
        SceneFlowService.Instance.GoToGame();
    }


    public void QuitToMainMenu() => SceneFlowService.Instance.GoToMainMenu();

    public void ExitGame()
    {
        SaveManager.Instance?.SaveGame("autosave");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    private static void ApplyLoadedData(GameData data)
    {
        if (WorldClock.Instance)
        {
         WorldClock.Instance.SetGameTime(data.worldData.gameTime);
        }
            // TODO: hydrate other systems (player pos, inventory, economy snapshots)
    }
}
