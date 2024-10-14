using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleBomb : MonoBehaviour
{
    public GameObject explosion;
    private Rigidbody2D rb;
    private bool hasFired = false;

    void Start()
    {
        if (!hasFired)
        {
            gameObject.SetActive(true);
            rb = GetComponent<Rigidbody2D>();
            StartCoroutine(Fire());
            hasFired = true;
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(9);
        Instantiate(explosion, new Vector3(transform.position.x, transform.position.y + 9f, 0), Quaternion.identity);
   
        gameObject.SetActive(false);
    }

}
