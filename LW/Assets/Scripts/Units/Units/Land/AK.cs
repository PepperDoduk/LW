using UnityEngine;
using System.Collections;

public class AK : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float moveSpeed = 0f;
    public float distance;
    public float intersection = 0f;

    public Animator anim;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isAttacking = false;
    public GameObject bullet;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        HP = 250;
        moveSpeed = 1.5f;
        AttackCoolTime = 1.1f;
        intersection = 20f;
        anim.SetInteger("AK_anim", (int)1);
    }

    void Update()
    {
        if (HP < 0)
        {
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
            if (!isAttacking)
            {
                StartCoroutine(FireAndWait());
            }
        }
        else if (distance > intersection)
        {
            isAttacking = false;
            anim.SetInteger("AK_anim", (int)1);
            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        anim.SetInteger("AK_anim", (int)-1);
        yield return new WaitForSeconds(0.37f);
        anim.SetInteger("AK_anim", (int)0);
        
        isAttacking = false;
        anim.SetInteger("AK_anim", (int)0);
    }

    public void PlaySound()
    {
        audioSource.Play();
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.07f);
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.07f);
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
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
            MinusHealthPoint(-40);
        }
    }
}
