using UnityEngine;
using System.Collections;

public class RPG : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float moveSpeed = 0f;
    public float distance;
    public float intersection = 0f;

    public AudioSource audioSource;
    public Animator anim;

    public float HP;
    public float attackCoolTime;

    private bool isAttacking = false;
    int tmp = 0;

    public GameObject bomb;

    void Start()
    {
        anim = GetComponent<Animator>();
        HP = 250;
        moveSpeed = 1.4f;
        attackCoolTime = 4.3f;
        intersection = 26f;
        anim.SetInteger("RPG_anim", (int)1);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (HP < 0)
        {
            Destroy(gameObject, 0.3f);
        }
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTarget = target;
            }
        }

        distance = closestDistance;

        if (distance <= intersection && !isAttacking && closestTarget != null)
        {
            StartCoroutine(Fire());
        }
        else if (distance > intersection)
        {
            isAttacking = false;
            anim.SetInteger("RPG_anim", (int)1);
            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    IEnumerator Fire()
    {
        if (tmp == 0 && !isAttacking)
        {
            tmp = 1;
            isAttacking = true;
            anim.SetInteger("RPG_anim", (int)-1);

            float elapsedTime = 0f;
            float attackDuration = 0.8f;

            while (elapsedTime < attackDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            StartCoroutine(Wait());
            isAttacking = false;
        }
    }

    public void MinusHealthPoint(float atk)
    {
        this.HP += atk;
    }

    public IEnumerator Wait()
    {
        anim.SetInteger("RPG_anim", (int)0);
        yield return new WaitForSeconds(attackCoolTime);
        tmp = 0;
    }

    public void PlaySound()
    {
        audioSource.Play();
        Instantiate(bomb, new Vector3(transform.position.x, transform.position.y+0.5f, 0), Quaternion.identity);
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