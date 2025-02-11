using UnityEngine;

public class E30mm : MonoBehaviour
{
    public AudioSource audioSource;
    public ObjectPool objectPool;

    public float speed;
    public float angle;
    private Rigidbody2D rb;

    public Vector3 ExplosionSize;
    public GameObject Explosion;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        int randomAngleOffset = Random.Range(-6, 6);
        angle = -155 - randomAngleOffset;
        float radians = angle * Mathf.Deg2Rad;
        Vector2 initialVelocity = new Vector2(speed * Mathf.Cos(radians), speed * Mathf.Sin(radians)); // Y축 반전 적용
        rb.linearVelocity = initialVelocity;
        float angleInDegrees = angle;
        transform.rotation = Quaternion.Euler(0, 0, angleInDegrees);
    }

    void Update()
    {
        Vector2 direction = rb.linearVelocity;
        if (direction.magnitude > 0)
        {
            float angleInDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angleInDegrees);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Unit") || other.gameObject.CompareTag("Ground"))
        {
            GameObject Striker = ObjectPoolManager.Instance.GetObjectFromPool(Explosion, Quaternion.identity, ExplosionSize);
            Striker.transform.position = transform.position;

            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.explosion30mm);
            StartCoroutine(objectPool.ReturnToPoolAfterDelay(0));
        }
    }
}
