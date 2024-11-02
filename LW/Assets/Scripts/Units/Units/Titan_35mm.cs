using UnityEngine;

public class Titan_35mm : MonoBehaviour
{
    public AudioSource audioSource;
    public ObjectPool objectPool;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        
    }

    void Update()
    {
        Vector3 movement = new Vector3(80.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Destroy(gameObject);
            objectPool.ReturnToPoolAfterDelay(0);
            objectPool.Deactivate();
        }
    }
}
