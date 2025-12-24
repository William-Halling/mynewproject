using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public sealed class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public bool IsLoading { get; private set; }
    public float LoadingProgress { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void LoadScene(string sceneName)
    {
        if (IsLoading)
        {
            Debug.LogWarning("[SceneLoader] Scene load already in progress.");

            return;
        }

        StartCoroutine(LoadRoutine(sceneName));
    }


    private IEnumerator LoadRoutine(string sceneName)
    {
        IsLoading = true;
        LoadingProgress = 0f;

        GameEventBus.Instance?.Publish(GameEventType.SceneLoadStarted, sceneName);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;


        while (op.progress < 0.9f)
        {
            LoadingProgress = Mathf.Clamp01(op.progress / 0.9f);
        
            yield return null;
        }

        op.allowSceneActivation = true;


        while (!op.isDone)
        {
            yield return null;
        }
        
        
        IsLoading = false;
        LoadingProgress = 1f;
        
        GameEventBus.Instance?.Publish(GameEventType.SceneLoaded, sceneName);
    }
}
