using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    // Input values
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool CrouchPressed { get; private set; }


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
        if (ServiceLocator.Instance != null)
        {
            ServiceLocator.Instance.RegisterService(this);
        }
    }


    private void Update()
    {
        // Read legacy input values
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        LookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));


        // Read button inputs
        InteractPressed = Input.GetKeyDown(KeyCode.E);
        JumpPressed = Input.GetKeyDown(KeyCode.Space);
        SprintPressed = Input.GetKey(KeyCode.LeftShift);
        CrouchPressed = Input.GetKey(KeyCode.LeftControl);


            // Handle pause input - with null check
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance != null)
        {
            GameManager.Instance.TogglePause();
        }
    }


    public void EnableInput()
    {
        // Enable cursor if needed
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    public void DisableInput()
    {
        // Reset input values
        MoveInput = Vector2.zero;
        LookInput = Vector2.zero;
        InteractPressed = false;
        JumpPressed = false;
        SprintPressed = false;
        CrouchPressed = false;

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}