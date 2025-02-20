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
                canvasGroup.alpha = 0f; // 투명하게
                canvasGroup.interactable = false; // 터치 비활성화
                canvasGroup.blocksRaycasts = false; // 이벤트 차단 비활성화
            }
            else
            {
                Debug.LogWarning($"GameObject {uiObject.name}에는 CanvasGroup이 없습니다.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
