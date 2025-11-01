using UnityEngine;


public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private bool handlePauseKey = true;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;


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


    private void Update()
    {
        if (!handlePauseKey) 
        {
            return;
        }
        if (Input.GetKeyDown(pauseKey) && GameStateService.Instance.Current != GameState.Loading)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        var s = GameStateService.Instance.Current;

        GameStateService.Instance.Apply(s == GameState.Play ? GameState.Paused : GameState.Play);
    }


    // Simple pass-throughs for UI buttons (SRP preserved)
    public void UI_StartNewGame()    => SessionService.Instance.StartNewGame();
    public void UI_LoadAutosave()    => SessionService.Instance.LoadGame("autosave");
    public void UI_QuitToMainMenu()  => SessionService.Instance.QuitToMainMenu();
    public void UI_ExitGame()        => SessionService.Instance.ExitGame();
}
