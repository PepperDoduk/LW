using UnityEngine;

public class UI_closeButton : MonoBehaviour
{
    public void DisableParentObject()
    {
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("이 오브젝트에는 부모가 없습니다.");
        }
    }
}
