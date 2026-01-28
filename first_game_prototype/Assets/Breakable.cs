using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour
{
    public float breakVelocityThreshold = 8f;
    public float respawnTime = 10f;

    Vector3 startPosition;
    Quaternion startRotation;

    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sr;

    bool isBroken;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken)
            return;

        // Only care about the ball
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (ballRb == null)
            return;

        float impactSpeed = ballRb.linearVelocity.magnitude;

        if (impactSpeed >= breakVelocityThreshold)
        {
            Break();
        }
    }

    void Break()
    {
        isBroken = true;

        // Disable visuals & collision
        sr.enabled = false;
        col.enabled = false;

        // Stop physics movement
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = false;

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.simulated = true;
        sr.enabled = true;
        col.enabled = true;

        isBroken = false;
    }
}
