using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private Pool[] pools;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary; // 프리팹을 키로 풀 관리

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool() //풀 초기화하기
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); //풀 사이즈만큼 프리팹을 만들어서
                obj.SetActive(false); // 싹다 비활성화해두고
                objectPool.Enqueue(obj); //해당 프리팹을 관리할 큐에 집어넣는다
            }

            poolDictionary.Add(pool.prefab, objectPool); // 각 프리팹에 해당하는 풀 저장
        }
    }

    public GameObject GetObjectFromPool(GameObject prefab, Quaternion rotation, Vector3 scale) // 순서대로 프리팹, 회전, 크기. 위치는 함수 호출할때 껴넣으니까 됐다!
    {
        if (poolDictionary.ContainsKey(prefab) && poolDictionary[prefab].Count > 0) // 아까 만든 딕셔너리 안에 오브젝트가 충분하다면
        {
            GameObject obj = poolDictionary[prefab].Dequeue(); // 큐에서 빼버리고
            obj.SetActive(true); // 활성화한다

            // 회전, 크기 설정
            obj.transform.rotation = rotation;
            obj.transform.localScale = scale;

            obj.GetComponent<ObjectPool>().prefab = prefab;
            obj.GetComponent<ObjectPool>().Init();
            return obj;
        }
        else
        {
            Debug.LogWarning("풀에 프리팹 부족, 새로 생성");

            GameObject obj = Instantiate(prefab);
            obj.transform.rotation = rotation;
            obj.transform.localScale = scale; 
            obj.GetComponent<ObjectPool>().prefab = prefab;
            obj.GetComponent<ObjectPool>().Init();
            return obj;
        }
    }

    private void ExpandPool(GameObject prefab, int amount) // 이건 아직 안씀. 프리팹 부족하면 늘리는 용도
    {
        if (poolDictionary.ContainsKey(prefab))
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);

                obj.GetComponent<ObjectPool>().prefab = prefab;

                poolDictionary[prefab].Enqueue(obj);
            }
        }
    }

    public void ReturnObjectToPool(GameObject prefab, GameObject obj) // 오브젝트를 풀에 다시 돌려주기
    {
        obj.GetComponent<ObjectPool>().Deactivate();
        obj.SetActive(false); // 풀에 갖다넣고 비활성화한다

        if (poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab].Enqueue(obj);
        }
        else
        {
            // 프리팹에 해당하는 풀이 없을 때는 새로 만든다
            Queue<GameObject> newPool = new Queue<GameObject>();
            newPool.Enqueue(obj);
            poolDictionary.Add(prefab, newPool);
        }
    }
}
