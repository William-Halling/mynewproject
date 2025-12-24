using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [Header("Optional Prefabs")]
    [SerializeField] private GameObject gameEventBusPrefab;
    [SerializeField] private GameObject inputManagerPrefab;
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject saveManagerPrefab;
    [SerializeField] private GameObject multiplayerControllerPrefab;

    
    [SerializeField] private GameObject sceneLoaderPrefab;
    [SerializeField] private GameObject serviceLocatorPrefab;
    

    [Header("Config")]
    [SerializeField] private bool loadMainMenu = true;
    [SerializeField] private string mainMenuScene = "MainMenu";

    public static Bootstrapper Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("🔥 Bootstrapper Awake");

        EnsureServiceLocator();
        EnsureCoreSystems();
        RegisterServices();
    }


    private void Start()
    {
        if (loadMainMenu)
        {
            Debug.Log("🔥 Loading Main Menu");

            SceneLoader.Instance.LoadScene(mainMenuScene);
        }
    }


    // -----------------------------
    // SERVICE LOCATOR
    // -----------------------------
    private void EnsureServiceLocator()
    {
        if (ServiceLocator.Instance != null)
        {
            return;
        }

        if (serviceLocatorPrefab != null)
        {
            Instantiate(serviceLocatorPrefab, transform);
        }
        else
        {
            var go = new GameObject("ServiceLocator");
            go.AddComponent<ServiceLocator>();
            go.transform.SetParent(transform);
        }
    }


        // -----------------------------
        // CORE SYSTEMS
        // -----------------------------
    private void EnsureCoreSystems()
    {
        CreateIfMissing(GameEventBus.Instance, gameEventBusPrefab, "GameEventBus", typeof(GameEventBus));
        CreateIfMissing(InputManager.Instance, inputManagerPrefab, "InputManager", typeof(InputManager));
        CreateIfMissing(GameManager.Instance, gameManagerPrefab, "GameManager", typeof(GameManager));
        CreateIfMissing(SaveManager.Instance, saveManagerPrefab, "SaveManager", typeof(SaveManager));
        CreateIfMissing(SceneLoader.Instance, sceneLoaderPrefab, "SceneLoader", typeof(SceneLoader));
        CreateIfMissing(MultiplayerController.Instance, multiplayerControllerPrefab, "MultiplayerController", typeof(MultiplayerController));
    }


    private void CreateIfMissing(Object instance, GameObject prefab, string name, System.Type type)
    {
        if (instance != null)
        {
            return;
        }

        GameObject go = prefab != null ? Instantiate(prefab, transform) : new GameObject(name, type);

        go.transform.SetParent(transform);
    }


        // -----------------------------
        // SERVICE REGISTRATION
        // -----------------------------
    private void RegisterServices()
    {
        var locator = ServiceLocator.Instance;

        locator.RegisterService(GameEventBus.Instance);
        locator.RegisterService(InputManager.Instance);
        locator.RegisterService(GameManager.Instance);
        locator.RegisterService(SaveManager.Instance);
        locator.RegisterService(SceneLoader.Instance);

        Debug.Log("🔥 Services registered");
    }
}
