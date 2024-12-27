using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform;   
    public float shakeDuration = 0.5f;  // 흔들림 지속 시간
    public float shakeMagnitude = 0.2f; // 흔들림 강도
    public float dampingSpeed = 1.0f;   // 흔들림 감소 속도
    public bool includeRotation = false; 

    private Vector3 initialPosition;    
    private Quaternion initialRotation; 

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        initialPosition = cameraTransform.localPosition;
        initialRotation = cameraTransform.localRotation;
    }

    public void SetShake(float duration, float magnitude, float damping)
    {
        this.shakeDuration = duration;
        this.shakeMagnitude = magnitude;
        this.dampingSpeed = damping;
    }

    public void TriggerShake(float duration = -1f, float magnitude = -1f)
    {
        if (duration > 0) shakeDuration = duration;
        if (magnitude > 0) shakeMagnitude = magnitude;

        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float currentShakeMagnitude = shakeMagnitude * (1.0f - (elapsed / shakeDuration));

            Vector3 currentPosition = cameraTransform.localPosition;
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f) * currentShakeMagnitude,
                Random.Range(-1f, 1f) * currentShakeMagnitude,
                0);

            float randomRotation = includeRotation
                ? Random.Range(-1f, 1f) * currentShakeMagnitude * 10f : 0;

            cameraTransform.localPosition = currentPosition + randomOffset;

            if (includeRotation)
            {
                cameraTransform.localRotation = initialRotation * Quaternion.Euler(0, 0, randomRotation);
            }

            elapsed += Time.deltaTime;

            yield return null;
        }

    }
}
