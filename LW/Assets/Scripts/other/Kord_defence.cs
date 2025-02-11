using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Kord_defence : MonoBehaviour
{

    public Vector3 sizeOfBullet12;
    public Vector3 locationOfBullet12;

    public Vector3 sizeOfMuzzleFlash12;
    public Vector3 locationOfMuzzleFlash12;

    public GameObject bulletPrefab;
    public GameObject MuzzleFlashPrefab_12mm;

    float ammo12mm = 1;

    public float intersection;

    public string targetTag = "Enemy";

    public float distance = 80;

    public Animator anim;

    public float HealthPoint;

    public static class AN
    {
        public const int Fire = -1;
        public const int Idle = 0;
    }

    void Start()
    {
        HealthPoint = 10000;
        intersection = 50;
    }

    void Update()
    {
        if (Time.frameCount % 15 == 0)
        {
            FindClosestTarget();
        }

        if (distance < intersection)
        {
            
            StartCoroutine(Fire12mm());
        }
        else
        {
            anim.SetInteger("Kord", AN.Idle);
        }
    }

    public IEnumerator Fire12mm()
    {
        if (ammo12mm > 0)
        {
            anim.SetInteger("Kord", AN.Fire);
            ammo12mm = 0;
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.Kord);

            for (int i = 0; i < 11; i++)
            {
                GameObject b12mm = ObjectPoolManager.Instance.GetObjectFromPool(bulletPrefab, Quaternion.identity, sizeOfBullet12);
                GameObject bmuzzleflash = ObjectPoolManager.Instance.GetObjectFromPool(MuzzleFlashPrefab_12mm, Quaternion.identity, sizeOfMuzzleFlash12);
                b12mm.transform.position = transform.position + locationOfBullet12;
                bmuzzleflash.transform.position = transform.position + locationOfMuzzleFlash12;
                yield return new WaitForSeconds(0.1f);
            }
            anim.SetInteger("Kord", AN.Idle);
            yield return new WaitForSeconds(2f);

            ammo12mm = 1;
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

    public void MinusHP(float atk)
    {
        this.HealthPoint += atk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("E125mm"))
        {
            MinusHP(-500);
        }
        if (other.gameObject.CompareTag("E762"))
        {
            MinusHP(-20);
        }
    }
}
