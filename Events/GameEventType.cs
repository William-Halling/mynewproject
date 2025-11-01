public enum GameEventType
{
    NewHour = 1000,
    NewDay = 1001,

    WorldGenerated,

    // Core Events
    GameStateChanged,
    SceneLoaded,
    SceneLoadStarted,
    SaveGameRequested,
    LoadGameRequested,

    // Input Events
    InputMove,
    InputInteract,
    InputPause,

    // Player Events
    PlayerHealthChanged,
    PlayerMoneyChanged,

    // Ship Events
    ShipDamaged,
    ShipRepaired,
    ShipSunk,
}