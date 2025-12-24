using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Root UI")]
    [SerializeField] private GameObject pauseUI;

    [Header("Buttons")]
    [SerializeField] private Button btnOptions;
    [SerializeField] private Button btnCustomerService;
    [SerializeField] private Button btnSave;
    [SerializeField] private Button btnLeaveGame;
    [SerializeField] private Button btnReturnToMainMenu;
    [SerializeField] private Button btnReturnToGame;


        // Single source of truth — no guessing, no relying on Time.timeScale
    private bool isPaused = false;


    private void Awake()
    {
            // Hard fail early if setup is wrong
        if (pauseUI == null)
        {
            UnityEngine.Debug.LogError("[PauseMenuManager] PauseUI is NOT assigned. Fix this shit.");
            enabled = false;
        
            return;
        }


            // Start hidden
        pauseUI.SetActive(false);


            // Bind buttons safely (won’t explode if missing)
        SafeBind(btnSave, SaveGame);
        SafeBind(btnReturnToGame, ResumeGame);
        SafeBind(btnReturnToMainMenu, ReturnToMainMenu);
        SafeBind(btnLeaveGame, LeaveGame);
    }


    private void Update()
    {
            // Escape key toggles pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Debug.Log("ESCAPE PRESSED — TOGGLING PAUSE");

            TogglePause();
        }
    }


    private void TogglePause()
    {
        if (isPaused)
        { 
            ResumeGame();
        }
        else
        { 
            PauseGame();
        }
    }


    private void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0f;                  // Freeze the world
        pauseUI.SetActive(true);              // Show menu

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UnityEngine.Debug.Log("GAME PAUSED");
    }



    private void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;                  // Resume time
        pauseUI.SetActive(false);             // Hide menu

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UnityEngine.Debug.Log("GAME RESUMED");
    }



    private void SaveGame()
    {
        UnityEngine.Debug.Log("SAVE GAME CLICKED");

        GameManager.Instance?.SaveGame("autosave");
    }


    private void ReturnToMainMenu()
    {
        UnityEngine.Debug.Log("RETURNING TO MAIN MENU");

        Time.timeScale = 1f;              // unpause time
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }


    private void LeaveGame()
    {
        UnityEngine.Debug.Log("LEAVE GAME");

        Application.Quit();
    }


        // Prevents null crashes and double bindings
    private void SafeBind(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
        {
            UnityEngine.Debug.LogWarning("[PauseMenuManager] A button reference is missing.");

            return;
        }


        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
