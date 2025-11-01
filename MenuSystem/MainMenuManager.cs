// MainMenuManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        // Hook up button events
        newGameButton.onClick.AddListener(OnNewGame);
        continueButton.onClick.AddListener(OnContinue);
        optionsButton.onClick.AddListener(OnOptions);
        quitButton.onClick.AddListener(OnQuit);

        // Disable Continue if no autosave found
        continueButton.interactable = SaveManager.Instance.HasSave("autosave");
    }


    private void OnDestroy()
    {
        // Remove listeners to prevent leaks
        newGameButton.onClick.RemoveListener(OnNewGame);
        continueButton.onClick.RemoveListener(OnContinue);
        optionsButton.onClick.RemoveListener(OnOptions);
        quitButton.onClick.RemoveListener(OnQuit);
    }


    private void OnNewGame()
    {
        // Delete autosave and begin game
        SaveManager.Instance.DeleteSave("autosave");

        LoadSceneWithLoading("Game");
    }


    private void OnContinue()
    {
        // Load saved game; you can add a load indicator here
        LoadSceneWithLoading("Game");
    }


    private void OnOptions()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }


    public void OnCloseOptions()
    {
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }


    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void LoadSceneWithLoading(string sceneName)
    {
        PendingLoad.SetNextScene(sceneName);
        SceneLoader.Instance.LoadScene("Loading");
    }
}
