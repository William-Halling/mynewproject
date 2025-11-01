using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public float loadingProgress { get; private set; }
    public bool isLoading { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Register with Service Locator
        ServiceLocator.Instance?.RegisterService(this);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;
        loadingProgress = 0f;

        // Notify systems that a scene load is starting
        GameEventBus.Instance.Publish(GameEventType.SceneLoadStarted, sceneName);

        // Set game state to loading (NO GameManager.CurrentState calls)
        GameStateService.Instance?.Apply(GameState.Loading);

        // Explicitly use UnityEngine.AsyncOperation to avoid ambiguity
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            loadingProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (asyncLoad.progress >= 0.9f)
            {
                // Wait for final approval to activate the scene
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingProgress = 1f;
        isLoading = false;

        // Notify systems that the scene has loaded
        GameEventBus.Instance.Publish(GameEventType.SceneLoaded, sceneName);

        // Option A: let SceneLoader set the final state (simple)
        if (sceneName == "MainMenu")
            GameStateService.Instance?.Apply(GameState.MainMenu);
        else
            GameStateService.Instance?.Apply(GameState.Play);

        // Option B (preferred SRP): remove the block above and let a SceneFlow/Manager
        // listener set the final state in response to GameEventType.SceneLoaded.
    }
}
