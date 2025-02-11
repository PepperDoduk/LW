using UnityEngine;

public class Eantiair : MonoBehaviour
{

    public AudioSource audioSource;
    public ObjectPool objectPool;

    public ScreenShake shake;

    public float speed;
    public float angle;
    private Rigidbody2D rb;

    public Vector3 ExplosionSize;
    public GameObject Explosion;
    void Awake()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {

        Transform nearestTarget = FindClosestTarget();
        if (nearestTarget != null)
        {
            Vector3 direction = (nearestTarget.position - transform.position).normalized;
            //direction.z += Random.Range(-5, 5);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 180, angle += Random.Range(-15, 15));
        }

        float radians = angle * Mathf.Deg2Rad;
        Vector2 initialVelocity = new Vector2(speed * Mathf.Cos(radians), speed * Mathf.Sin(radians));
        rb.linearVelocity = initialVelocity;
    }
    //void OnEnable()
    //{
    //    Transform nearestTarget = FindClosestTarget();
    //    if (nearestTarget != null)
    //    {
    //        Vector3 direction = (nearestTarget.position - transform.position).normalized;

    //        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //        transform.rotation = Quaternion.Euler(0, 0, angle);  // 올바른 2D 회전
    //    }
    //}


    Transform FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("AirUnit");
        Transform nearestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = target.transform;
            }
        }

        return nearestTarget;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AirUnit") || other.gameObject.CompareTag("Ground"))
        {
            GameObject Striker = ObjectPoolManager.Instance.GetObjectFromPool(Explosion, Quaternion.identity, ExplosionSize);
            Striker.transform.position = transform.position;

            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.striker_explosion);
            shake.SetShake(0.3f, 0.3f, 0.8f);
            shake.TriggerShake();

            StartCoroutine(objectPool.ReturnToPoolAfterDelay(0));
        }
    }
}
