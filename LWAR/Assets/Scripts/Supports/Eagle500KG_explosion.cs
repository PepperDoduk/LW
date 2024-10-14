using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle500KG_explosion : MonoBehaviour
{
    public float fadeDuration = 1f;
    private float fadeTimer = 0f;
    private Renderer objectRenderer;
    public Animator animator;
    private bool isPlaying = false;

    private void Start()
    {
    }

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            fadeTimer += Time.deltaTime *8;

            if (fadeTimer < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
                Color newColor = objectRenderer.material.color;
                newColor.a = alpha;
                objectRenderer.material.color = newColor;
            }
            else
            {
                gameObject.SetActive(false);
                isPlaying = false;
            }
        }
    }

    public void SetTmp()
    {
        if (!isPlaying)
        {
            
            StartCoroutine(Fire());
        }
    }

    void PlayAnimation()
    {

        animator.SetTrigger("PlayAnimation");
    }

    IEnumerator Fire()
    {
        isPlaying = true;
        PlayAnimation();
        yield return new WaitForSeconds(fadeDuration);
        isPlaying = false;
    }

    public void PlaySound()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.EAGLE_EXPLOSION);
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
