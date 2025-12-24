using System;
using System.Collections.Generic;
using UnityEngine;


    /// <summary>
    /// Lightweight in-project event bus. Replaces the custom EventSystem class to avoid colliding with UnityEngine.EventSystems.EventSystem.
    /// </summary>
public class GameEventBus : MonoBehaviour
{
    public static GameEventBus Instance { get; private set; }

    private Dictionary<string, List<Action<object>>> _listeners;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
    
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

        _listeners = new Dictionary<string, List<Action<object>>>();


            // Register with ServiceLocator if present
        ServiceLocator.Instance?.RegisterService(this);
    }


    private string Key(GameEventType evt) => evt.ToString();


    public void AddListener(GameEventType evt, Action<object> callback)
    {
        var key = Key(evt);


        if (!_listeners.ContainsKey(key))
        {
            _listeners[key] = new List<Action<object>>();
        }

        if (!_listeners[key].Contains(callback))
        { 
            _listeners[key].Add(callback);
        }
    }


    public void RemoveListener(GameEventType evt, Action<object> callback)
    {
        var key = Key(evt);


        if (_listeners.ContainsKey(key))
        { 
            _listeners[key].Remove(callback);
        }
    }


    public void Publish(GameEventType evt, object payload = null)
    {
        var key = Key(evt);
        if (!_listeners.ContainsKey(key))
        { 
            return;
        }

            // copy to avoid modification-during-iteration
        var copy = new List<Action<object>>(_listeners[key]);
        
        
        foreach (var cb in copy)
        {
            try
            {
                cb?.Invoke(payload);
            }
            catch (Exception e) 
            {
                Debug.LogError($"[GameEventBus] Exception while publishing {evt}: {e}"); 
            }
        }
    }



    // convenience alias
    public void InvokeEvent(GameEventType evt, object payload = null) => Publish(evt, payload);
}
