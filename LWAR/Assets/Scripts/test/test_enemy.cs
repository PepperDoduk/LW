using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_enemy : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("115mm"))
        {
            GameObject explosion = ObjectPoolManager.Instance.GetObjectFromPool(explosionPrefab, Quaternion.identity, new Vector3(1f, 1f, 1));
            explosion.transform.position = new Vector3(transform.position.x - 1, transform.position.y , 0); 
        }
    }
    }
