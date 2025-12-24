using UnityEngine;


public sealed class LocalMultiplayerService : IMultiplayerService
{
    public bool IsInitialized { get; private set; }

    public int MaxPlayers => 8;
    public int CurrentPlayers => 1;
    

    public void Initialize()
    {
        IsInitialized = true;

        Debug.Log("[Multiplayer] Local service initialized");
    }


    public void HostSession()
    {
        Debug.Log("[Multiplayer] Hosting local session");
    }


    public void JoinSession()
    {
        Debug.Log("[Multiplayer] Joining local session");
    }


    public void InviteFriends()
    {
        Debug.Log("[Multiplayer] Invite requested (local stub)");
    }


    public void LeaveSession()
    {
        Debug.Log("[Multiplayer] Leaving session");
    }
}
