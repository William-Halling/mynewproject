

public interface IMultiplayerService
{
    bool IsInitialized { get; }

    void Initialize();
    void HostSession();
    void JoinSession();
    void InviteFriends();
    void LeaveSession();

    int MaxPlayers { get; }
    int CurrentPlayers { get; }
}
