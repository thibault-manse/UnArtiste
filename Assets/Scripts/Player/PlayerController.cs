using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 4.2f;
    [SerializeField] private float acceleration = 28f;
    [SerializeField] private float deceleration = 36f;
    [SerializeField] private float airControl = 0.55f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 9f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.16f;
    [SerializeField] private LayerMask groundMask;

    [Header("Aim")]
    [SerializeField] private Transform weaponPivot;

    private Rigidbody2D body;
    private Vector2 moveInput;
    private Vector2 aimInput = Vector2.right;
    private bool facingRight = true;
    private bool controlsLocked;

    public Vector2 AimDirection => GetAimDirection();
    public bool FacingRight => facingRight;
    public bool IsGrounded => groundCheck != null && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (controlsLocked)
        {
            return;
        }

        float targetSpeed = moveInput.x * maxSpeed;
        float rate = Mathf.Abs(moveInput.x) > 0.05f ? acceleration : deceleration;
        if (!IsGrounded)
        {
            rate *= airControl;
        }

        float speedDelta = targetSpeed - body.linearVelocity.x;
        body.AddForce(Vector2.right * speedDelta * rate);

        if (Mathf.Abs(moveInput.x) > 0.05f)
        {
            SetFacing(moveInput.x > 0f);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = controlsLocked ? Vector2.zero : context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        if (value.sqrMagnitude > 0.12f)
        {
            aimInput = value.normalized;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!controlsLocked && context.performed && IsGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0f);
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void LockControls(bool locked)
    {
        controlsLocked = locked;
        if (locked)
        {
            moveInput = Vector2.zero;
        }
    }

    private Vector2 GetAimDirection()
    {
        Vector2 direction = aimInput.sqrMagnitude > 0.12f ? aimInput.normalized : (facingRight ? Vector2.right : Vector2.left);
        if (weaponPivot != null)
        {
            weaponPivot.right = direction;
        }

        return direction;
    }

    private void SetFacing(bool right)
    {
        if (facingRight == right)
        {
            return;
        }

        facingRight = right;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1f : -1f);
        transform.localScale = scale;
    }
}
