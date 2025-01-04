using System.Collections;
using UnityEngine;

public class SU_57 : MonoBehaviour
{
    public Animator anim;


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
    public float bomb = 5;
    public float missile = 6;
    public float ammo = 100;
    public float flare = 3;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float duration = 2f;
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;
    private float timeElapsed = 0f;

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
    private float intersection = 70;

    public int animNum;

    public int speedNum = 1;

    public static class AN
    {
        public const int Landed = -2;
        public const int Landing = -1;
        public const int Idle = 0;
        public const int Bording = 1;

    }
    void Start()
    {
        StartCoroutine("TakingOff");
        startRotation = transform.rotation;
        bomb = 5;
        missile = 6;
        ammo = 100;
        flare = 3;

        intersection = 70;
        animNum = AN.Landed;

        isLanded = false;
        isAttacking = false;
        stop = false;
    }

    void Update()
    {

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.3f)
        {
            startRotation = transform.rotation;
            timeElapsed = 0f;
        }

        if (transform.transform.position.z !=0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
       }

        if (transform.position.x > 200)
        {
            StartCoroutine(TurnToLeft());
        }

        if (transform.position.x < -260)
        {
            StartCoroutine(TurnToRight());
        }

        if (distance < 90 && isFlying && transform.position.y > 50 && distance>80 && speedNum ==1)
        {
            if (!isAttack)
            {
                StartCoroutine(Attack());
            }
        }

        timeElapsed += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);

        if (transform.rotation != targetRotation)
        {
            startRotation = transform.rotation;
            timeElapsed = 0f;
        }

        if ((bomb == 0 && missile < 2) || (ammo < 20 && missile < 2) || ammo < 20)
        {
            Landing();
        }

        if (isFlying && !isLanded && !stop)
        {
            moveSpeed = 70;

            animNum = AN.Idle;
        }
        else if (!isFlying && isLanded && !isTakingOff)
        {
            moveSpeed = 0;
        }

        anim.SetInteger("su57", animNum);

        Vector3 moveDirection = transform.right;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (Time.frameCount % 5 == 0)
        {
            LocateTarget();
            LocateBase();
        }

    }

    public IEnumerator Attack()
    {
        if (bomb > 0 && !isAttack)
        {
                isAttack = true;
                StartCoroutine(LandBombing());
           yield break;
        }
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

    public IEnumerator LandBombing()
    {
        bomb--;
        GameObject Bomb = ObjectPoolManager.Instance.GetObjectFromPool(bombPrefab, Quaternion.identity, sizeOfBomb);
        Bomb.transform.position = transform.position;
        Bomb.transform.rotation= transform.rotation;
        yield return new WaitForSeconds(6f);
        isAttack = false;
    }

    public IEnumerator Landing()
    {
        if(speedNum == 1) { 
        targetRotation = Quaternion.Euler(0f, transform.rotation.y, -45f);
        isLanding = true;
        yield return new WaitForSeconds(1f);
        animNum = AN.Landing;
        yield return new WaitForSeconds(1f);
        StartCoroutine("Landed");
            }
    }

    public IEnumerator Landed()
    {
        animNum = AN.Landed;
        targetRotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
        isFlying = false;
        yield return new WaitForSeconds(4);
        missile = 4;
        ammo = 100;
        bomb = 6;
        flare = 3;
        yield return new WaitForSeconds(1);
        StartCoroutine("TakingOff");

    }

    public IEnumerator TakingOff()
    {
        isTakingOff = true;
        moveSpeed = 7;

        startRotation = transform.rotation;


        yield return new WaitForSeconds(1f);
        moveSpeed = 15;
        yield return new WaitForSeconds(0.6f);
        targetRotation = Quaternion.Euler(0f, transform.rotation.y, 20f);
        moveSpeed = 22;
        yield return new WaitForSeconds(0.4f);

        targetRotation = Quaternion.Euler(0f, transform.rotation.y, 35f);
        moveSpeed = 40;

        yield return new WaitForSeconds(0.4f);
        animNum = AN.Bording;

        yield return new WaitForSeconds(0.7f);

        moveSpeed = 55;
        isTakingOff = false;
        isFlying = true;

        yield return new WaitForSeconds(0.3f);
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
        animNum = AN.Idle;
    }

    public IEnumerator TurnToLeft()
    {
        speedNum = -1;
        targetRotation = Quaternion.Euler(0f, -180f, 0);
        moveSpeed = -60;
        yield return new WaitForSeconds(0f);

    }
    public IEnumerator TurnToRight()
    {
        speedNum = 1;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        moveSpeed = 60;
        yield return new WaitForSeconds(0f);

    }

}
