using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProductionManager : MonoBehaviour
{
    private Queue<New_Unit_Production> productionQueue = new Queue<New_Unit_Production>(); //  전역 생산 큐
    private bool isProducing = false; //  현재 생산 중인지 체크
    public Product_slider slider; // 모든 유닛의 쿨타임을 하나의 슬라이더로 표시

    public static UnitProductionManager Instance; // 싱글톤 패턴 사용

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddToQueue(New_Unit_Production unit)
    {
        productionQueue.Enqueue(unit); //  생산 요청을 큐에 추가
        if (!isProducing)
        {
            StartCoroutine(ProcessProductionQueue()); //  생산 코루틴 시작
        }
    }

    private IEnumerator ProcessProductionQueue()
    {
        isProducing = true;
        while (productionQueue.Count > 0)
        {
            New_Unit_Production currentUnit = productionQueue.Dequeue();
            slider.SetTime(currentUnit.coolTime); //  슬라이더 설정
            yield return slider.StartTimer(); //  슬라이더가 끝날 때까지 대기

            //  유닛 생성
            GameObject unit = ObjectPoolManager.Instance.GetObjectFromPool(currentUnit.unitPrefab, Quaternion.identity, currentUnit.unitSize);
            unit.transform.position = currentUnit.unitLocation;
            Debug.Log($"{currentUnit.gameObject.name} - Unit production Success");
        }
        isProducing = false;
    }
}
