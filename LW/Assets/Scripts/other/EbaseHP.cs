using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EbaseHP : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    [SerializeField] private GameObject smokePrefab;
    public float hp;
    public float maxHp;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 100000;
        hp = maxHp;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyDarkenEffect();
        if ((Time.frameCount % 50 == 0) && hp < 50000)
        {
            GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(smokePrefab, Quaternion.identity, new Vector3(1, 1, 1));
            smoke.transform.position = transform.position + new Vector3(-2, 3, 0);
        }
        if (hp < 0)
        {
            SceneManager.LoadScene("Victory");
        }
    }

    void ApplyDarkenEffect()
    {
        float hpRatio = Mathf.Clamp01(hp / maxHp);
        if (hpRatio > 0.8f)
        {
            SetColor(Color.white);
            return;
        }
        float normalizedRatio = Mathf.Clamp01(hpRatio / 0.7f);
        float darkenAmount = Mathf.Lerp(1f, 0.3f, 1 - normalizedRatio);

        Color darkenColor = new Color(darkenAmount, darkenAmount, darkenAmount, 1f);
        SetColor(darkenColor);
    }

    void SetColor(Color color)
    {
        foreach (var renderer in spriteRenderers)
        {
            renderer.color = color;
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
