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
        // 가장 가까운 타겟을 찾는 함수 호출
        FindClosestTarget();

        if (targetObject != null && !isLaunch)
        {
            // 가장 가까운 타겟의 위치로 방향을 계산
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;

            // LookRotation을 사용하여 Z축만 회전하도록 설정
            // Y축은 무시하고, Z축만 회전하도록 회전값을 계산합니다.
            targetRotation = Quaternion.LookRotation(Vector3.forward, direction);  // Z축 회전만 고려

            // targetRotation을 업데이트
        }

        // 파티클 생성
        if (Time.frameCount % 1 * frameSmoke == 0)
        {
            GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(this.smokePrefab, Quaternion.identity, new Vector3(1, 1, 1));
            smoke.transform.position = smokeObject.transform.position;
        }

        // 이동 처리
        Vector3 moveDirection = transform.up;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 회전 적용 (targetRotation으로 회전)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * duration);
    }

    void FindClosestTarget()
    {
        Debug.Log("FindClosestTarget()");

        // 지정된 타겟 태그를 가진 오브젝트들을 찾음
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        // 모든 타겟과의 거리 계산
        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTarget = target;
            }
        }

        // 가장 가까운 타겟이 있을 경우
        if (closestTarget != null)
        {
            targetObject = closestTarget;

            // 가장 가까운 오브젝트의 방향 벡터 계산
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;
            Debug.Log("가장 가까운 오브젝트 방향: " + direction);
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
