using UnityEngine;


public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState currentState = GameState.MainMenu;
    [SerializeField] private bool handlePauseKey = true;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    
    public GameState CurrentState
    {
        get => currentState;
        set => ApplyState(value);
    }


    private void Awake()
    {
        if (Instance && Instance != this) 
        { 
            Destroy(gameObject); 
            
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void OnEnable()  => GameEventBus.Instance?.AddListener(GameEventType.SceneLoaded, OnSceneLoaded);
    void OnDisable() => GameEventBus.Instance?.RemoveListener(GameEventType.SceneLoaded, OnSceneLoaded);


    private void Update()
    {
        if (!handlePauseKey || currentState == GameState.Loading) 
        {
            return;
        }

        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }


    public void TogglePause()=>
        ApplyState(currentState == GameState.Play ? GameState.Paused : GameState.Play);


    // Simple pass-throughs for UI buttons (SRP preserved)
    public void UI_StartNewGame()    => SessionService.Instance.StartNewGame();
    public void UI_LoadAutosave()    => SessionService.Instance.LoadGame("autosave");
    public void UI_QuitToMainMenu()  => SessionService.Instance.QuitToMainMenu();
    public void UI_ExitGame()        => SessionService.Instance.ExitGame();


    public void StartNewGame()
    {
        WorldClock.Instance?.SetGameTime(0f);     // seed world time
        PendingLoad.SetNextScene(Scenes.Game);

        ApplyState(GameState.Loading);
        SceneLoader.Instance?.LoadScene(Scenes.Loading);
    }


    public void LoadGame(string slotName = "autosave")
    {
        PendingLoad.SetNextScene(Scenes.Game);
        ApplyState(GameState.Loading);
        SceneLoader.Instance?.LoadScene(Scenes.Loading);
    }


    public void QuitToMainMenu()
    {
        PendingLoad.SetNextScene(Scenes.MainMenu);
        ApplyState(GameState.Loading);
        SceneLoader.Instance?.LoadScene(Scenes.Loading);
    }


    public void ExitGame()
    {
        SaveManager.Instance?.SaveGame("autosave");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnSceneLoaded(object payload)
    {
        var scene = payload as string ?? string.Empty;

        if (scene == Scenes.MainMenu) 
        {
            ApplyState(GameState.MainMenu);
        }
        
        else if (scene == Scenes.Game) 
        {
            ApplyState(GameState.Play);
        }
    }


    private void ApplyState(GameState newState)
    {
        if (currentState == newState) 

            return;

        currentState = newState;
        GameEventBus.Instance?.InvokeEvent(GameEventType.GameStateChanged, newState);


        switch (newState)
        {
            case GameState.Play:
                Time.timeScale = 1f; SetCursor(false); SetInput(true);  break;
            case GameState.Paused:
                Time.timeScale = 0f; SetCursor(true);  SetInput(false); break;
            case GameState.Loading:
                Time.timeScale = 1f; SetCursor(true);  SetInput(false); break;
            case GameState.MainMenu:
                Time.timeScale = 1f; SetCursor(true);  SetInput(false); break;
            case GameState.Quit:
                Time.timeScale = 0.5f; break;
        }
    }


    private static void SetCursor(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }


    private static void SetInput(bool enabled)
    {
        if (!InputManager.Instance) 
        {
            return;
        }
        if (enabled) 
        {
            InputManager.Instance.EnableInput(); 
        }   
        else 
        {
            InputManager.Instance.DisableInput();
        }
    }
}
