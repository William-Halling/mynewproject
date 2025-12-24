using System;

public static class EventSystemExtensions
{
    private static GameEventBus Bus => GameEventBus.Instance;

    public static void PublishGame(GameEventType evt, object payload = null)
        => Bus?.Publish(evt, payload);

    public static void SubscribeGame(GameEventType evt, Action<object> callback)
        => Bus?.AddListener(evt, callback);

    public static void UnsubscribeGame(GameEventType evt, Action<object> callback)
        => Bus?.RemoveListener(evt, callback);
}
