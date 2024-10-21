using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;  // 반환할 프리팹
    public float disableTime;  // 비활성화까지의 대기 시간

    // 풀에서 오브젝트를 가져와 초기화할 때 호출
    public void Init()
    {
        // disableTime 후 풀로 반환
        if (disableTime != -1)
        {
            StartCoroutine(ReturnToPoolAfterDelay(disableTime));
        }
        else if(disableTime == -1)
        {
           
        }
    }

    // 오브젝트 비활성화
    public void Deactivate()
    {
        ObjectPoolManager.Instance.ReturnObjectToPool(prefab, gameObject);
        gameObject.SetActive(false);
        
    }

    // 일정 시간이 지나면 풀로 반환함
    private IEnumerator ReturnToPoolAfterDelay(float t)
    {
        yield return new WaitForSeconds(t);

        // 풀로 오브젝트 갖다넣기
        ObjectPoolManager.Instance.ReturnObjectToPool(prefab, gameObject);
    }
}
