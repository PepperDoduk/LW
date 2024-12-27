using UnityEngine;

public class Tank_152mm_Direct : MonoBehaviour
{
    [Header("Classes")]
    public ObjectPool objectPool;
    public ScreenShake shake;
    public MSTA msta;

    [Header("Stats")]
    public float speed;
    public float angle;
    public Rigidbody2D rb;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;
    private float timeElapsed = 0f;

    private bool isAreadyExploded = false;

    public float attackDirection;

    [Header("Explosion Values")]
    public Vector3 ExplosionSize;
    public GameObject Explosion;

    [Header("Movement")]

    public float fireForce = 55f;

    void OnEnable()
    {
        ResetPhysics();
        Force();
        isAreadyExploded = false;
    }

    public void ResetPhysics()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
    void Start()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
    }

    void Force()
    {
            rb.gravityScale = 0.3f;
            rb.AddForce(Vector2.right * fireForce * Random.Range(1.55f, 1.75f) * 2.2f, ForceMode2D.Impulse);
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
        if (!isAreadyExploded)
        {
            if (!isAreadyExploded && other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Ground"))
            {
                isAreadyExploded = true;
                StartCoroutine(objectPool.ReturnToPoolAfterDelay(0.12f));
                GameObject Bomb = ObjectPoolManager.Instance.GetObjectFromPool(Explosion, Quaternion.identity, ExplosionSize);
                Bomb.transform.position = transform.position;

                shake.SetShake(0.5f, 0.5f, 0.5f);
                shake.TriggerShake();

                Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.striker_explosion);

            }
        }
    }


}
