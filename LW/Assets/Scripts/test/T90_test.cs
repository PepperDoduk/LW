using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class T90_test : MonoBehaviour
{
    [SerializeField] private GameObject T90muzzleFlashPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private GameObject bulletPrefab;

    public string targetTag = "Enemy";
    public float moveSpeed = 2f;
    public float distance;
    public float intersection;

    public Animator anim;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isAttacking = false;

    public ObjectPool pool; 

    public GameObject bomb;

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
        anim.SetInteger("T90_anim", (int)1);
    }

    void Update()
    {

        if (intersection < distance && distance>25)
        {
            moveSpeed = 4.5f;
        }
        else
        {
            moveSpeed = 2;
        }

        if (HP < 0)
        {
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);
            StartCoroutine(pool.ReturnToPoolAfterDelay(0.3f));
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
            StartCoroutine(Fire12mm());
            if (!isAttacking)
            {
                StartCoroutine(FireAndWait());
                
            }
        }
        else if (distance > intersection)
        {
            isAttacking = false;
            anim.SetInteger("T90_anim", (int)1);
            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    public int ammo7_62mm = 1;
    public IEnumerator Fire12mm()
    {
        if (ammo7_62mm > 0)
        {
            ammo7_62mm = 0;
            
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.PKM);

            for (int i = 0; i < 19; i++)
            {
                GameObject muzzleFlash = ObjectPoolManager.Instance.GetObjectFromPool(muzzleFlashPrefab, Quaternion.identity,sizeOfMuzzleFlash12);
                muzzleFlash.transform.position = transform.position + locationOfMuzzleFlash12;

                GameObject bullet = ObjectPoolManager.Instance.GetObjectFromPool(bulletPrefab, Quaternion.identity, sizeOfBullet12);
                muzzleFlash.transform.position = transform.position + locationOfBullet12;
                yield return new WaitForSeconds(0.15f);
            }

            yield return new WaitForSeconds(6f);

            ammo7_62mm = 1;
        }
    }

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        anim.SetInteger("T90_anim", (int)-1);
        
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("T90_anim", (int)0);
        yield return new WaitForSeconds(AttackCoolTime);
        isAttacking = false;
        anim.SetInteger("T90_anim", (int)0);
    }
    public void MuzzleFlash()
    {
        GameObject t90muzzleFlash = ObjectPoolManager.Instance.GetObjectFromPool(T90muzzleFlashPrefab, Quaternion.identity, sizeOfMuzzleFlash);
        t90muzzleFlash.transform.position = transform.position + locationOfMuzzleFlash;
    }

    public void PlaySound()
    {
        audioSource.Play();
        GameObject t90_115mm = ObjectPoolManager.Instance.GetObjectFromPool(bomb, Quaternion.identity, sizeOfBomb);
        t90_115mm.transform.position = transform.position + locationOfBomb;
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
