using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 8f;
    public float slideDuration = 0.5f;

    Rigidbody2D rb;
    bool isSliding = false;
    bool isGrounded = false;

    public bool IsGrounded() => isGrounded;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        // 점프: Space / ↑
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded && !isSliding)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // 슬라이드: Ctrl / ↓
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding)
            StartCoroutine(Slide());
    }

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;
        // TODO: 슬라이드 애니메이션/콜라이더 변경이 있으면 여기서
        yield return new WaitForSeconds(slideDuration);
        isSliding = false;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground")) isGrounded = true;
    }
    void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}