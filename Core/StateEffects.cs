using UnityEngine;

public sealed class StateEffects : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ServiceLocator.Instance?.RegisterService(this);
    }


    private void OnEnable()
    {
        GameStateService.Instance.OnStateChanged += Handle;
    }


    private void OnDisable()
    {
        if (GameStateService.Instance != null)
            GameStateService.Instance.OnStateChanged -= Handle;
    }


    private void Handle(GameState state)
    {
        switch (state)
        {
            case GameState.Play:
                Time.timeScale = 1f;
                SetCursor(false);
                SetInput(true);
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                SetCursor(true);
                SetInput(false);
                break;

            case GameState.MainMenu:
                Time.timeScale = 1f;
                SetCursor(true);
                SetInput(false);
                break;

            case GameState.Loading:
                Time.timeScale = 1f; // keep anims/progress alive
                SetCursor(true);
                SetInput(false);
                break;

            case GameState.Quit:
                Time.timeScale = 1f;
                break;
        }
    }


    private static void SetCursor(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible   = visible;
    }


    private static void SetInput(bool enabled)
    {
        if (InputManager.Instance == null) return;
        if (enabled) InputManager.Instance.EnableInput();
        else         InputManager.Instance.DisableInput();
    }
}
