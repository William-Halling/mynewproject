using UnityEngine;

public sealed class SceneFlowService : MonoBehaviour
{
    public static SceneFlowService Instance { get; private set; }

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


    private void OnEnable()
    {
        GameEventBus.Instance?.AddListener(GameEventType.SceneLoaded, OnSceneLoaded);
    }


    private void OnDisable()
    {
        GameEventBus.Instance?.RemoveListener(GameEventType.SceneLoaded, OnSceneLoaded);
    }


    public void GoToMainMenu()
    {
        PendingLoad.SetNextScene(Scenes.MainMenu);
        GameStateService.Instance.Apply(GameState.Loading);
        SceneLoader.Instance.LoadScene(Scenes.Loading);
    }


    public void GoToGame()
    {
        PendingLoad.SetNextScene(Scenes.Game);
        GameStateService.Instance.Apply(GameState.Loading);
        SceneLoader.Instance.LoadScene(Scenes.Loading);
    }


    private void OnSceneLoaded(object payload)
    {
        string scene = payload as string ?? string.Empty;
        
        if(scene == Scenes.MainMenu) 
        {
            GameStateService.Instance.Apply(GameState.MainMenu);
        }
        else if(scene == Scenes.Game)     
        {
            GameStateService.Instance.Apply(GameState.Play);
        }
        // Loading is intermediate; we set Loading before we open it
    }
}
