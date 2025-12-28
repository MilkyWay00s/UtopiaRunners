using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 20f;
    public int maxJumpCount = 2;
    public float slideDuration = 0.5f;
    public GameObject currentWeapon;
    [Header("Slide")]
    public float slideScaleY = 0.3f;


    Rigidbody2D rb;
    BoxCollider2D box;

    bool isSliding = false;
    bool isGrounded = false;
    int jumpCount = 0;

    Vector3 originalScale;
    Vector2 originalColliderSize;
    Vector2 originalColliderOffset;

    public bool IsGrounded() => isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        originalScale = transform.localScale;
        originalColliderSize = box.size;
        originalColliderOffset = box.offset;
    }

    void Update()
    {
        bool jumpInput =
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.UpArrow);

        bool slideHold =
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.DownArrow);

        bool slideDown =
            Input.GetKeyDown(KeyCode.LeftControl) ||
            Input.GetKeyDown(KeyCode.DownArrow);

        if (isSliding && jumpInput && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (jumpCount == 0)  
                OnJump();
                GetComponent<IcarusSkill>()?.OnJumpOrSlide();

            jumpCount++;
            GetComponent<HaniSkill>()?.OnAirJump(jumpCount);
        }

        if (slideDown && !isSliding && isGrounded)
        {
            StartSlide();
        }

        if (isSliding && !slideHold)
        {
            EndSlide();
        }

        if (!isSliding && jumpInput && jumpCount < maxJumpCount)
        {
            DoJump();
        }
    }

    void DoJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount++;
    }

    void StartSlide()
    {
        isSliding = true;
        GetComponent<IcarusSkill>()?.OnJumpOrSlide();
        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * slideScaleY,
            originalScale.z
        );
        box.size = new Vector2(
            originalColliderSize.x,
            originalColliderSize.y * slideScaleY
        );
        box.offset = new Vector2(
            originalColliderOffset.x,
            originalColliderOffset.y - (originalColliderSize.y * (1f - slideScaleY) * 0.5f)
        );
    }

    void EndSlide()
    {
        isSliding = false;
        transform.localScale = originalScale;
        box.size = originalColliderSize;
        box.offset = originalColliderOffset;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnJump()
    {
        var chainWeapon =
            currentWeapon.GetComponent<ElectricOrbBehaviour>();

        chainWeapon?.EnableChainOnce();
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
