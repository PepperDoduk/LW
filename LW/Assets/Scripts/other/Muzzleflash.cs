using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzleflash : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeSprite();
    }

    void ChangeSprite()
    {
        if (sprites.Length > 0)
        {
            int randomIndex = Random.Range(0, sprites.Length);

            spriteRenderer.sprite = sprites[randomIndex];
        }
    }
}
