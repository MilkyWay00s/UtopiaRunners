using UnityEngine;
using System;

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

    [SerializeField] private Animator animator;

    // 캐릭터/스킬이 구독할 이벤
    public event Action<int> OnJumped;        // jumpCount 전달
    public event Action OnSlideStarted;
    public event Action OnSlideEnded;
    public event Action OnFirstJump;

    public bool IsGrounded() => isGrounded;
    public bool IsSliding => isSliding;
    public int JumpCount => jumpCount;

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
        if (ChatScriptController.Instance != null && ChatScriptController.Instance.IsChatPlaying)
            return;

        bool jumpInput =
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.UpArrow);

        bool slideHold =
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.DownArrow);

        bool slideDown =
            Input.GetKeyDown(KeyCode.LeftControl) ||
            Input.GetKeyDown(KeyCode.DownArrow);

        if (slideDown && !isSliding && isGrounded)
        {
            StartSlide();
        }

        if (isSliding && !slideHold)
        {
            EndSlide();
        }

        if (jumpInput && jumpCount < maxJumpCount)
        {
            if (isSliding)
                EndSlide();

            DoJump();
        }
    }

    void DoJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount++;

        // 첫 점프 이벤트
        if (jumpCount == 1)
            OnFirstJump?.Invoke();

        // 점프 이벤트 
        OnJumped?.Invoke(jumpCount);
    }


    void StartSlide()
    {
        isSliding = true;

        if (animator) animator.SetBool("IsSlide", true);

        if (normalVisual) normalVisual.SetActive(false);
        if (slideVisual) slideVisual.SetActive(true);

        box.size = slideColliderSize;
        box.offset = slideColliderOffset;


        OnSlideStarted?.Invoke();
    }

    void EndSlide()
    {
        isSliding = false;

        if (animator) animator.SetBool("IsSlide", false);

        if (normalVisual) normalVisual.SetActive(true);
        if (slideVisual) slideVisual.SetActive(false);

        box.size = originalColliderSize;
        box.offset = originalColliderOffset;


        OnSlideEnded?.Invoke();
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