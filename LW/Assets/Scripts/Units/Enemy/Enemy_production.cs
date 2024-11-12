using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_production : MonoBehaviour
{
    public GameObject Unit1;
    public GameObject Unit2;
    public float x;
    public float y;
    public float z;

    void Start()
    {
        InvokeRepeating("Enemy", 1f, Random.Range(5f, 15f));
    }

    void Enemy()
    {
        int randomValue = Random.Range(1, 101); 

        if (randomValue <= 79)
        {
            Instantiate(Unit1, new Vector3(transform.position.x + x, transform.position.y + y, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(Unit2, new Vector3(transform.position.x + x, transform.position.y + y, 0), Quaternion.identity);
        }
    }
}
