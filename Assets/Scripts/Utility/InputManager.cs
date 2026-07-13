using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // The public static instance that other scripts will use to access the singleton
    public static InputManager Instance { get; private set; }

    // Reference to your auto-generated Input Action class
    private PlayerControl gameInput;

    // Custom C# events to hide the complex CallbackContext syntax from other scripts
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;

    public event Action OnSprintPressed;
    public event Action OnSprintReleased;
    
    public event Action OnAttackPressed;
    
    public event Action OnLockOnPressed;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        gameInput = new PlayerControl();
        gameInput.Player.Enable();
        gameInput.Combat.Enable();
        gameInput.Camera.Enable();


        gameInput.Player.Jump.performed += ctx => OnJumpPressed?.Invoke();
        gameInput.Player.Jump.canceled += ctx => OnJumpReleased?.Invoke();

        gameInput.Player.Sprint.performed += ctx => OnSprintPressed?.Invoke();
        gameInput.Player.Sprint.canceled += ctx => OnSprintReleased?.Invoke();
        
        gameInput.Combat.Attack.performed += ctx => OnAttackPressed?.Invoke();
        gameInput.Combat.LockOn.performed += ctx => OnLockOnPressed?.Invoke();
    }

    private void OnDestroy()
    {
        if (gameInput != null)
        {
            gameInput.Player.Disable();
        }
    }

    public Vector2 GetInputVector()
    {
        return gameInput.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetCameraRotationVector()
    {
        return gameInput.Camera.Look.ReadValue<Vector2>();
    }
    
    
    public float Horizontal 
    {
        get { return gameInput.Player.Move.ReadValue<Vector2>().x; }
    }

    public float Vertical
    {
        get { return gameInput.Player.Move.ReadValue<Vector2>().y; }
    }
}