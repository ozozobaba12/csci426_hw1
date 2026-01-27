// https://findmysourcecode.com/drag-and-shoot-unity-2d-tutorial/
using UnityEngine;
using System.Collections;

public class DragShot : MonoBehaviour
{
    private Rigidbody2D rb;

    public Color startColor = Color.green;

    public Color endColor = Color.red;

    private SpriteRenderer spriteRenderer;

    public float maxPullDuration = 0.001f;

    public float pullTimer = 0f;
    public Vector2 startPos;
    public Vector2 endPos;
    public Vector3 camAdjust = new Vector3(0, 0, -10);
    public bool isDragging;

    public Vector3 delta;
    private LineRenderer lineRenderer;
    public float forceMultiplier = 500f;
    public Transform cameraObject; 

    public Vector3 lastMouse;



    void Start()
    {
        // Initialize Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Set initial color
        spriteRenderer.color = Color.green;
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not attached to the GameObject!");
            return;
        }
        lastMouse = Input.mousePosition;
        // Get the existing Line Renderer component
        // lineRenderer = GetComponent<LineRenderer>();

        delta = new Vector3(0,0,0);

        /*
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is not found on this GameObject!");
        }
        */

    
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && isDragging && pullTimer < maxPullDuration)
        {
            Dragging();
        }
        else if (Input.GetMouseButtonUp(0) && isDragging || pullTimer > maxPullDuration && isDragging)
        {
            Shoot();
            pullTimer = 0;
            spriteRenderer.color = Color.blue;
        }
        if(!isDragging)
        {
            spriteRenderer.color = Color.green;
        }
        delta = Input.mousePosition - lastMouse;
        lastMouse = Input.mousePosition;
        cameraObject.localPosition = camAdjust;
    }

    private void StartDragging()
    {
        isDragging = true;
        // Store the ball's current position
        // lineRenderer.enabled = true; // Enable the line renderer
        Time.timeScale = 0.25f;
        StartCoroutine(TransitionColor(startColor, endColor, maxPullDuration));
    }

    void OnMouseDown()
    {
        StartDragging();
    }

    private void Dragging()
    {
        // Update the endpoint of the drag
        startPos = rb.position;
        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // lineRenderer.SetPosition(0, startPos);
        // lineRenderer.SetPosition(1, endPos);

        camAdjust.x -= delta.x * 0.01f;
        camAdjust.y -= delta.y * 0.01f;

        pullTimer += Time.deltaTime;
    }

    private void Shoot()
    {
        Time.timeScale = 1.0f;
        isDragging = false;
        // lineRenderer.enabled = false; // Disable the line after shooting
        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the drag vector
        Vector2 dragVector = startPos - endPos; // Inverted vector

        // Apply the force in the opposite direction of the drag
        rb.isKinematic = false; // Allow gravity to take effect
        rb.AddForce(dragVector * forceMultiplier);
        camAdjust.x = 0;
        camAdjust.y = 0;
        spriteRenderer.color = Color.blue;
    }

    IEnumerator TransitionColor(Color startColor, Color endColor, float time)
    {
        float timer = 0f;

        while (timer < time)
        {
            // Calculate the interpolation value (0 to 1)
            timer += Time.deltaTime;
            float t = timer / time;

            // Smooth the transition (optional, makes it less linear)
            t = Mathf.SmoothStep(0f, 1f, t); 

            // Interpolate from the start color to the end color
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);

            if (!isDragging)
            {
                spriteRenderer.color = startColor;
                yield break;
            }

            yield return null; // Wait until the next frame
        }

        // Ensure the final color is exactly red at the end
        spriteRenderer.color = endColor;
    }
}
