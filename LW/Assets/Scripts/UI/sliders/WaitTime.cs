using UnityEngine;
using UnityEngine.UI;

public class WaitTime : MonoBehaviour
{
    public Slider timerSlider; 
    public float totalTime = 30f; 
    private float elapsedTime = 0f;

    private void Start()
    {
        timerSlider.value = 0; 
    }

    private void Update()
    {
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            timerSlider.value = elapsedTime / totalTime;
        }
    }
}
