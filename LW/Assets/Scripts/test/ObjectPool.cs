using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; 
    public float disableTime;  
    public void Init()
    {
        if(disableTime != -1)
            StartCoroutine(ReturnToPoolAfterDelay(disableTime));
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator ReturnToPoolAfterDelay(float t)
    {
            yield return new WaitForSeconds(t);
            ObjectPoolManager.Instance.ReturnObjectToPool(prefab, gameObject);
            gameObject.SetActive(false);
    }
}
