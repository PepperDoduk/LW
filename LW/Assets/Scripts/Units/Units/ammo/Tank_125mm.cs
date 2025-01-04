using UnityEngine;

public class Tank_125mm : MonoBehaviour
{
    public AudioSource audioSource;
    public ObjectPool objectPool;

    [SerializeField] private GameObject explosionPrefab;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(65.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankHit);
            GameObject explosion = ObjectPoolManager.Instance.GetObjectFromPool(explosionPrefab, Quaternion.identity, new Vector3(0.7f, 0.7f, 1));
            explosion.transform.position = transform.position;

            StartCoroutine(objectPool.ReturnToPoolAfterDelay(0));
        }
    }
}
