using UnityEngine;
using System.Collections;
using static ObjectPoolManager;

public class M1: MonoBehaviour
{
    public string targetTag = "Unit";
    public float moveSpeed = 0f;
    public float distance;
    public float intersection = 0f;

    public Animator anim;
    public GameObject bomb;
    public GameObject bulletPrefab;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    [SerializeField] private GameObject MuzzleFlashPrefab_12mm;
    public int ammo12mm = 1;

    private bool isAttacking = false;
    public bool isDied = false;

    public ObjectPool pool;

    public int animNum = 0;
    public int ammo = 1;

    private SpriteRenderer[] spriteRenderers;
    public float maxHP = 9000;

    [SerializeField] private GameObject MuzzleFlashPrefab_125mm;
    [SerializeField] private GameObject smokePrefab;


    public Vector3 sizeOfBomb;
    public Vector3 locationOfBomb;

    public Vector3 sizeOfMuzzleFlash;
    public Vector3 locationOfMuzzleFlash;

    public Vector3 sizeOfBullet12;
    public Vector3 locationOfBullet12;

    public Vector3 sizeOfMuzzleFlash12;
    public Vector3 locationOfMuzzleFlash12;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        HP = 9000;
        AttackCoolTime = 6f;
        anim.SetInteger("M1_anim", (int)1);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        isDied = false;
        HP = 9000;
        intersection = 45 + Random.Range(-7, 12);
        AttackCoolTime = 6 + Random.Range(-1.5f, 1.5f);
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
        float hpRatio = Mathf.Clamp01(HP / maxHP);
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
    void Update()
    {
        if ((Time.frameCount % 50 == 0) && HP < 3000)
        {
            GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(smokePrefab, Quaternion.identity, new Vector3(1, 1, 1));
            smoke.transform.position = transform.position + new Vector3(-2, 3, 0);
        }
        if (HP < 0)
        {
            ApplyDarkenEffect();
            anim.SetInteger("M1_anim", (int)-5);
            isDied = true;
            StopAllCoroutines();
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);
            StartCoroutine(pool.ReturnToPoolAfterDelay(2f));
            gameObject.SetActive(false);
            
            //Destroy(gameObject);
        }
        else
        {
            if (!isDied)
            {
                anim.SetInteger("M1_anim", animNum);
                ApplyDarkenEffect();
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
                    StartCoroutine(Fire12mm());
                    if (!isAttacking && !isDied)
                    {
                        StartCoroutine(FireAndWait());
                    }
                }
                else if (distance > intersection)
                {
                    isAttacking = false;
                    anim.SetInteger("M1_anim", (int)1);
                    Vector3 nowPosition = transform.position;
                    nowPosition.x += moveSpeed * -1 * Time.deltaTime;
                    transform.position = nowPosition;
                }
            }
        }
    }

    //IEnumerator FireAndWait()
    //{
    //    isAttacking = true;
    //    anim.SetInteger("M1_anim", (int)-1);
    //    yield return new WaitForSeconds(0.5f);
    //    anim.SetInteger("M1_anim", (int)0);
    //    yield return new WaitForSeconds(AttackCoolTime);
    //    isAttacking = false;
    //    anim.SetInteger("M1_anim", (int)0);
    //}

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        animNum = -1;
        anim.SetInteger("M1_anim", animNum);
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

            Instantiate(bomb, transform.position + locationOfBomb, Quaternion.identity);
            //GameObject tankAmmo = ObjectPoolManager.Instance.GetObjectFromPool(bomb, Quaternion.identity, sizeOfBomb);
            GameObject muzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab_125mm, Quaternion.identity, sizeOfMuzzleFlash);
            //tankAmmo.transform.position = transform.position + locationOfBomb;
            muzzleflash.transform.position = transform.position + locationOfMuzzleFlash;
            yield return new WaitForSeconds(AttackCoolTime);
            Reload();

        }
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
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.BrowningM2);

            for (int i = 0; i < 15; i++)
            {
                GameObject b12mm = ObjectPoolManager.Instance.GetObjectFromPool(bulletPrefab, Quaternion.identity, sizeOfBullet12);
                GameObject bmuzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab_12mm, Quaternion.identity, sizeOfMuzzleFlash12);
                b12mm.transform.position = transform.position + locationOfBullet12;
                bmuzzleflash.transform.position = transform.position + locationOfMuzzleFlash12;
                bmuzzleflash.transform.rotation = Quaternion.Euler(0,180,0);
                yield return new WaitForSeconds(0.15f);
            }

            yield return new WaitForSeconds(3.5f);

            ammo12mm = 1;
        }
    }


    //public void PlaySound()
    //{
    //    audioSource.Play();
    //    Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 2.0f, 0), Quaternion.identity);
    //}
    public void MinusHealthPoint(float atk)
    {
        this.HP += atk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("125mm"))
        {
            MinusHealthPoint(-750);
        }
        if (other.gameObject.CompareTag("115mm"))
        {
            MinusHealthPoint(-550);
        }
        if (other.gameObject.CompareTag("210mm"))
        {
            MinusHealthPoint(-1050);
        }
        if (other.gameObject.CompareTag("35mm"))
        {
            MinusHealthPoint(-55);
        }
        if (other.gameObject.CompareTag("Kh38"))
        {
            MinusHealthPoint(-6500);
        }
        if (other.gameObject.CompareTag("30mmHE"))
        {
            MinusHealthPoint(-45);
        }
        if (other.gameObject.CompareTag("striker"))
        {
            MinusHealthPoint(-450);
        }
        if (other.gameObject.CompareTag("Rpg"))
        {
            MinusHealthPoint(-250);
        }
        if (other.gameObject.CompareTag("12mm"))
        {
            MinusHealthPoint(-30);
        }
        if (other.gameObject.CompareTag("152mmHE"))
        {
            MinusHealthPoint(-4500);
        }
        if (other.gameObject.CompareTag("7.62mm"))
        {
            MinusHealthPoint(-10);
        }

        if (other.gameObject.CompareTag("AerialBomb"))
        {
            MinusHealthPoint(-5000);
        }
        if (other.gameObject.CompareTag("TacNuke"))
        {
            MinusHealthPoint(-25000);
        }
    }
}
