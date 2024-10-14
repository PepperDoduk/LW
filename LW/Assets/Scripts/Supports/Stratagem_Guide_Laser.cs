using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stratagem_Guide_Laser : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Hide());
    }


    void Update()
    {
        
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
