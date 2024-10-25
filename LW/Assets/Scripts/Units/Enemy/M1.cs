using UnityEngine;
using System.Collections;

public class M1: MonoBehaviour
{
    public string targetTag = "Unit";
    public float moveSpeed = 0f;
    public float distance;
    public float intersection = 0f;

    public Animator anim;
    public GameObject bomb;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isAttacking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        HP = 6000;
        moveSpeed = 0.7f;
        AttackCoolTime = 7f;
        intersection = 14f;
        anim.SetInteger("M1_anim", (int)1);
    }

    void Update()
    {
        if (HP < 0.01)
        {
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);
            Destroy(gameObject,0.3f);
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
            anim.SetInteger("M1_anim", (int)1);
            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * -1 * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        anim.SetInteger("M1_anim", (int)-1);
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("M1_anim", (int)0);
        yield return new WaitForSeconds(AttackCoolTime);
        isAttacking = false;
        anim.SetInteger("M1_anim", (int)0);
    }

    public void PlaySound()
    {
        audioSource.Play();
        Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 2.0f, 0), Quaternion.identity);
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
        if (other.gameObject.CompareTag("210mm"))
        {
            MinusHealthPoint(-1000);
        }
        if (other.gameObject.CompareTag("35mm"))
        {
            MinusHealthPoint(-55);
        }

        if (other.gameObject.CompareTag("500KG"))
        {
            MinusHealthPoint(-10000);
        }
        if (other.gameObject.CompareTag("Rpg"))
        {
            MinusHealthPoint(-250);
        }
        if (other.gameObject.CompareTag("7.62mm"))
        {
            MinusHealthPoint(-10);
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.BULLET);
        }
    }
}
