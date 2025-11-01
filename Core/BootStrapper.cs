using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameEventBus eventSystemPrefab;
    [SerializeField] private InputManager inputManagerPrefab;
    [SerializeField] private GameManager gameManagerPrefab;


    public static Bootstrapper Instance { get; private set; }


    [Header("Configuration")]
    [SerializeField] private bool loadMainMenu = true;
    //[SerializeField] private string initialScene = "MainMenu";


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Register with Service Locator
        ServiceLocator.Instance.RegisterService(this);
    }


    private void Start()
    {
        InitializeGame();
    }


    public void InitializeGame()
    {
        Debug.Log("Initializing game systems...");


        // Ensure core systems exist
        EnsureCoreSystems();


        // Register all services
        RegisterServices();


        // Load initial scene
        if (loadMainMenu)
        {
            //SceneLoader.Instance.LoadScene(initialScene);
        }

        Debug.Log("Game initialization complete");
    }


    private void EnsureCoreSystems()
    {
        if (GameEventBus.Instance == null)
        {
            if (eventSystemPrefab != null)
            {
                Instantiate(eventSystemPrefab, transform);
            }

            else
            {
                GameObject eventSystemObj = new GameObject("EventSystem");

                eventSystemObj.AddComponent<GameEventBus>();
                eventSystemObj.transform.SetParent(transform);
            }
        }


        // InputManager
        if (InputManager.Instance == null)
        {
            if (inputManagerPrefab != null)
            {
                Instantiate(inputManagerPrefab, transform);
            }
            else
            {
                GameObject inputManagerObj = new GameObject("InputManager");

                inputManagerObj.AddComponent<InputManager>();
                inputManagerObj.transform.SetParent(transform);
            }
        }


        // GameManager
        if (GameManager.Instance == null)
        {
            if (gameManagerPrefab != null)
            {
                Instantiate(gameManagerPrefab, transform);
            }
            else
            {
                GameObject gameManagerObj = new GameObject("GameManager");

                gameManagerObj.AddComponent<GameManager>();
                gameManagerObj.transform.SetParent(transform);
            }
        }
        

        // SaveManager
        if (SaveManager.Instance == null)
        {
            GameObject saveManagerObj = new GameObject("SaveManager");

            saveManagerObj.AddComponent<SaveManager>();
            saveManagerObj.transform.SetParent(transform);
        }


        // SceneLoader
        if (SceneLoader.Instance == null)
        {
            GameObject sceneLoaderObj = new GameObject("SceneLoader");

            sceneLoaderObj.AddComponent<SceneLoader>();
            sceneLoaderObj.transform.SetParent(transform);
        }

    }


    private void RegisterServices()
    {
        ServiceLocator.Instance.RegisterService(GameEventBus.Instance);
        ServiceLocator.Instance.RegisterService(InputManager.Instance);
        ServiceLocator.Instance.RegisterService(GameManager.Instance);
        ServiceLocator.Instance.RegisterService(SaveManager.Instance);
        ServiceLocator.Instance.RegisterService(SceneLoader.Instance);
    }
}