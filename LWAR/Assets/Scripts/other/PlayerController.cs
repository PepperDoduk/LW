using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
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
            SceneManager.LoadScene("Defeat");
        }
    }

    public void MinusHealthPoint(float atk)
    {
        this.hp += atk;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("E125mm"))
        {
            MinusHealthPoint(-500);
        }
        if (other.gameObject.CompareTag("500KG"))
        {
            MinusHealthPoint(-10000);
        }
        if (other.gameObject.CompareTag("E762"))
        {
            MinusHealthPoint(-30);
        }
    }

    public float ReturnHp()
    {
        return hp;
    }
}
