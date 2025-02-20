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
        hp = 100000;
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
            MinusHealthPoint(-750);
        }
        if (other.gameObject.CompareTag("115mm"))
        {
            MinusHealthPoint(-550);
        }
        if (other.gameObject.CompareTag("210mm"))
        {
            MinusHealthPoint(-1050);
        }
        if (other.gameObject.CompareTag("35mm"))
        {
            MinusHealthPoint(-55);
        }
        if (other.gameObject.CompareTag("Kh38"))
        {
            MinusHealthPoint(-6500);
        }
        if (other.gameObject.CompareTag("30mmHE"))
        {
            MinusHealthPoint(-45);
        }
        if (other.gameObject.CompareTag("striker"))
        {
            MinusHealthPoint(-450);
        }

        if (other.gameObject.CompareTag("Rpg"))
        {
            MinusHealthPoint(-250);
        }
        if (other.gameObject.CompareTag("12mm"))
        {
            MinusHealthPoint(-30);
        }
        if (other.gameObject.CompareTag("152mmHE"))
        {
            MinusHealthPoint(-4500);
        }
        if (other.gameObject.CompareTag("7.62mm"))
        {
            MinusHealthPoint(-10);
        }
        if (other.gameObject.CompareTag("TacNuke"))
        {
            MinusHealthPoint(-35000);
        }
        if (other.gameObject.CompareTag("AerialBomb"))
        {
            MinusHealthPoint(-5000);
        }

    }
        public float ReturnHp()
        {
            return hp;
        }
    }
