using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kh_38 : MonoBehaviour
{
    [Header("Classes")]
    public ObjectPool objectPool;
    public SU_57 su57;
    public ScreenShake shake;

    [Header("Stats")]
    public float speed;
    public float angle;
    private Rigidbody2D rb;
    [SerializeField] private float duration = 1.4f;
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;
    private float timeElapsed = 0f;

    float su57YRotation;
    public float attackDirection;

    private bool isAreadyExploded;

    [Header("Explosion Values")]
    public Vector3 ExplosionSize;
    public GameObject Explosion;

    [Header("Movement")]
    public float moveSpeed;

    void Start()
    {
        isAreadyExploded = false;
        timeElapsed = 0;
        moveSpeed = 10f;
        startRotation = transform.rotation;

        shake = Camera.main.GetComponent<ScreenShake>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("Attack");
    }

    void OnEnable()
    {
        //isAreadyExploded = false;
        timeElapsed = 0;
        moveSpeed = 10f;
        startRotation = transform.rotation;

        shake = Camera.main.GetComponent<ScreenShake>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("Attack");
    }

    void Update()
    {

        timeElapsed += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);

        Vector3 moveDirection = transform.right;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        Vector2 direction = rb.linearVelocity;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAreadyExploded && other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Ground"))
        {
            isAreadyExploded = true;
            GameObject Bomb = ObjectPoolManager.Instance.GetObjectFromPool(Explosion, Quaternion.identity, ExplosionSize);
            Bomb.transform.position = transform.position;

            shake.SetShake(0.8f, 0.8f, 0.5f);
            shake.TriggerShake();

            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.striker_explosion);
            StartCoroutine(objectPool.ReturnToPoolAfterDelay(0));
        }
    }

    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        moveSpeed = 70f;

        targetRotation = Quaternion.Euler(0f, startRotation.y, -57f);
    }

}
