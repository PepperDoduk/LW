using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class T90_test : MonoBehaviour
{
    //¬¤¬Ñ¬Ù¬Ú¬â¬à¬Ó¬Ñ¬ß¬ß¬Ñ¬ñ ¬Ó¬à¬Õ¬Ñ ¬Ú ¬ã¬í¬â¬Ñ¬ñ ¬â¬í¬Ò¬Ñ ¬à¬é¬Ö¬ß¬î ¬Ó¬Ü¬å¬ã¬ß¬í.
    [SerializeField] private GameObject T90muzzleFlashPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;

    public string targetTag = "Enemy";
    public float moveSpeed = 0f;
    public float distance;
    public float intersection = 0f;

    public Animator anim;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isAttacking = false;

    public GameObject bomb;
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        HP = 3000;
        moveSpeed = 0.8f;
        AttackCoolTime = 3f;
        intersection = 10f;
        anim.SetInteger("T90_anim", (int)1);
    }

    void Update()
    {

        if (HP < 0)
        {
            //AudioManager.instance.PlaySfx(AudioManager.Sfx.TankDestroy);
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

    public int ammo35mm = 1;
    public IEnumerator Fire12mm()
    {
        if (ammo35mm > 0)
        {
            ammo35mm = 0;
            //AudioManager.instance.PlaySfx(AudioManager.Sfx.PKM);
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.PKM);

            for (int i = 0; i < 19; i++)
            {
                //Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 5.6f, 0), Quaternion.Euler(0, 0, Random.Range(-0.3f, 0.3f)));
                GameObject muzzleFlash = ObjectPoolManager.Instance.GetObjectFromPool(muzzleFlashPrefab, Quaternion.identity, new Vector3(0.6f, 0.6f, 1));
                muzzleFlash.transform.position = new Vector3(transform.position.x + 0.6f, transform.position.y + 2.5f, -2);
                yield return new WaitForSeconds(0.15f);
            }

            yield return new WaitForSeconds(6f);

            ammo35mm = 1;
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
        GameObject t90muzzleFlash = ObjectPoolManager.Instance.GetObjectFromPool(T90muzzleFlashPrefab, Quaternion.identity, new Vector3(0.4f, 0.2f, 1));
        t90muzzleFlash.transform.position = new Vector3(transform.position.x + 2.45f, transform.position.y + 1.53f, 0);
    }

    public void PlaySound()
    {
        audioSource.Play();
        Instantiate(bomb, new Vector3(transform.position.x+2, transform.position.y + 1.62f, 3), Quaternion.identity);
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
