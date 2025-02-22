using UnityEngine;

public class E12mm : MonoBehaviour
{
    public AudioSource audioSource;
    public ObjectPool objectPool;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 movement = new Vector3(-100.0f, 0.0f, 0.0f);
        transform.Translate(movement * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Unit"))
        {
            StartCoroutine(objectPool.ReturnToPoolAfterDelay(0));
        }
    }
}
