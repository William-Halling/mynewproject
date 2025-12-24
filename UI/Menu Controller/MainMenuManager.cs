using UnityEngine;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    [Header("--- Main Buttons ---")]
    [SerializeField] private Button btnSinglePlayer;
    [SerializeField] private Button btnMultiplayer;
    [SerializeField] private Button btnOptions;
    [SerializeField] private Button btnSupport;
    [SerializeField] private Button btnQuit;


    [Header("--- Single Player Panel ---")]
    //[SerializeField] private Button btnNewGame; 
    [SerializeField] private Button btnLoadGame;
    [SerializeField] private Button btnSPBack;


    [Header("--- Quit Confirm ---")]
    [SerializeField] private Button btnQuitYes;
    [SerializeField] private Button btnQuitNo;


    [Header("--- Options ---")]
    [SerializeField] private Button btnOptionsBack;


    private void Start()
    {
        // Main menu routing
        btnSinglePlayer.onClick.AddListener(() => MenuRouter.Instance.ShowPanel(MenuRouter.Instance.panelSinglePlayer ));


        btnMultiplayer.onClick.AddListener(() => MenuRouter.Instance.ShowPanel(MenuRouter.Instance.panelMultiplayer));


        btnOptions.onClick.AddListener(() => MenuRouter.Instance.ShowPanel(MenuRouter.Instance.panelOptions));


        btnSupport.onClick.AddListener(() => MenuRouter.Instance.ShowPanel(MenuRouter.Instance.panelSupport));


        btnQuit.onClick.AddListener(() => MenuRouter.Instance.ShowPanel(MenuRouter.Instance.panelQuitConfirm));


        // Back buttons
        btnSPBack.onClick.AddListener(MenuRouter.Instance.Back);
        btnOptionsBack.onClick.AddListener(MenuRouter.Instance.Back);

        // Quit confirm
        btnQuitNo.onClick.AddListener(MenuRouter.Instance.Back);
        btnQuitYes.onClick.AddListener(Application.Quit);
    }
}
