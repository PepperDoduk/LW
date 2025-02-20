using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Product_slider : MonoBehaviour
{
    public Slider timerSlider;
    private float totalTime = 0f;
    private float elapsedTime = 0f;

    public void ResetTime()
    {
        timerSlider.value = 0;
        elapsedTime = 0;
    }

    public void SetTime(float t)
    {
        totalTime = t;
        ResetTime();
    }

    public IEnumerator StartTimer()
    {
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            timerSlider.value = elapsedTime / totalTime;
            yield return null;
        }
        ResetTime();
    }
}
