using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stratagem : MonoBehaviour
{

    public GameObject bomb;
    public GameObject guideLaser;

    private bool laserStarted = false;

    void Start()
    {
        if (!laserStarted)
        {
            StartCoroutine(Laser());
            laserStarted = true;
        }
    }

    void Update()
    {

    }

    IEnumerator Laser()
    {
        yield return new WaitForSeconds(4.3f);
        yield return new WaitForSeconds(3);

        InstantiateBombOnce();

        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

    private void InstantiateBombOnce()
    {
        if (bomb != null)
        {
            Debug.Log("Bomb instantiated");
            Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 30f, 0), Quaternion.identity);
            StartCoroutine(WaitAndDestroy());
            bomb = null;
        }
        else
        {
            Debug.Log("Bomb is null");
        }
    }

    IEnumerator WaitAndDestroy()
    {

        Instantiate(guideLaser, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

}
