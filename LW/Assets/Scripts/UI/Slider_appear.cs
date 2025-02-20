using UnityEngine;

public class Slider_appear : MonoBehaviour
{
    public GameObject[] uiObjects;
    public void OnClick()
    {
        foreach (GameObject uiObject in uiObjects)
        {
            CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                Debug.LogWarning($"GameObject {uiObject.name}에는 CanvasGroup이 없습니다.");
            }
        }
    }
}
