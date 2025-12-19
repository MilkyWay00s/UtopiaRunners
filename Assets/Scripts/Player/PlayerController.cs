using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 20f;
    public int maxJumpCount = 2;
    public float slideDuration = 0.5f;

    Rigidbody2D rb;
    bool isSliding = false;
    bool isGrounded = false;
    int jumpCount = 0;

    public bool IsGrounded() => isGrounded;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        // 점프: Space / ↑
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        // 슬라이드: Ctrl / ↓
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding)
            StartCoroutine(Slide());
    }

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;
        yield return new WaitForSeconds(slideDuration);
        isSliding = false;
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
        if (c.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}
