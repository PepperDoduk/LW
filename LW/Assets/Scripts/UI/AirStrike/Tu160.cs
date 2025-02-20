using UnityEngine;
using System.Collections;

public class Tu160 : MonoBehaviour
{
    public float moveSpeed;
    public ScreenShake shake;
    public GameObject target;
    private void OnEnable()
    {
        
        StartCoroutine(Bombing());
    }

    private void Update()
    {
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
