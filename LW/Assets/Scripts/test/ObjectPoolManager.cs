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
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary; // �������� Ű�� Ǯ ����

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

    private void InitializePool() //Ǯ �ʱ�ȭ�ϱ�
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); //Ǯ �����ŭ �������� ����
                obj.SetActive(false); // �ϴ� ��Ȱ��ȭ�صΰ�
                objectPool.Enqueue(obj); //�ش� �������� ������ ť�� ����ִ´�
            }

            poolDictionary.Add(pool.prefab, objectPool); // �� �����տ� �ش��ϴ� Ǯ ����
        }
    }

    public GameObject GetObjectFromPool(GameObject prefab, Quaternion rotation, Vector3 scale) // ������� ������, ȸ��, ũ��. ��ġ�� �Լ� ȣ���Ҷ� �������ϱ� �ƴ�!
    {
        if (poolDictionary.ContainsKey(prefab) && poolDictionary[prefab].Count > 0) // �Ʊ� ���� ��ųʸ� �ȿ� ������Ʈ�� ����ϴٸ�
        {
            GameObject obj = poolDictionary[prefab].Dequeue(); // ť���� ��������
            obj.SetActive(true); // Ȱ��ȭ�Ѵ�

            // ȸ��, ũ�� ����
            obj.transform.rotation = rotation;
            obj.transform.localScale = scale;

            obj.GetComponent<ObjectPool>().prefab = prefab;
            obj.GetComponent<ObjectPool>().Init();
            return obj;
        }
        else
        {
            Debug.LogWarning("Ǯ�� ������ ����, ���� ����");

            GameObject obj = Instantiate(prefab);
            obj.transform.rotation = rotation;
            obj.transform.localScale = scale; 
            obj.GetComponent<ObjectPool>().prefab = prefab;
            obj.GetComponent<ObjectPool>().Init();
            return obj;
        }
    }

    private void ExpandPool(GameObject prefab, int amount) // �̰� ���� �Ⱦ�. ������ �����ϸ� �ø��� �뵵
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

    public void ReturnObjectToPool(GameObject prefab, GameObject obj) // ������Ʈ�� Ǯ�� �ٽ� �����ֱ�
    {
        obj.GetComponent<ObjectPool>().Deactivate();
        obj.SetActive(false); // Ǯ�� ���ٳְ� ��Ȱ��ȭ�Ѵ�

        if (poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab].Enqueue(obj);
        }
        else
        {
            // �����տ� �ش��ϴ� Ǯ�� ���� ���� ���� �����
            Queue<GameObject> newPool = new Queue<GameObject>();
            newPool.Enqueue(obj);
            poolDictionary.Add(prefab, newPool);
        }
    }
}
