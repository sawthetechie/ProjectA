using System;
using Cinemachine;
using UnityEngine;

public class PlayerCameraControls : MonoBehaviour
{
    public static PlayerCameraControls Instance;
    
    [SerializeField] CinemachineVirtualCamera cameraController;
    [SerializeField] float sprintCamFOV;
    [SerializeField] private float transitionSpeed;
    private float initialFOV;
    private float targetFOV;
    public Vector3 InputDir {get; private set;}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        initialFOV =  cameraController.m_Lens.FieldOfView;
        targetFOV = initialFOV; 
        
        InputManager.Instance.OnSprintPressed += SprintCameraSet;
        InputManager.Instance.OnSprintReleased += SprintCameraUnset;
    }
    void Update()
    {
        cameraController.m_Lens.FieldOfView = Mathf.Lerp(
            cameraController.m_Lens.FieldOfView, 
            targetFOV, 
            transitionSpeed * Time.deltaTime
        );
    }

    public Vector3 GetDirection()
    {
        var inputCommand = InputManager.Instance.GetInputVector();
        
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection =
            cameraForward * inputCommand.y +
            cameraRight * inputCommand.x;
        InputDir = moveDirection;
        
        return moveDirection;
    }
    
    void SprintCameraSet()
    {
        targetFOV = sprintCamFOV;
    }

    void SprintCameraUnset()
    {
        targetFOV = initialFOV;
    }
}
