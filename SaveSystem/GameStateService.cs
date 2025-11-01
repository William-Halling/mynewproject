using System;
using UnityEngine;

public sealed class GameStateService : MonoBehaviour
{
    public static GameStateService Instance { get; private set; }

    [SerializeField] private GameState current = GameState.MainMenu;

    /// <summary>Raised after the state actually changes.</summary>
    public event Action<GameState> OnStateChanged;

    /// <summary>Current game state. Setting this applies the new state (idempotent).</summary>
    public GameState Current
    {
        get => current;
        set => Apply(value);
    }

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Optional: register with your locator for convenience
        ServiceLocator.Instance?.RegisterService(this);
    }

    /// <summary>Apply a new state if it differs from the current one.</summary>
    public void Apply(GameState next)
    {
        if (current == next) return;

        current = next;

        // Broadcast through your central bus for any listeners.
        GameEventBus.Instance?.Publish(GameEventType.GameStateChanged, next);

        // And also via a direct C# event for lightweight subscribers.
        OnStateChanged?.Invoke(next);
    }
}
