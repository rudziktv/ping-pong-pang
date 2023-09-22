using UnityEngine;

public class TestGameRule : MonoBehaviour
{
    [SerializeField]
    GameObject ball;

    [Range(1f, 100f)]
    [SerializeField]
    float velocity;

    Rigidbody2D rb;

    private void Start()
    {
        rb = ball.GetComponent<Rigidbody2D>();

        rb.AddForce(Vector2.right * velocity, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * velocity / rb.mass;
    }
}
