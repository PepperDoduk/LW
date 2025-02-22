using UnityEngine;
using System.Collections;

public class R_77 : MonoBehaviour
{
    public bool isLaunch;

    public Rigidbody2D rb;

    public float moveSpeed;
    public float frameSmoke;

    public string targetTag = "AirEnemy";

    [SerializeField] private float duration = 0.7f;
    [SerializeField] private Quaternion targetRotation;

    public float distance = 200;

    public Vector3 locationOfSmoke;

    public GameObject smokeObject;
    private GameObject targetObject;

    public Vector3 ExplosionSize;
    public GameObject Explosion;

    public ScreenShake shake;

    public ObjectPool objectPool;

    [SerializeField] private GameObject smokePrefab;
    void OnEnable()
    {
        frameSmoke = 7;
        isLaunch = false;
        StartCoroutine(Launch());
        moveSpeed = 80;
        shake = Camera.main.GetComponent<ScreenShake>();
    }

    void Update()
    {
        if (!isLaunch)
        {
            FindClosestTarget();

            if (targetObject != null && !isLaunch)
            {
                Vector3 direction = (targetObject.transform.position - transform.position).normalized;

                targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            }

            if (Time.frameCount % 1 * frameSmoke == 0)
            {
                GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(this.smokePrefab, Quaternion.identity, new Vector3(1, 1, 1));
                smoke.transform.position = smokeObject.transform.position;
            }

            Vector3 moveDirection = transform.up;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * duration);
        }
    }

    void FindClosestTarget()
    {

        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTarget = target;
            }
        }

        if (closestTarget != null)
        {
            targetObject = closestTarget;

            Vector3 direction = (targetObject.transform.position - transform.position).normalized;
        }
    }



    public IEnumerator Launch()
    {
        moveSpeed = 0;
        rb.gravityScale = 1;
        yield return new WaitForSeconds(1);
        rb.gravityScale = 0;
        moveSpeed = 80;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AirEnemy") || other.gameObject.CompareTag("Ground"))
        {
            GameObject Striker = ObjectPoolManager.Instance.GetObjectFromPool(Explosion, Quaternion.identity, ExplosionSize);
            Striker.transform.position = transform.position;

            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.striker_explosion);
            shake.SetShake(1.3f, 1f, 2f);
            shake.TriggerShake();

            StartCoroutine(objectPool.ReturnToPoolAfterDelay(0));
        }
    }
}
