using UnityEngine;
using Player;
using Utility;

namespace Player
{

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float baseMoveSpeed = 5f;
    
    [SerializeField] [Range(0.1f, 5f)] float sprintMultiplier;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] private float maxMoveClamp = 0.3f;
    [SerializeField] private float jumpForce;

    Quaternion targetRotation;
    float currentMoveSpeed;
    [SerializeField] bool isGrounded;
    
    private CharacterController _cc;
    private Animator _anim;
    private MeleeFighter _mf;
    private float baseMaxMoveClamp;
    private float baseJumpForce;
    private bool isSprinting;
    private float verticalVelocity;
    private float originalCCheight;
    private float gravity = -9.81f;
    
    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;
    private float initialGroundCheckRadius;
    [SerializeField] private Vector3 groundCheckOffset;
  
    
    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _mf = GetComponent<MeleeFighter>();
        
        InputManager.Instance.OnJumpPressed += JumpPressed;
        InputManager.Instance.OnSprintPressed += SprintPressed;
        InputManager.Instance.OnSprintReleased += SprintReleased;
        
        originalCCheight = _cc.height;
        currentMoveSpeed = baseMoveSpeed;
        initialGroundCheckRadius =  groundCheckRadius;

        // Cache the "resting" values once so sprint start/stop is idempotent
        // even if the events fire more than once without a matching pair.
        baseMaxMoveClamp = maxMoveClamp;
        baseJumpForce = jumpForce;
    }

    void OnDestroy()
    {
        if (InputManager.Instance == null) return;

        InputManager.Instance.OnJumpPressed -= JumpPressed;
        InputManager.Instance.OnSprintPressed -= SprintPressed;
        InputManager.Instance.OnSprintReleased -= SprintReleased;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if(_mf.InAction)
        {
            targetRotation = transform.rotation;
            _anim.SetFloat("ForwardSpeed", 0f);
            return;
        }
        
        var moveDirection = PlayerCameraControls.Instance.GetDirection();
        float moveAmount = Mathf.Clamp(moveDirection.magnitude, 0, maxMoveClamp);
        
        var moveInput =  new Vector3(moveDirection.x, 0, moveDirection.z).normalized;
        
        //Gravity
        GroundCheck();
        
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -0.5f;
        }

        if (!isGrounded)
        {
            _cc.height = 0.5f;
            groundCheckRadius = initialGroundCheckRadius * 2;
        }
        else
        {
            _cc.height = originalCCheight;
            groundCheckRadius = initialGroundCheckRadius;
        }

        verticalVelocity += gravity * Time.deltaTime;
        
        var horizontolVelocity = moveInput * currentMoveSpeed;
        horizontolVelocity.y = verticalVelocity;
        
        //Final Movements
        if (CombatController.Instance.CombatMode)
        {
            if (CombatController.Instance.CombatMode)
            {
                Vector3 disToTarget = CombatController.Instance.targetEnemy.transform.position - transform.position;
                disToTarget.y = 0;
                Vector3 toTarget = disToTarget.normalized;
                Vector3 strafeAxis = Vector3.Cross(Vector3.up, toTarget);

                // Raw input, NOT camera-relative moveInput — W/S = approach/retreat,
                // A/D = strafe perpendicular to the enemy, so it actually arcs around them.
                Vector2 rawInput = InputManager.Instance.GetInputVector();
                Vector3 combatMoveDir = strafeAxis * rawInput.x + toTarget * rawInput.y;
                if (combatMoveDir.sqrMagnitude > 1f)
                    combatMoveDir.Normalize();

                horizontolVelocity = combatMoveDir * (currentMoveSpeed / 2f);
                horizontolVelocity.y = verticalVelocity;
                
                targetRotation = Quaternion.LookRotation(toTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


                var forwardSpeed = Vector3.Dot(combatMoveDir, transform.forward);
                _anim.SetFloat("ForwardSpeed", forwardSpeed, 0.2f, Time.deltaTime);

                float Angle = Vector3.SignedAngle(transform.forward, combatMoveDir, Vector3.up);
                var sideSpeed = Mathf.Sin(Angle * Mathf.Deg2Rad);

                _anim.SetFloat("SideSpeed", sideSpeed, 0.2f, Time.deltaTime);
                _cc.Move(horizontolVelocity * Time.deltaTime);
            }
        }
        else
        {
            _cc.Move(horizontolVelocity * Time.deltaTime);
            if (moveInput.sqrMagnitude > 0.001f)
            {
                targetRotation = Quaternion.LookRotation(moveInput); 
            }
        
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _anim.SetFloat("ForwardSpeed", moveAmount);
        }
        
        _anim.SetBool("Jump", !isGrounded);
    }
    
    void JumpPressed()
    {
        if (!isGrounded) return;
        verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
    }
    

    void SprintPressed()
    {
        if(CombatController.Instance.CombatMode) return;
        if (isSprinting) return; // guard against duplicate press events

        isSprinting = true;
        currentMoveSpeed = baseMoveSpeed * sprintMultiplier;
        maxMoveClamp = 1f;
        jumpForce = baseJumpForce * 2f;
    }

    void SprintReleased()
    {
        if(CombatController.Instance.CombatMode) return;
        if (!isSprinting) return; // guard against duplicate release events

        isSprinting = false;
        currentMoveSpeed = baseMoveSpeed;
        maxMoveClamp = baseMaxMoveClamp;
        jumpForce = baseJumpForce;
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere( transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}
    
}