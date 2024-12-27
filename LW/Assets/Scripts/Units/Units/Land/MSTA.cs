using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSTA : MonoBehaviour
{
    public Tank_152mm_Curve ammo152;

    public string targetTag = "Enemy";
    public float moveSpeed = 0f;
    public float distance = 50;
    public float intersection = 0f;

    [Header("Size Of 152mm Ammo")]
    public Vector3 sizeOfBomb;
    public Vector3 locationOfBomb;

    [Header("Size Of MuzzleFlash 152mm")]
    public Vector3 sizeOfMuzzleFlash;
    public Vector3 locationOfMuzzleFlash;
    public Quaternion rotationOfMuzzleFlash;

    [Header("Size Of 12mm Bullet")]
    public Vector3 sizeOfBullet12;
    public Vector3 locationOfBullet12;

    [Header("Size Of MuzzleFlash 12mm")]
    public Vector3 sizeOfMuzzleFlash12;
    public Vector3 locationOfMuzzleFlash12;

    public int animNum = 0;

    public int ammo12mm = 1;

    public Animator anim;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isCoolTime = false;

    public GameObject bombPrefab_curve;
    public GameObject bombPrefab_direct;

    public GameObject bulletPrefab;

    public ScreenShake shake;

    [SerializeField] private GameObject MuzzleFlashPrefab_152mm;
    [SerializeField] private GameObject MuzzleFlashPrefab_12mm;

    public ObjectPool pool;

    public int ammo = 1;

    private bool isDied;
    private bool isMoving = false;

    public static class AN
    {
        public const int DirectFire = -2;
        public const int CurvedFire = -1;
        public const int Idle = 0;
        public const int Move = 1;
    }

    void Start()
    {
        isDied = false;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        shake = Camera.main.GetComponent<ScreenShake>();

        animNum = AN.Move;
        anim.SetInteger("msta", animNum);
        ammo12mm = 1;
    }

    void Update()
    {
        anim.SetInteger("msta", animNum);

        if (HP < 0)
        {
            isDied = true;
            if (isDied)
                Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);
            StartCoroutine(pool.ReturnToPoolAfterDelay(0f));
            isDied = false;
        }

        if (isCoolTime)
        {
            animNum = AN.Idle;
        }

        if (Time.frameCount % 15 == 0)
        {
            FindClosestTarget();
        }

        if (distance <= intersection)
        {
            if(distance<25){
                StartCoroutine(Fire12mm());
            }
            if (distance > 50 && !isCoolTime && ammo > 0)
            {
                StartCoroutine(CurvedFire());
            }
            if(distance < 49 && !isCoolTime && ammo > 0)
            {
                StartCoroutine(DirectFire());
            }
        }
        else if (distance > intersection && !isCoolTime)
        {
            animNum = AN.Move;
            isCoolTime = false;
            anim.SetInteger("msta", animNum);

            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    public void FindClosestTarget()
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


    public void MoveStartEnd()
    {
        animNum = AN.Move;
    }
    public void MoveStopEnd()
    {
        animNum = AN.Idle;
    }


    IEnumerator CurvedFire()
    {
        if (!isCoolTime)
        {
            isCoolTime = true;
            animNum = AN.CurvedFire;
            anim.SetInteger("msta", animNum);
            yield return new WaitForSeconds(0.5f);

            if (ammo > 0)
            {
                yield return new WaitForSeconds(AttackCoolTime);
                isCoolTime = false;
            }
            else
            {
                StartCoroutine(Idle());
            }
        }
    }

    IEnumerator DirectFire()
    {
        if (!isCoolTime)
        {
            isCoolTime = true;
            animNum = AN.DirectFire;
            anim.SetInteger("msta", animNum);
            yield return new WaitForSeconds(0.5f);

            if (ammo > 0)
            {
                yield return new WaitForSeconds(AttackCoolTime);
                isCoolTime = false;
            }
            else
            {
                StartCoroutine(Idle());
            }
        }
    }

    IEnumerator Idle()
    {
        animNum = AN.Idle;
        anim.SetInteger("msta", animNum);
        yield return new WaitForSeconds(AttackCoolTime);
        isCoolTime = false;
    }


    IEnumerator FireToCurve()
    {
        if (ammo > 0)
        {
            ammo--;
            audioSource.Play();

            shake.SetShake(0.1f, 0.1f, 0.5f);
            shake.TriggerShake();

            GameObject bomb = ObjectPoolManager.Instance.GetObjectFromPool(bombPrefab_curve, Quaternion.identity, sizeOfBomb);
            GameObject muzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab_152mm, Quaternion.identity, sizeOfMuzzleFlash);

            bomb.transform.position = transform.position + locationOfBomb;

            

            muzzleflash.transform.position = transform.position + locationOfMuzzleFlash;
            muzzleflash.transform.rotation = rotationOfMuzzleFlash;

            yield return new WaitForSeconds(AttackCoolTime);
            Reload();

        }
    }

    IEnumerator FireToDirect()
    {
        if (ammo > 0)
        {
            ammo--;
            audioSource.Play();

            shake.SetShake(0.1f, 0.1f, 0.5f);
            shake.TriggerShake();

            GameObject bomb = ObjectPoolManager.Instance.GetObjectFromPool(bombPrefab_direct, Quaternion.identity, sizeOfBomb);
            GameObject muzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab_152mm, Quaternion.identity, sizeOfMuzzleFlash);
            bomb.transform.position = transform.position + new Vector3(3.7f,3.2f,0);
            muzzleflash.transform.position = transform.position + new Vector3(4.3f, 3f, 0);
            muzzleflash.transform.rotation = Quaternion.Euler(0,0,0);

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

    public float ReturnDist()
    {
        return distance;
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
        this.HP += atk;
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
    }
}
