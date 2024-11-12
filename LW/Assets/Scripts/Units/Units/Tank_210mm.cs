using UnityEngine;

public class Tank_210mm : MonoBehaviour
{
    public AudioSource audioSource;
    public ObjectPool objectPool;

    [SerializeField] private GameObject explosionPrefab;

    public float speed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(speed, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankHit);

            StartCoroutine(objectPool.ReturnToPoolAfterDelay(0));
            GameObject explosion = ObjectPoolManager.Instance.GetObjectFromPool(explosionPrefab, Quaternion.identity, new Vector3(1f, 1f, 1));
            explosion.transform.position = transform.position; //new Vector3(transform.position.x - 1, transform.position.y, 0);
        }
    }
}
