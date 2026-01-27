using UnityEngine;

public class BallMovement2D : MonoBehaviour
{
    public float moveForce = 2f;
    public float jumpVelocity = 40f;

    public bool gameFreeze = false;

    public float freezeTimer = 0;

    public float maxFreezerTime = 0.75f;

    public float freezeThreshold = 20f;

    public GameObject freezeSprite;

    public AudioSource audioSource;

    public AudioClip soundEffectClipBIGHIT;

    public AudioClip soundEffectClipTHROW;

    public AudioClip soundEffectClipBOUNCING;

    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }
        if(gameFreeze)
        {
            freezeTimer += Time.unscaledDeltaTime;
            if (freezeTimer > maxFreezerTime)
            {
                freezeTimer = 0;
                gameFreeze = false;
                if(GetComponent<DragShot>().isDragging)
                {
                    Time.timeScale = 0.25f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        // Debug.Log(moveInput);
        rb.AddForce(new Vector2(moveInput * moveForce, 0f));

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the first contact point
        ContactPoint2D contact = collision.contacts[0];

        // The impulse represents the total force (mass * velocity change) 
        // applied over the time of the collision.
        float impulse = contact.normalImpulse;

        Debug.Log("Collision Force (Impulse): " + impulse);

        if (collision.gameObject.CompareTag("Dummy"))
        {
            Debug.Log("Collided with the dummy!");
            // Strong hit
            if(impulse > freezeThreshold)
            {
                Debug.Log("Strong HIT");
                Instantiate(freezeSprite, new Vector3(contact.point.x, contact.point.y, 0), Quaternion.identity);
                gameFreeze = true;
                audioSource.PlayOneShot(soundEffectClipBIGHIT); 
                Time.timeScale = 0.0f;
            }
            else
            {
                audioSource.PlayOneShot(soundEffectClipTHROW); 
            }
            // Add your custom logic here (e.g., play a sound, update a variable)
        }
        else
        {
            audioSource.PlayOneShot(soundEffectClipBOUNCING); 
        }
    }
}
