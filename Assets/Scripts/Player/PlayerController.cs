using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 20f;
    public int maxJumpCount = 2;

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
            EndSlide();
            DoJump();
            return;
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

        // 캐릭터 눕히기 (Scale)
        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * slideScaleY,
            originalScale.z
        );

        // 콜라이더 줄이기
        box.size = new Vector2(
            originalColliderSize.x,
            originalColliderSize.y * slideScaleY
        );

        // 바닥에 붙이기
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

    void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
