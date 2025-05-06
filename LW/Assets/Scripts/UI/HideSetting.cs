using UnityEngine;

public class HideSetting : MonoBehaviour
{
    public GameObject[] uiObjects;
    
    void Start()
    {
        foreach (GameObject uiObject in uiObjects)
        {
            CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false; 
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                Debug.LogWarning($"GameObject {uiObject.name}���� CanvasGroup�� �����ϴ�.");
            }
        }
    }

    void Update()
    {
        
    }
}
