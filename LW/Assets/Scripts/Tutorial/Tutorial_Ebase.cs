using UnityEngine;
using System.Collections;

public class Tutorial_Ebase : MonoBehaviour
{
    public GameObject[] Units;
    public float x;
    public float y;
    public float z;
    public Vector3[] unitSize;
    public Vector3[] unitLocation;

    private GameObject[] unit;

    public double unitValue;

    void Start()
    {
        //InvokeRepeating("Enemy", 1f, Random.Range(2f, 7.5f));
        Invoke("Enemy", 1f);
    }

    void Enemy()
    {
        int randomValue = Random.Range(0, 100);

        int unitIndex;

        if (randomValue < 82)
            unitIndex = 0;
        else if (randomValue < 92)
            unitIndex = 1;
        else
            unitIndex = 2;

        if (Units[unitIndex] == null)
        {
            return;
        }

        // GameObject unit = ObjectPoolManager.Instance.GetObjectFromPool(Units[unitIndex], Quaternion.identity, unitSize[unitIndex]);
        // unit.transform.position = transform.position;

        Instantiate(Units[unitIndex], transform.position + unitLocation[unitIndex], Quaternion.identity);

        Invoke("Enemy", Random.Range(2f, 7.5f));
    }
}
