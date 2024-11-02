using UnityEngine;
using System.Collections;
using System.Drawing;

public class Titan_II : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float moveSpeed = 0f;
    public float distance;
    public float intersection = 0f;

    public Vector3 sizeOfBomb;
    public Vector3 locationOfBomb;

    public Vector3 sizeOfMuzzleFlash;
    public Vector3 locationOfMuzzleFlash;

    public Vector3 sizeOfBullet35;
    public Vector3 locationOfBullet35;

    public Vector3 sizeOfMuzzleFlash35;
    public Vector3 locationOfMuzzleFlash35;

    public Vector3 sizeOfMissle;
    public Vector3 locationOfMissle;

    public int animNum = 0;

    public int ammo35mm = 1;

    public Animator anim;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isAttacking = false;

    public GameObject bombPrefab;
    public GameObject bulletPrefab;

    [SerializeField] private GameObject titanMuzzleFlashPrefab;
    [SerializeField] private GameObject MuzzleFlashPrefab;

    public int ammo = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animNum = 1;
        anim.SetInteger("titan", animNum);
        ammo35mm = 1;
    }

    void Update()
    {
        if (HP < 0)
        {
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);
            Destroy(gameObject, 0.3f);
        }

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

        if (distance <= intersection)
        {
            StartCoroutine(Fire35mm());
            if (!isAttacking && ammo > 0)
            {
                StartCoroutine(FireAndWait());
            }
        }
        else if (distance > intersection)
        {
            isAttacking = false;
            animNum = 1;
            anim.SetInteger("titan", animNum);
            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        animNum = -1;
        anim.SetInteger("titan", animNum);
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
        animNum = 0;
        anim.SetInteger("titan", animNum);
        yield return new WaitForSeconds(AttackCoolTime);
        Reload();
        isAttacking = false;
    }


    IEnumerator Fire()
    {
        if (ammo > 0)
        {
            ammo--;
            audioSource.Play();
            //Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 7f, 0), Quaternion.identity);

            for (int i = 0; i < 2; ++i)
            {
                GameObject bomb = ObjectPoolManager.Instance.GetObjectFromPool(bombPrefab, Quaternion.identity, sizeOfBomb);
                GameObject muzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(titanMuzzleFlashPrefab, Quaternion.identity, sizeOfMuzzleFlash);
                bomb.transform.position = transform.position + locationOfBomb;
                muzzleflash.transform.position = transform.position + locationOfMuzzleFlash;
                yield return new WaitForSeconds(0.5f);
            }

            //Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 7f, 0), Quaternion.identity);
            /*GameObject bomb2 = ObjectPoolManager.Instance.GetObjectFromPool(bombPrefab, Quaternion.identity, sizeOfBomb);
            GameObject muzzleflash2 = ObjectPoolManager.Instance.GetObjectFromPool(titanMuzzleFlashPrefab, Quaternion.identity, sizeOfMuzzleFlash);
            bomb2.transform.position = transform.position + locationOfBomb;
            muzzleflash2.transform.position = transform.position + locationOfMuzzleFlash;*/
        }
    }


    public void Reload()
    {
        ammo = 1;
    }

    public IEnumerator Fire35mm()
    {
        if (ammo35mm > 0)
        {
            ammo35mm = 0;
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.Titan35mm);

            for (int i = 0; i < 20; i++)
            {
                //Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 5.6f, 0), Quaternion.Euler(0, 0, Random.Range(-0.3f, 0.3f)));
                GameObject b35mm = ObjectPoolManager.Instance.GetObjectFromPool(bulletPrefab, Quaternion.identity, sizeOfBullet35);
                GameObject bmuzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab, Quaternion.identity, sizeOfMuzzleFlash35);
                b35mm.transform.position = transform.position + locationOfBullet35;
                bmuzzleflash.transform.position = transform.position + locationOfMuzzleFlash35;
                yield return new WaitForSeconds(0.06f);
            }

            yield return new WaitForSeconds(6f);

            ammo35mm = 1;
        }
    }

    public void MinusHealthPoint(float atk)
    {
        this.HP += atk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("E125mm"))
        {
            MinusHealthPoint(-500);
        }
        if (other.gameObject.CompareTag("500KG"))
        {
            MinusHealthPoint(-10000);
        }
        if (other.gameObject.CompareTag("E762"))
        {
            MinusHealthPoint(-20);
        }
    }
}
