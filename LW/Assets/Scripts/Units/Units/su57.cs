using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class su57 : MonoBehaviour
{
    public Animator anim;
    public string targetTag = "Enemy";
    public string LandTag = "Airbase";

    public bool isFlying;
    public bool isBording;
    public bool isLanding;
    public bool isLanded;

    public float bomb;
    public float missile;
    public float ammo;
    public float flare;

    public float moveSpeed;

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject bulletPrefab;

    public float distance;
    public float distanceToBase;
    public float intersection;

    public Vector3 targetEulerAngles;

    public float duration = .0f;

    public Quaternion startRotation;
    public Quaternion targetRotation;
    private float timeElapsed = 0f;

    public int animNum;

    public static class AN
    {
        public const int Landed = -2;
        public const int Landing = -1;
        public const int Idle = 0;
        public const int Bording = 1;

    }
    void Start()
    {
        Bording();
        startRotation = transform.rotation;
        bomb = 5;
        missile = 6;
        ammo = 100;
        flare = 3;

        intersection = 70;
        animNum = AN.Landed;

    }

    void Update()
    {
        // 비행 중일 때와 착륙 중일 때의 이동 속도 설정
        if ((bomb == 0 && missile < 2) || (ammo < 20 && missile < 2) || ammo < 20)
        {
            Landing();
        }

        if (isFlying && !isLanded)
        {
            moveSpeed = 20;
        }
        else if (!isFlying && isLanded && !isBording)
        {
            moveSpeed = 0;
        }

        if (transform.rotation != targetRotation)
        {
            // 새로운 목표 각도로 회전하기 위해 startRotation을 현재 회전값으로 갱신
            startRotation = transform.rotation;
            timeElapsed = 0f;  // 시간 초기화
        }

        // 목표 각도로 서서히 회전
        timeElapsed += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);

        // 애니메이션 설정
        anim.SetInteger("su57", animNum);

        // 비행체 이동: 현재 회전 방향으로 이동
        Vector3 moveDirection = transform.right;  // 오른쪽 방향으로 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        // 가장 가까운 적 탐색
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

        // 가장 가까운 기지 탐색
        GameObject[] bases = GameObject.FindGameObjectsWithTag(LandTag);
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

        if (other.gameObject.CompareTag("Airbase"))
        {
            isFlying = false;
            StartCoroutine("Landed");
        }
    }

    public IEnumerator Landing()
    {
        targetRotation = Quaternion.Euler(0f, 0f, -45f);
        isLanding = true;
        yield return new WaitForSeconds(1f);
        animNum = AN.Landing;
        yield return new WaitForSeconds(1f);
        Landed();
    }

    public IEnumerator Landed()
    {
        animNum = AN.Landed;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        isFlying = false;
        yield return new WaitForSeconds(4);
        missile = 4;
        ammo = 100;
        bomb = 6;
        flare = 3;
        yield return new WaitForSeconds(1);
        Bording();
        
    }

    public IEnumerator Bording()
    {
        isBording = true;
        moveSpeed = 5;

        startRotation = transform.rotation;


        yield return new WaitForSeconds(2f);

        targetRotation = Quaternion.Euler(0f, 0f, 45f);
        moveSpeed = 11;

        yield return new WaitForSeconds(0.4f);
        animNum = AN.Bording;

        yield return new WaitForSeconds(0.5f);

        moveSpeed = 17;
        isBording = false;
        isFlying = true;

        startRotation = transform.rotation; 
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
    }


}
