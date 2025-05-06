using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProductionManager : MonoBehaviour
{
    private Queue<New_Unit_Production> productionQueue = new Queue<New_Unit_Production>();
    private bool isProducing = false;
    public Product_slider slider;

    public static UnitProductionManager Instance;

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
        productionQueue.Enqueue(unit);
        if (!isProducing)
        {
            StartCoroutine(ProcessProductionQueue());
        }
    }

    private IEnumerator ProcessProductionQueue()
    {
        isProducing = true;
        while (productionQueue.Count > 0)
        {
            New_Unit_Production currentUnit = productionQueue.Dequeue();
            slider.SetTime(currentUnit.coolTime);
            yield return slider.StartTimer();

            GameObject unit = ObjectPoolManager.Instance.GetObjectFromPool(currentUnit.unitPrefab, Quaternion.identity, currentUnit.unitSize);
            unit.transform.position = currentUnit.unitLocation;
            Debug.Log($"{currentUnit.gameObject.name} - Unit production Success");
        }
        isProducing = false;
    }
}
