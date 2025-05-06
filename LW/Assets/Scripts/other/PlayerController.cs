using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float hp;
    public float maxHp;
    private SpriteRenderer[] spriteRenderers;
    [SerializeField] private GameObject smokePrefab;
    void Start()
    {
        maxHp = 15000;
        hp = maxHp;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        ApplyDarkenEffect();
        if ((Time.frameCount % 50 == 0) && hp < 7000)
        {
            GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(smokePrefab, Quaternion.identity, new Vector3(1, 1, 1));
            smoke.transform.position = transform.position + new Vector3(-2, 3, 0);
        }
        if (hp < 0)
        {
            SceneManager.LoadScene("Defeat");
        }
    }

    void ApplyDarkenEffect()
    {
        float hpRatio = Mathf.Clamp01(hp / maxHp);
        if (hpRatio > 0.7f)
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
