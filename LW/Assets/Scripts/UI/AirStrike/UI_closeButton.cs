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
            Debug.LogWarning("�� ������Ʈ���� �θ� �����ϴ�.");
        }
    }
}
