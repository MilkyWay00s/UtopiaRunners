using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 20f;
    public int maxJumpCount = 2;
    public float slideDuration = 0.5f;
    public GameObject currentWeapon;

    [Header("Slide Visual")]
    public GameObject normalVisual;
    public GameObject slideVisual;

    [Header("Slide Collider")]
    public Vector2 slideColliderSize = new Vector2(1f, 0.5f);
    public Vector2 slideColliderOffset = new Vector2(0f, -0.25f);

    Rigidbody2D rb;
    BoxCollider2D box;

    bool isSliding = false;
    bool isGrounded = false;
    int jumpCount = 0;

    Vector2 originalColliderSize;
    Vector2 originalColliderOffset;

    public bool IsGrounded() => isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        originalColliderSize = box.size;
        originalColliderOffset = box.offset;

        if (normalVisual) normalVisual.SetActive(true);
        if (slideVisual) slideVisual.SetActive(false);
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

        // 슬라이드 중 점프
        if (isSliding && jumpInput && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            if (jumpCount == 0)
                OnJump();

            GetComponent<IcarusSkill>()?.OnJumpOrSlide();

            jumpCount++;
            GetComponent<HaniSkill>()?.OnAirJump(jumpCount);
        }

        // 슬라이드 시작
        if (slideDown && !isSliding && isGrounded)
        {
            StartSlide();
        }

        // 슬라이드 종료
        if (isSliding && !slideHold)
        {
            EndSlide();
        }

        // 일반 점프
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

        if (normalVisual) normalVisual.SetActive(false);
        if (slideVisual) slideVisual.SetActive(true);

        box.size = slideColliderSize;
        box.offset = slideColliderOffset;
    }

    void EndSlide()
    {
        isSliding = false;

        if (normalVisual) normalVisual.SetActive(true);
        if (slideVisual) slideVisual.SetActive(false);

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

    void OnJump()
    {
        var chainWeapon =
            currentWeapon.GetComponent<ElectricOrbBehaviour>();

        chainWeapon?.EnableChainOnce();
    }
}
