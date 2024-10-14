using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EAGLE500KGBOMB : MonoBehaviour
{
    public Sprite[] ImgAsset;
    private Image NowImage;
    public AudioManager audio;

    public void EAGLEStart()
    {
        gameObject.SetActive(true);
        NowImage = GetComponent<Image>();
        audio.PlayBgm(false);
        StartCoroutine(StartCountDown());
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
    IEnumerator StartCountDown()
    {
        
        AudioManager.instance.PlaySfx(AudioManager.Sfx.EAGLEBGM);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.EAGLE);
        yield return StartCoroutine(ChangeImagesWithDelay(0.2f));
        yield return StartCoroutine(ChangeImagesWithDelay(0.7f));
        yield return StartCoroutine(ChangeImagesWithDelay(1f));
        yield return StartCoroutine(ChangeImagesWithDelay(2.3f));
        gameObject.SetActive(false);
    }

    IEnumerator ChangeImagesWithDelay(float delay)
    {
        for (int i = 0; i < 4; ++i)
        {
            NowImage.sprite = ImgAsset[i];
            yield return new WaitForSeconds(delay);
        }
        NowImage.sprite = ImgAsset[4];
        yield return new WaitForSeconds(0.2f);
        NowImage.sprite = ImgAsset[5];
        yield return new WaitForSeconds(0.45f);
        NowImage.sprite = ImgAsset[6];
        yield return new WaitForSeconds(28f);
        audio.PlayBgm(true);
        gameObject.SetActive(false);
    }
}
