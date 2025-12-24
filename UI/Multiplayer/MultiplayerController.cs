using UnityEngine;

public class MultiplayerController : MonoBehaviour
{
    public static MultiplayerController Instance { get; private set; }

    private IMultiplayerService service;

    public bool IsInLobby { get; private set; }
    public bool IsHost { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

            // Swap this line later for Steam/EOS/etc
        service = new LocalMultiplayerService();
        service.Initialize();

        ServiceLocator.Instance?.RegisterService(this);
    }


    public void HostGame()
    {
        if (!service.IsInitialized)
        {
            return;
        }
        
        IsHost = true;
        IsInLobby = true;

        service.HostSession();

        SceneLoader.Instance.LoadScene("Lobby");
    }


    public void JoinGame()
    {
        if (!service.IsInitialized)
        {
            return;
        
        }
        IsHost = false;
        IsInLobby = true;

        service.JoinSession();

        SceneLoader.Instance.LoadScene("Lobby");
    }


    public void InviteFriends()
    {
        if (!service.IsInitialized)
        {
            return;
        }
        
        service.InviteFriends();
    }


    public void LeaveGame()
    {
        service.LeaveSession();

        IsInLobby = false;
        IsHost    = false;
    }
}
