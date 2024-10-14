using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;  // ��ȯ�� ������
    public float disableTime;  // ��Ȱ��ȭ������ ��� �ð�

    // Ǯ���� ������Ʈ�� ������ �ʱ�ȭ�� �� ȣ��
    public void Init()
    {
        // disableTime �� Ǯ�� ��ȯ
        StartCoroutine(ReturnToPoolAfterDelay(disableTime));
    }

    // ������Ʈ ��Ȱ��ȭ
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // ���� �ð��� ������ Ǯ�� ��ȯ��
    private IEnumerator ReturnToPoolAfterDelay(float t)
    {
        yield return new WaitForSeconds(t);

        // Ǯ�� ������Ʈ ���ٳֱ�
        ObjectPoolManager.Instance.ReturnObjectToPool(prefab, gameObject);
    }
}
