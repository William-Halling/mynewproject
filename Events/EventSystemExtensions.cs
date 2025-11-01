using System;

public static class EventSystemExtensions
{
    public static void PublishGame(this GameEventBus bus, GameEventType evt, object payload = null)
        => bus.Publish(evt, payload);

    public static void SubscribeGame(this GameEventBus bus, GameEventType evt, Action<object> callback)
        => bus.AddListener(evt, callback);

    public static void UnsubscribeGame(this GameEventBus bus, GameEventType evt, Action<object> callback)
        => bus.RemoveListener(evt, callback);
}
