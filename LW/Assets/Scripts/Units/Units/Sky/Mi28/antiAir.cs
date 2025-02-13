using UnityEngine;
using System.Collections;

public class antiAir : MonoBehaviour
{
    public AudioSource audioSource;
    public ObjectPool objectPool;

    public ScreenShake shake;

    public float speed;
    public float angle;
    private Rigidbody2D rb;

    public Vector3 ExplosionSize;
    public GameObject Explosion;

    [SerializeField] private float duration = 1.2f;
    [SerializeField] private Quaternion targetRotation;
    void Awake()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        targetRotation = Quaternion.Euler(0, 0, Random.Range(14, -2));
    }


    void Update()
    {
        Vector3 moveDirection = transform.right;
        transform.position += moveDirection * speed * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AirEnemy") || other.gameObject.CompareTag("Ground"))
        {
            GameObject Striker = ObjectPoolManager.Instance.GetObjectFromPool(Explosion, Quaternion.identity, ExplosionSize);
            Striker.transform.position = transform.position;

            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.striker_explosion);
            shake.SetShake(0.3f, 0.3f, 1f);
            shake.TriggerShake();

            StartCoroutine(objectPool.ReturnToPoolAfterDelay(0));
        }
    }
}
