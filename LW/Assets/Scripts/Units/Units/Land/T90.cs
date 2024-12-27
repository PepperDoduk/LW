using UnityEngine;
using System.Collections;

public class T90 : MonoBehaviour
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
            AudioManager.instance.PlaySfx(AudioManager.Sfx.TankDestroy);
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
            anim.SetInteger("T90_anim", (int)1);
            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
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

    public void PlaySound()
    {
        audioSource.Play();
        Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 1.62f, 3), Quaternion.identity);
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
