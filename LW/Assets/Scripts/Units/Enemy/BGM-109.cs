using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class BGM_109 : MonoBehaviour
{
    public bool isLaunch;

    public float moveSpeed;
    public float frameSmoke;

    public string targetTag = "Unit";

    [SerializeField] private float duration = 1.2f;
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
        frameSmoke = 5;
        isLaunch = false;
        StartCoroutine(Launch());

        shake = Camera.main.GetComponent<ScreenShake>();
    }

    void Update()
    {
        // ���� ����� Ÿ���� ã�� �Լ� ȣ��
        FindClosestTarget();

        if (targetObject != null && !isLaunch)
        {
            // ���� ����� Ÿ���� ��ġ�� ������ ���
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;

            // LookRotation�� ����Ͽ� Z�ุ ȸ���ϵ��� ����
            // Y���� �����ϰ�, Z�ุ ȸ���ϵ��� ȸ������ ����մϴ�.
            targetRotation = Quaternion.LookRotation(Vector3.forward, direction);  // Z�� ȸ���� ���

            // targetRotation�� ������Ʈ
        }

        // ��ƼŬ ����
        if (Time.frameCount % 1 * frameSmoke == 0)
        {
            GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(this.smokePrefab, Quaternion.identity, new Vector3(1, 1, 1));
            smoke.transform.position = smokeObject.transform.position;
        }

        // �̵� ó��
        Vector3 moveDirection = transform.up;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // ȸ�� ���� (targetRotation���� ȸ��)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * duration);
    }

    void FindClosestTarget()
    {
        Debug.Log("FindClosestTarget()");

        // ������ Ÿ�� �±׸� ���� ������Ʈ���� ã��
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        // ��� Ÿ�ٰ��� �Ÿ� ���
        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTarget = target;
            }
        }

        // ���� ����� Ÿ���� ���� ���
        if (closestTarget != null)
        {
            targetObject = closestTarget;

            // ���� ����� ������Ʈ�� ���� ���� ���
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;
            Debug.Log("���� ����� ������Ʈ ����: " + direction);
        }
    }



    public IEnumerator Launch()
    {
        isLaunch=true;
        moveSpeed = 15;
        targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        yield return new WaitForSeconds(1);

        moveSpeed = 20;
        targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 80f);
        yield return new WaitForSeconds(0.4f);
        moveSpeed = 35;
        frameSmoke = 4;
        yield return new WaitForSeconds(0.3f);
        isLaunch = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Unit") || other.gameObject.CompareTag("Ground"))
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
