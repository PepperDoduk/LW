using UnityEngine;
using System.Collections;
using static ObjectPoolManager;
using UnityEngine.UI;

public class Mi28 : MonoBehaviour
{
    public Mi28_30mm mi2830mm;

    public float soundTime;

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
    [SerializeField] private GameObject rocketPrefab;
    public GameObject bulletPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Prefab Locate")]
    public Vector3 sizeOfRocket;
    public Vector3 locationOfMissile;
    public Vector3 sizeOfBullet;
    public Vector3 locationOfBullet;
    public Vector3 sizeOfMuzzleFlash;
    public Vector3 locationOfMuzzleFlash;

    [Header("Distance Tracking")]
    public float distance;
    public float distanceToBase;
    public float intersection = 70;

    private float startY;
    private float targetY;

    public int speedNum = 1;

    public bool isPlayingSound = false;

    public float timeCheck;
    public float timeCheckResult;

    private float accelerationDuration = 3f; 
    private float targetSpeed = 16f; 
    private float accelerationElapsed = 0f;

    public bool isMoveBack = false;

    public float healthPoint;
    public float maxHP = 4500;
    private bool isDied;

    public ObjectPool pool;

    private SpriteRenderer[] spriteRenderers;

    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private GameObject antiAircraftRocket;

    public float airIntersection = 80;
    public float distanceToAirUnit;

    public AudioSource audioSource;
    GameObject sliderObject;
    private Slider slider;

    void Start()
    {
        
        StartCoroutine(TakingOff());
        startPosition = transform.position;
        startRotation = transform.rotation;
        missile = 12000;
        ammo = 300000;
        flare = 5;

        distance = 100;

        moveSpeed = 0;

        intersection = 65;
        targetY = 20;

        isLanded = false;
        isAttacking = false;
        stop = false;
        isFlying = false;
        isDied = false;

        healthPoint = maxHP;

        sliderObject = GameObject.Find("sfxSlider");
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        slider = sliderObject.GetComponent<Slider>();
        

    }

    private void OnEnable()
    {
        healthPoint = maxHP;
    }

    void Update()
    {
        if (slider != null)
        {
            audioSource.volume = 0.3f * slider.value;
        }

        if (healthPoint < 0)
        {
            isDied = true;
            if (isDied)
                Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);

            StartCoroutine(pool.ReturnToPoolAfterDelay(0f));
            ApplyDarkenEffect();
            isDied = false;
        }

        if (distanceToAirUnit <= airIntersection)
        {
            StartCoroutine(AntiAircraft());
        }

        ApplyDarkenEffect();
        if ((Time.frameCount % 50 == 0) && healthPoint < 2500)
        {
            GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(smokePrefab, Quaternion.identity, new Vector3(1, 1, 1));
            smoke.transform.position = transform.position + new Vector3(-3, 0, 0);
        }

        if (isFlying)
        {
            float sinY = 19.5f + Mathf.Sin(Time.time) * 1.5f;
            transform.position = new Vector3(transform.position.x, sinY, transform.position.z);
        }

        if (Time.frameCount % 5 == 0)
        {
            LocateTarget();
            LocateAirTarget();
        }

        if (moveSpeed == 0)
            targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        timeElapsed += Time.deltaTime;

        HandleMovement();

        if (distance <= intersection)
        {
            StartCoroutine(Attack());
            StartCoroutine(mi2830mm.Fire30mm());
        }
        else
        {
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
        }
    }

    void SetColor(Color color)
    {
        foreach (var renderer in spriteRenderers)
        {
            renderer.color = color;
        }
    }
    void ApplyDarkenEffect()
    {
        float hpRatio = Mathf.Clamp01(healthPoint / maxHP);
        if (hpRatio > 0.8f)
        {
            SetColor(Color.white);
            return;
        }
        float normalizedRatio = Mathf.Clamp01(hpRatio / 0.8f);
        float darkenAmount = Mathf.Lerp(1f, 0.3f, 1 - normalizedRatio);

        Color darkenColor = new Color(darkenAmount, darkenAmount, darkenAmount, 1f);
        SetColor(darkenColor);
    }
    void HandleMovement()
    {
        if (distance < 30 && !isMoveBack)
        {
            StartCoroutine(MoveBack());
        }
        else {
            if (((distanceToAirUnit <= airIntersection) && (distance <= intersection)) ||
                (distanceToAirUnit <= airIntersection) ||
                (distance <= intersection))
            {
                Stop();
            }
            else
            {
                Move();
            }

            Vector3 moveDirection = transform.right;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * duration);
        }
    }

    public void Move()
    {
        accelerationElapsed += Time.deltaTime;
        if (accelerationElapsed > accelerationDuration)
        {
            accelerationElapsed = accelerationDuration;
        }

        float t = accelerationElapsed / accelerationDuration;
        moveSpeed = Mathf.Lerp(0, targetSpeed, t);

        targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, -13f);
    }
    public void Stop()
    {
        if (moveSpeed > 0)
        {
            accelerationElapsed -= Time.deltaTime;
            if (accelerationElapsed < 0)
            {
                accelerationElapsed = 0;
            }

            float t = accelerationElapsed / accelerationDuration;
            moveSpeed = Mathf.Lerp(0, targetSpeed, t);
        }
        else
        {
            moveSpeed = 0;
            accelerationElapsed = 0;
            targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }

    public IEnumerator MoveBack()   
    {
        if (isMoveBack)
            yield break;

        isMoveBack = true;

        float moveBackDuration = 5f;
        float elapsed = 0f;

        Vector3 moveDirection = -transform.right;

        targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 13f);

        while (elapsed < moveBackDuration)
        {
            elapsed += Time.deltaTime;

            moveSpeed = Mathf.Lerp(0, targetSpeed, elapsed / moveBackDuration);

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);

            yield return null;
        }

        float decelerationDuration = 12f;
        elapsed = 0f;
        float initialSpeed = moveSpeed;

        while (elapsed < decelerationDuration)
        {
            elapsed += Time.deltaTime;

            moveSpeed = Mathf.Lerp(initialSpeed, 0, elapsed / decelerationDuration);

            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            yield return null;
        }

        moveSpeed = 0;

        isMoveBack = false;
    }



    public IEnumerator TakingOff()
    {
        isTakingOff = true;
        moveSpeed = 0;
        float takeOffDuration = 3f;
        float elapsed = 0f;

        startY = transform.position.y; 
        targetY = 18f;

        while (elapsed < takeOffDuration)
        {
            elapsed += Time.deltaTime;

            float newY = Mathf.Lerp(startY, targetY, elapsed / takeOffDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            moveSpeed = Mathf.Lerp(0, 4, elapsed / takeOffDuration);

            yield return null;
        }

        isTakingOff = false;
        isFlying = true;

        startY = transform.position.y;
        targetY = startY;
    }

    public void MinusHP(float atk)
    {
        this.healthPoint += atk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("E125mm"))
        //{
        //    MinusHP(-500);
        //}
        //if (other.gameObject.CompareTag("E762"))
        //{
        //    MinusHP(-20);
        //}
        //if (other.gameObject.CompareTag("EBGM109"))
        //{
        //    MinusHP(-6500);
        //}
        if (other.gameObject.CompareTag("Eantiair"))
        {
            MinusHP(-1000);
        }
        if (other.gameObject.CompareTag("E30mmHE"))
        {
            MinusHP(-110);
        }
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
        if (transform.position.y > 8)
        {
            for (int i = 0; i < 4; ++i)
            {
                missile--;
                Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TitanMissle);
                GameObject rocket = ObjectPoolManager.Instance.GetObjectFromPool(rocketPrefab, Quaternion.identity, sizeOfRocket);
                rocket.transform.position = transform.position + locationOfMissile;
                rocket.transform.rotation = transform.rotation;
                yield return new WaitForSeconds(0.4f);
            }
            yield return new WaitForSeconds(10f);
            isAttack = false;
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

    public IEnumerator AntiAircraft()
    {
        if (missile > 0 && !isAttack)
        {
            isAttack = true;
            StartCoroutine(AntiAirRocketFire());
            yield break;
        }
    }
    public IEnumerator AntiAirRocketFire()
    {
        isAttack = true;
        for (int i = 0; i < 6; ++i)
        {
            missile--;
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TitanMissle);

            GameObject antiAirRocket = ObjectPoolManager.Instance.GetObjectFromPool(antiAircraftRocket, Quaternion.identity, new Vector3(0.2f, 0.2f, 0.2f));
            antiAirRocket.transform.position = transform.position + locationOfMissile + new Vector3(0, -1, 0);
            //antiAirRocket.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(10f);
        isAttack = false;
    }

    public void LocateAirTarget()
    {
        GameObject[] bases = GameObject.FindGameObjectsWithTag("AirEnemy");
        float closestDistanceToAirUnit = Mathf.Infinity;
        foreach (GameObject baseObj in bases)
        {
            float dist = Vector3.Distance(transform.position, baseObj.transform.position);
            if (dist < closestDistanceToAirUnit)
            {
                closestDistanceToAirUnit = dist;
            }
        }
        distanceToAirUnit = closestDistanceToAirUnit;
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
        targetRotation = Quaternion.Euler(0f, -0f, 0);
        moveSpeed = -40;
        yield return null;

    }
    public IEnumerator TurnToRight()
    {
        speedNum = 1;
        targetRotation = Quaternion.Euler(0f, -180f, 0f);
        moveSpeed = 40;
        yield return null;

    }

}
