using UnityEngine;

public class Slider_disappear : MonoBehaviour
{
    public GameObject[] uiObjects;
    public GameObject[] leave;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
            if (leave != null)
            {
                for(int i=0; i<leave.Length; ++i)
                leave[i].SetActive(false);
            }
        }
    }
        public void OnClick()
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
}
