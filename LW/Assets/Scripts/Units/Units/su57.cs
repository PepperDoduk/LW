using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class su57 : MonoBehaviour
{
    public Animator anim;
    public string targetTag = "Enemy";
    public string LandTag = "Airbase";

    public bool isFlying;
    public float bomb;
    public float missile;
    public float ammo;
    public float flare;

    public float moveSpeed;

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject bulletPrefab;

    public float distance;
    public float distanceToBase;
    public float intersection;

    public int animNum ;
    void Awake()
    {
        bomb = 5;
        missile = 6;
        ammo = 100;
        flare = 3;

        moveSpeed = 35;

        intersection = 70;
        animNum = -2;
    }

    void Update()
    {
        anim.SetInteger("su57", animNum);
        if (isFlying)
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

            GameObject[] bases = GameObject.FindGameObjectsWithTag(targetTag);
            float closestDistanceToBase = Mathf.Infinity;

            foreach (GameObject target in targets)
            {
                float dist = Vector3.Distance(transform.position, target.transform.position);
                if (dist < closestDistanceToBase)
                {
                    closestDistanceToBase = dist;
                }
            }

            distance = closestDistanceToBase;


            Vector3 nowPosition = transform.position;
            nowPosition.x += moveSpeed * Time.deltaTime;
            transform.position = nowPosition;
        }
        else if (!isFlying)
        {
            moveSpeed = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Airbase"))
        {
            isFlying = false;
            StartCoroutine("Landed");
        }
    }

    public IEnumerator Landed()
    {
        yield return new WaitForSeconds(6);
        missile = 4;
        ammo = 100;
        bomb = 6;
        flare = 3;
        yield return new WaitForSeconds(3);
    } 
    public IEnumerator Bording()
    {
        yield return new WaitForSeconds(0f);

    }

}
