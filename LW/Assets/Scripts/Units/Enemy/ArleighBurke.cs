using UnityEngine;
using System.Collections;

public class ArleighBurke : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float moveSpeed = 0f;
    public float distance = 50;
    public float intersection = 0f;

    public Vector3 sizeOfMissile;
    public Vector3 locationOfBomb;

    public int animNum = 0;

    public int ammo12mm = 1;
    public int ammoStriker = 1;

    public Animator anim;
    public AudioSource audioSource;

    public float HP;
    public float AttackCoolTime;

    private bool isAttacking = false;

    public GameObject missilePrefab;

    public ObjectPool pool;

    public int ammo = 1;

    private bool isDied;
    private bool isMoving = false;

    void OnEnable()
    {
        isDied = false;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        distance = 800;
    }

    void Update()
    {
        if (HP < 0)
        {
            isDied = true;
            if (isDied)
                Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankDestroy);
            StartCoroutine(pool.ReturnToPoolAfterDelay(0f));
            isDied = false;
        }

        if (Time.frameCount % 15 == 0)
        {
            FindClosestTarget();
        }

        if (distance <= intersection)
        {
            //StartCoroutine(Fire12mm());

            if (!isAttacking && ammo > 0)
            {
                StartCoroutine(FireAndWait());
            }
        }
        else if (distance > intersection)
        {
            isAttacking = false;
            anim.SetInteger("ARMATA", animNum);

            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
    }

    void FindClosestTarget()
    {
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
    }

    IEnumerator FireAndWait()
    { 
        if (!isAttacking)
        {

            StartCoroutine (BGM109Launch());
        }
        else
        {
            StartCoroutine(Idle());
        }
        yield break;
    }

    IEnumerator Idle()
    {
        anim.SetInteger("ARMATA", animNum);
        yield return new WaitForSeconds(AttackCoolTime);
        isAttacking = false;
    }


    IEnumerator BGM109Launch()
    {
        if (ammo > 0)
        {
            ammo = 0;
            isAttacking = true;
            for (int i = 0; i < 3; ++i)
            {
                audioSource.Play();

                GameObject BGM109 = ObjectPoolManager.Instance.GetObjectFromPool(missilePrefab, Quaternion.identity, sizeOfMissile);
                BGM109.transform.position = transform.position + locationOfBomb;
                yield return new WaitForSeconds(4);
            }
            yield return new WaitForSeconds(AttackCoolTime);
            isAttacking = false;
            Reload();

        }
    }

    public void Reload()
    {
        ammo = 1;
    }

    public void MinusHP(float atk)
    {
        this.HP += atk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("E125mm"))
        {
            MinusHP(-500);
        }
    }
}
