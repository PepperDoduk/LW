using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class Mi28 : MonoBehaviour
{
    [Header("Tags")]
    public string targetTag = "Enemy";
    public string landTag = "Airbase";

    [Header("Flight State")]
    public bool isFlying;
    public bool isTakingOff;
    public bool isLanding;
    public bool isLanded;
    public bool isAttacking;
    public bool stop;
    public bool isAttack;

    [Header("Ammunition")]
    public float missile = 12;
    public float ammo = 300;
    public float flare = 5;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float duration = 2f;
    private Vector3 startPosition; 
    private Vector3 targetPosition; 
    private Quaternion startRotation; 
    private Quaternion targetRotation; 
    private float timeElapsed = 0f;
    private float timeElapsedI = 0f;

    [Header("Prefabs")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Prefab Locate")]
    public Vector3 sizeOfBomb;
    public Vector3 locationOfBomb;

    [Header("Distance Tracking")]
    public float distance;
    public float distanceToBase;
    public float intersection = 70;

    private float startY;
    private float targetY;

    public int speedNum = 1;

    void Start()
    {
        StartCoroutine(TakingOff());
        startPosition = transform.position;
        startRotation = transform.rotation;
        missile = 12;
        ammo = 300;
        flare = 5;

        moveSpeed = 0;

        intersection = 60;
        targetY = 20;

        isLanded = false;
        isAttacking = false;
        stop = false;
        isFlying = false;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);

        if (distance <= intersection)
        { StartCoroutine(Attack()); }
        else
        {
            //float newY = Mathf.Lerp(startY, targetY, timeElapsed / duration);
            if (isFlying)
                transform.position = new Vector3(transform.position.x, 20, transform.position.z);

            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
            {
                timeElapsed = 0f;
                startY = transform.position.y;
            }

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.3f)
            {
                startRotation = transform.rotation;
                timeElapsed = 0f;
            }

            if (transform.rotation != targetRotation)
            {
                startRotation = transform.rotation;
                timeElapsed = 0f;
            }

            if (transform.position.x > 200)
            {
                StartCoroutine(TurnToLeft());
            }
            else if (transform.position.x < -200)
            {
                StartCoroutine(TurnToRight());
            }

            if (transform.position.z != 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }

            //if (distance < 90 && isFlying && transform.position.y > 15 && distance > 80 && speedNum == 1)
            //{
            //    if (!isAttack)
            //    {
            //        StartCoroutine(Attack());
            //    }
            //}

            //if ((ammo < 10 || missile < 2)
            //{
            //    StartCoroutine(Landing());
            //}

            if (isFlying && !isLanded && !stop)
            {
                targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, -13f);
                moveSpeed = 16;
            }
            else if (!isFlying && isLanded && !isTakingOff)
            {
                moveSpeed = 0;
            }

            Vector3 moveDirection = transform.right;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            if (Time.frameCount % 5 == 0)
            {
                LocateTarget();
                LocateBase();
            }
        }
    }

    public IEnumerator TakingOff()
    {
        isTakingOff = true;
        moveSpeed = 0; // 처음엔 정지 상태
        float takeOffDuration = 3f; // 이륙 시간
        float elapsed = 0f;

        startY = transform.position.y; // 시작 높이
        targetY = startY + 20f; // 목표 높이

        while (elapsed < takeOffDuration)
        {
            elapsed += Time.deltaTime;

            // Y축 상승
            float newY = Mathf.Lerp(startY, targetY, elapsed / takeOffDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // moveSpeed를 점진적으로 증가
            moveSpeed = Mathf.Lerp(0, 4, elapsed / takeOffDuration);

            yield return null;
        }

        // 이륙 완료 상태 전환
        isTakingOff = false;
        isFlying = true;

        // **여기에서 startY와 targetY를 적절히 초기화**
        startY = transform.position.y; // 현재 Y 좌표를 새로운 startY로 설정
        targetY = startY; // targetY를 현재 높이로 고정
    }




    public IEnumerator Attack()
    {
        if (missile > 0 && !isAttack)
        {
            isAttack = true;
            StartCoroutine(LandBombing());
            yield break;
        }
    }
    public IEnumerator LandBombing()
    {
        missile--;
        GameObject Bomb = ObjectPoolManager.Instance.GetObjectFromPool(bombPrefab, Quaternion.identity, sizeOfBomb);
        Bomb.transform.position = transform.position;
        Bomb.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(6f);
        isAttack = false;
    }

    public void LocateTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;
        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
            }
        }
        distance = closestDistance;
    }

    public void LocateBase()
    {
        GameObject[] bases = GameObject.FindGameObjectsWithTag("Airbase");
        float closestDistanceToBase = Mathf.Infinity;
        foreach (GameObject baseObj in bases)
        {
            float dist = Vector3.Distance(transform.position, baseObj.transform.position);
            if (dist < closestDistanceToBase)
            {
                closestDistanceToBase = dist;
            }
        }
        distanceToBase = closestDistanceToBase;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag(landTag))
        {
            isFlying = false;
            StartCoroutine("Landed");
        }
    }



    public IEnumerator Landing()
    {
        if (speedNum == 1)
        {
            targetRotation = Quaternion.Euler(0f, transform.rotation.y, -45f);
            isLanding = true;
            yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(1f);
            StartCoroutine("Landed");
        }
    }

    public IEnumerator Landed()
    {
        targetRotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
        isFlying = false;
        yield return new WaitForSeconds(4);
        missile = 4;
        ammo = 100;
        missile = 6;
        flare = 3;
        yield return new WaitForSeconds(1);
        StartCoroutine("TakingOff");

    }

    public IEnumerator TurnToLeft()
    {
        speedNum = -1;
        targetRotation = Quaternion.Euler(0f, -180f, 0);
        moveSpeed = -40;
        yield return null;

    }
    public IEnumerator TurnToRight()
    {
        speedNum = 1;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        moveSpeed = 40;
        yield return null;

    }

}
