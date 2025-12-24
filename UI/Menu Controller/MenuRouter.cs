using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuRouter : MonoBehaviour
{
    public static MenuRouter Instance { get; private set; }


    [Header("Panel References")]
    public GameObject panelMain;
    public GameObject panelSinglePlayer;
    public GameObject panelMultiplayer;
    public GameObject panelOptions;
    public GameObject panelSupport;
    public GameObject panelQuitConfirm;


    private List<GameObject> _allPanels;
    private GameObject _currentPanel;
    private readonly Stack<GameObject> _history = new Stack<GameObject>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;


        _allPanels = new List<GameObject>
        {
            panelMain,
            panelSinglePlayer,
            panelMultiplayer,
            panelOptions,
            panelSupport,
            panelQuitConfirm
        };
    }


    private void Start()
    {
        CloseAllPanels();

        ShowPanel(panelMain);
    }


    private void CloseAllPanels()
    {
        foreach (var panel in _allPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        _history.Clear();
        _currentPanel = null;
    }


        /// <summary>
        /// Shows a new panel, hides the current one, and pushes the current one onto history.
        /// </summary>
    public void ShowPanel(GameObject panelToShow, GameObject defaultSelection = null)
    {
        if (panelToShow == null || panelToShow == _currentPanel)
        { 
            return;
        }


        if (_currentPanel != null)
        {
            _currentPanel.SetActive(false);
            _history.Push(_currentPanel);
        }


        panelToShow.SetActive(true);
        _currentPanel = panelToShow;


            // Try to pick something sensible if no explicit default is passed
        if (defaultSelection == null)
        {
            var selectable = panelToShow.GetComponentInChildren<Selectable>();

            defaultSelection = selectable != null ? selectable.gameObject : null;
        }


        SetSelection(defaultSelection);
    }


        /// <summary>
        /// Navigates back to the last panel in the history stack.
        /// </summary>
    public void Back()
    {
        if (_history.Count > 0)
        {
            if (_currentPanel != null)
            {
                _currentPanel.SetActive(false);
            }

            GameObject previousPanel = _history.Pop();
            previousPanel.SetActive(true);
            _currentPanel = previousPanel;

            var defaultSelectable = previousPanel.GetComponentInChildren<Selectable>();
            SetSelection(defaultSelectable != null ? defaultSelectable.gameObject : null);
        }
        else
        {
            // No history = ensure main menu is active and focused
            CloseAllPanels();
            panelMain?.SetActive(true);
            _currentPanel = panelMain;

            var defaultSelectable = panelMain != null ? panelMain.GetComponentInChildren<Selectable>() : null;

            SetSelection(defaultSelectable != null ? defaultSelectable.gameObject : null);
        }
    }



    private void SetSelection(GameObject target)
    {
        var uiEventSystem = UnityEngine.EventSystems.EventSystem.current;
       
        if (uiEventSystem == null)

            return;


        uiEventSystem.SetSelectedGameObject(null);

        if (target != null)
        {
            uiEventSystem.SetSelectedGameObject(target);
        }
    }
}
