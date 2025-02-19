using UnityEngine;
using System.Collections;

public class M16: MonoBehaviour
{
    public string targetTag = "Unit";
    public float moveSpeed = 0f;
    public float distance;
    public float intersection = 0f;

    public Animator anim;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isAttacking = false;
    public GameObject bullet;
    public ObjectPool pool;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        HP = 250;
        AttackCoolTime = 2f;
        intersection = 6f;
        anim.SetInteger("M16_anim", (int)1);
    }

    void Update()
    {
        if (HP < 0)
        {
            //Destroy(gameObject,0.2f);
            StartCoroutine(pool.ReturnToPoolAfterDelay(0.2f));

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
            if (!isAttacking)
            {
                StartCoroutine(FireAndWait());
            }
        }
        else if (distance > intersection)
        {
            isAttacking = false;
            anim.SetInteger("M16_anim", (int)1);
            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed* -1 * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        anim.SetInteger("M16_anim", (int)-1);
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("M16_anim", (int)0);
        yield return new WaitForSeconds(AttackCoolTime);
        isAttacking = false;
        anim.SetInteger("M16_anim", (int)0);
    }

    public void PlaySound()
    {
        audioSource.Play();
        StartCoroutine(Shoot());
    }

    public void MinusHealthPoint(float atk)
    {
        this.HP += atk;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("125mm"))
        {
            MinusHealthPoint(-500);
        }
        if (other.gameObject.CompareTag("115mm"))
        {
            MinusHealthPoint(-350);
        }

        if (other.gameObject.CompareTag("500KG"))
        {
            MinusHealthPoint(-10000);
        }
        if (other.gameObject.CompareTag("210mm"))
        {
            MinusHealthPoint(-1000);
        }

        if (other.gameObject.CompareTag("Rpg"))
        {
            MinusHealthPoint(-150);
        }
        if (other.gameObject.CompareTag("7.62mm"))
        {
            MinusHealthPoint(-40);
        }
        if (other.gameObject.CompareTag("12mm"))
        {
            MinusHealthPoint(-65);
        }
        if (other.gameObject.CompareTag("35mm"))
        {
            MinusHealthPoint(-105);
        }
        if (other.gameObject.CompareTag("152mmHE"))
        {
            MinusHealthPoint(-5500);
        }
        if (other.gameObject.CompareTag("30mmHE"))
        {
            MinusHealthPoint(-150);
        }
        if (other.gameObject.CompareTag("striker"))
        {
            MinusHealthPoint(-450);
        }

        if (other.gameObject.CompareTag("Kh38"))
        {
            MinusHealthPoint(-3000);
        }
    }
    IEnumerator Shoot()
    {
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.07f);
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.07f);
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
    }
}
