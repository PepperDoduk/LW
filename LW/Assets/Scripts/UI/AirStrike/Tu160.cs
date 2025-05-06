using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class Tu160 : MonoBehaviour
{
    public float moveSpeed;
    public ScreenShake shake;
    public GameObject target;
    public AudioSource audioSource;

    GameObject sliderObject;
    private Slider slider;
    private void OnEnable()
    {
        sliderObject = GameObject.Find("sfxSlider");
        slider = sliderObject.GetComponent<Slider>();
        StartCoroutine(Bombing());
    }

    private void Update()
    {
        if (slider != null)
        {
            audioSource.volume = 0.3f * slider.value;
        }
        Vector3 nowPosition = transform.position;
        nowPosition.x += moveSpeed * Time.deltaTime;
        transform.position = nowPosition;

        if(target.transform.position.x == transform.position.x)
        {
            shake.SetShake(0.7f, 0.8f, 2f);
            shake.TriggerShake();
        }
    }

    public IEnumerator Bombing()
    {
        moveSpeed = 0;
        yield return new WaitForSeconds(5);

        moveSpeed = 250;
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
