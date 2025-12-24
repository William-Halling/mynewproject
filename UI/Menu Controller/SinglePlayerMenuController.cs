using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SinglePlayerMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button btnNewGame;
    [SerializeField] private Button btnContinue;
    [SerializeField] private Button btnLoadGame;
    [SerializeField] private Button btnBack;
    
 private void Awake()
    {
        // Validate FIRST
        if (btnNewGame == null || btnContinue == null || btnLoadGame == null || btnBack == null)
        {
            UnityEngine.Debug.LogError("[SinglePlayerMenuController] Missing button references. Check the Inspector.");
            enabled = false;
            return;
        }

        // Then bind
        Bind(btnNewGame, OnNewGame);
        Bind(btnContinue, OnContinue);
        Bind(btnLoadGame, OnLoadGame);
        Bind(btnBack, MenuRouter.Instance.Back);
    }

    private void OnEnable()
    {
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        bool hasAnySave =
            SaveManager.Instance != null &&
            SaveManager.Instance.HasAnySave();

        btnContinue.interactable = hasAnySave;
        btnLoadGame.interactable = hasAnySave;
    }

    private void OnNewGame()
    {
        if (GameManager.Instance == null)
        {
            UnityEngine.Debug.LogError("[SinglePlayerMenuController] GameManager.Instance is null");

            return;
        }

        GameManager.Instance.StartNewGame();
    }


    private void OnContinue()
    {
        GameManager.Instance.ContinueGame();
    }


    private void OnLoadGame()
    {
        // Temporary behavior until load panel exists
        GameManager.Instance.ContinueGame();
    }


    private void Bind(Button button, UnityEngine.Events.UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}