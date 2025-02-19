using UnityEngine;
using System.Collections;

public class VLADIMIR : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float moveSpeed;
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

    public float HealthPoint;
    public float AttackCoolTime;

    private bool isAttacking = false;

    public GameObject bombPrefab;
    public GameObject bulletPrefab;

    [SerializeField] private GameObject MuzzleFlashPrefab_115mm;
    [SerializeField] private GameObject MuzzleFlashPrefab_12mm;

    public ObjectPool pool;

    public int ammo = 1;

    private bool isDied;
    private bool isMoving = false;

    public static class AN
    {
        public const int Fire = -1;
        public const int Idle = 0;
        public const int Move = 1;
    }

    void OnEnable()
    {
        isDied = false;
        HealthPoint = 5000;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animNum = AN.Move;
        anim.SetInteger("vlad", animNum);
        ammo12mm = 1;
        distance = 100;
    }

    void Update()
    {


        anim.SetInteger("vlad", animNum);

        if (HealthPoint < 0)
        {
            isDied = true;
            if (isDied)
                Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);
            StartCoroutine(pool.ReturnToPoolAfterDelay(0f));
            isDied = false;
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
            isMoving = true;
            anim.SetInteger("vlad", animNum);

            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
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
            //animNum = AN.Start;
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
            //animNum = AN.Stop;
            isMoving = false;
            isAttacking = true;
        }
    }

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        animNum = AN.Fire;
        anim.SetInteger("vlad", animNum);
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
        anim.SetInteger("vlad", animNum);
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
            GameObject muzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab_115mm, Quaternion.identity, sizeOfMuzzleFlash);
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
        this.HealthPoint += atk;
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
    }


}
