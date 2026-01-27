using UnityEngine;

public class DissolveFast : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float dissolveTimer = 0.0f;
    public float lifeSpan = 0.15f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dissolveTimer += Time.deltaTime;
        if(dissolveTimer > lifeSpan)
        {
            Destroy(gameObject);
        }
    }
}
