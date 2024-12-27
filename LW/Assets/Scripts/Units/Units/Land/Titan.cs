using UnityEngine;
using System.Collections;

public class Titan : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float moveSpeed = 0f;
    public float distance;
    public float intersection = 0f;

    public int animNum = 0;

    public int ammo35mm = 1;

    public Animator anim;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isAttacking = false;

    public GameObject bomb;
    public GameObject bullet;

    public int ammo = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        HP = 25000;
        moveSpeed = 0.35f;
        AttackCoolTime = 15f;
        intersection = 25f;
        animNum = 1;
        anim.SetInteger("titan_anim", animNum);
        ammo35mm = 1;
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
            anim.SetInteger("titan_anim", animNum);
            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    IEnumerator FireAndWait()
    {
        isAttacking = true;
        animNum = -1;
        anim.SetInteger("titan_anim", animNum);
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
        anim.SetInteger("titan_anim", animNum);
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
            Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 7f, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 7f, 0), Quaternion.identity);

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
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Titan35mm);

            for (int i = 0; i < 35; i++)
            {
                Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 5.6f, 0), Quaternion.Euler(0, 0, Random.Range(-0.3f, 0.3f)));
                yield return new WaitForSeconds(0.025f);
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
