using UnityEngine;
using System.Collections;

public class ARAMTA : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float moveSpeed = 0f;
    public float distance = 50;
    public float intersection = 0f;

    public Vector3 sizeOfBomb;
    public Vector3 locationOfBomb;

    public Vector3 sizeOfMuzzleFlash;
    public Vector3 locationOfMuzzleFlash;

    public Vector3 sizeOfBullet12;
    public Vector3 locationOfBullet12;

    public Vector3 sizeOfMuzzleFlash12;
    public Vector3 locationOfMuzzleFlash12;

    public int animNum = 0;

    public int ammo12mm = 1;
    public int ammoStriker = 1;

    public Animator anim;
    public AudioSource audioSource;

    public float healthPoint;
    public float AttackCoolTime;

    private bool isAttacking = false;

    public GameObject bombPrefab;
    public GameObject bulletPrefab;

    [SerializeField] private GameObject MuzzleFlashPrefab_125mm;
    [SerializeField] private GameObject MuzzleFlashPrefab_12mm;

    private SpriteRenderer[] spriteRenderers;

    public ObjectPool pool;

    public int ammo = 1;

    private bool isDied;
    private bool isMoving = false;

    public float maxHP = 8000;
    [SerializeField] private GameObject smokePrefab;

    public static class AN
    {
        public const int Fire = -1;
        public const int Idle = 0;
        public const int Start = 1;
        public const int Move = 2;
        public const int Stop = 3;
    }

    void OnEnable()
    {
        isDied = false;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animNum = AN.Move;
        anim.SetInteger("ARMATA", animNum);
        ammo12mm = 1;
        distance = 100;
        healthPoint = maxHP;
        intersection = 40 + Random.Range(-5, 10);
        AttackCoolTime = 5 + Random.Range(-1.5f, 1.5f);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (healthPoint < 0)
        {
            isDied = true;
            if (isDied)
                Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);

            StartCoroutine(pool.ReturnToPoolAfterDelay(0f));
            ApplyDarkenEffect();
            isDied = false;
        }

        ApplyDarkenEffect();
        anim.SetInteger("ARMATA", animNum);

        if ((Time.frameCount % 50 == 0) && healthPoint < 3000)
        {
            GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(smokePrefab, Quaternion.identity, new Vector3(1, 1, 1));
            smoke.transform.position = transform.position + new Vector3(-2, 3, 0);
        }

        if (Time.frameCount % 15 == 0)
        {
            FindClosestTarget();
        }

        if (distance <= intersection)
        {

            animNum = AN.Idle;
            StartCoroutine(Fire12mm());

            if (!isAttacking && ammo > 0)
            {
                StartCoroutine(FireAndWait());
            }
        }
        else if (distance > intersection)
        {
            animNum = AN.Move;
            isAttacking = false;
            anim.SetInteger("ARMATA", animNum);

            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
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
        if (hpRatio > 0.7f)
        {
            SetColor(Color.white);
            return;
        }
        float normalizedRatio = Mathf.Clamp01(hpRatio / 0.7f);
        float darkenAmount = Mathf.Lerp(1f, 0.3f, 1 - normalizedRatio);

        Color darkenColor = new Color(darkenAmount, darkenAmount, darkenAmount, 1f);
        SetColor(darkenColor);
    }

    void FindClosestTarget()
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


    IEnumerator MoveStart()
    {
        if (isMoving == false)
        {
            animNum = AN.Start;
            isMoving = true;
            isAttacking = false;

            yield return new WaitForSeconds(0.5f);
            animNum = AN.Move;  
        }
    }

    public void MoveStartEnd()
    {
        animNum = AN.Move;
    }    
    public void MoveStopEnd()
    {
        animNum = AN.Idle;
    }

    void MoveStop()
    {
        if (isMoving)
        {
            animNum = AN.Stop;
            isMoving = false;
            isAttacking = true;
        }
    }

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        animNum = AN.Fire;
        anim.SetInteger("ARMATA", animNum);
        yield return new WaitForSeconds(0.5f);

        if (ammo > 0)
        {
            yield return new WaitForSeconds(AttackCoolTime);
            isAttacking = false;
        }
        else
        {
            StartCoroutine(Idle());
        }
    }

    IEnumerator Idle()
    {
        animNum = AN.Idle;
        anim.SetInteger("ARMATA", animNum);
        yield return new WaitForSeconds(AttackCoolTime);
        isAttacking = false;
    }


    IEnumerator Fire()
    {
        if (ammo > 0)
        {
            ammo--;
            audioSource.Play();

            GameObject bomb = ObjectPoolManager.Instance.GetObjectFromPool(bombPrefab, Quaternion.identity, sizeOfBomb);
            GameObject muzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab_125mm, Quaternion.identity, sizeOfMuzzleFlash);
            bomb.transform.position = transform.position + locationOfBomb;
            muzzleflash.transform.position = transform.position + locationOfMuzzleFlash;
            yield return new WaitForSeconds(AttackCoolTime);
            Reload();

        }
    }

    public Vector3 ReturnCurrentPos()
    {
        return transform.position;
    }

    public void Reload()
    {
        ammo = 1;
    }

    public IEnumerator Fire12mm()
    {
        if (ammo12mm > 0)
        {
            ammo12mm = 0;
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.Kord);

            for (int i = 0; i < 11; i++)
            {
                GameObject b12mm = ObjectPoolManager.Instance.GetObjectFromPool(bulletPrefab, Quaternion.identity, sizeOfBullet12);
                GameObject bmuzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab_12mm, Quaternion.identity, sizeOfMuzzleFlash12);
                b12mm.transform.position = transform.position + locationOfBullet12;
                bmuzzleflash.transform.position = transform.position + locationOfMuzzleFlash12;
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(3.5f);

            ammo12mm = 1;
        }
    }

    public void MinusHP(float atk)
    {
        this.healthPoint += atk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("E125mm"))
        {
            MinusHP(-500);
        }
        if (other.gameObject.CompareTag("E762"))
        {
            MinusHP(-20);
        }        
        if (other.gameObject.CompareTag("EBGM109"))
        {
            MinusHP(-6500);
        }        
        if (other.gameObject.CompareTag("Estriker"))
        {
            MinusHP(-1000);
        }        
        if (other.gameObject.CompareTag("E30mmHE"))
        {
            MinusHP(-110);
        }
        if (other.gameObject.CompareTag("E12mm"))
        {
            MinusHP(-40);
        }
    }


}
