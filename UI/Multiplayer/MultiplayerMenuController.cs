using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenuController : MonoBehaviour
{
    [SerializeField] private Button btnHost;
    [SerializeField] private Button btnJoin;
    [SerializeField] private Button btnInvite;
    [SerializeField] private Button btnBack;


    private void Awake()
    {
        if(btnJoin == null || btnInvite == null || btnBack == null)
        {
            UnityEngine.Debug.LogError("[MultiplayerMenuController] Missing button references");
            enabled = false;

            return;
        }
        

        Bind(btnHost, OnHost);
        Bind(btnJoin, OnJoin);
        Bind(btnInvite, OnInvite);
        Bind(btnBack, MenuRouter.Instance.Back);
    }


    private void OnHost()
    {
        MultiplayerController.Instance?.HostGame();
    }


    private void OnJoin()
    {
        if (MultiplayerController.Instance == null)
        {
            UnityEngine.Debug.LogError("[MultiplayerMenuController] MultiplayerController missing");
           
            return;
        }

        MultiplayerController.Instance?.JoinGame();
    }

    
    private void OnInvite()
    {
        if (MultiplayerController.Instance == null)
        {
            return;
        }
        
        MultiplayerController.Instance.InviteFriends();
    }
        
        
    private void Bind(Button button, UnityEngine.Events.UnityAction action)
    {
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(action);
    }
}