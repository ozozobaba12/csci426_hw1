using UnityEngine;

public class BallMovement2D : MonoBehaviour
{
    public float moveForce = 2f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Debug.Log(moveInput);
        rb.AddForce(new Vector2(moveInput * moveForce, 0f));
    }
}
