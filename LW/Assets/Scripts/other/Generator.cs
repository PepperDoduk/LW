using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{

    public float hp;
    // Start is called before the first frame update
    void Start()
    {
        hp = 15000;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            SceneManager.LoadScene("Victory");
        }
    }

    public void MinusHealthPoint(float atk)
    {
        this.hp += atk;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("125mm"))
        {
            MinusHealthPoint(-600);
        }
        if (other.gameObject.CompareTag("115mm"))
        {
            MinusHealthPoint(-450);
        }

        if (other.gameObject.CompareTag("210mm"))
        {
            MinusHealthPoint(-750);
        }

        if (other.gameObject.CompareTag("500KG"))
        {
            MinusHealthPoint(-10000);
        }

        if (other.gameObject.CompareTag("Rpg"))
        {
            MinusHealthPoint(-400);
        }
        if (other.gameObject.CompareTag("7.62mm"))
        {
            MinusHealthPoint(-40);
        }
    }

    public float ReturnHp()
    {
        return hp;
    }
}
