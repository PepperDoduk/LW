using UnityEngine;

public class HideSetting : MonoBehaviour
{
    public GameObject[] uiObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject uiObject in uiObjects)
        {
            CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f; // �����ϰ�
                canvasGroup.interactable = false; // ��ġ ��Ȱ��ȭ
                canvasGroup.blocksRaycasts = false; // �̺�Ʈ ���� ��Ȱ��ȭ
            }
            else
            {
                Debug.LogWarning($"GameObject {uiObject.name}���� CanvasGroup�� �����ϴ�.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
