using TMPro;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public UnityEngine.UI.Image button;
    public TextMeshProUGUI text;
    public GameObject[] willShowUIs;
    public void ShowUIAndCloseItSelf()
    {
        if (button != null && text != null)
        {
            button.color = new Color(0.11f, 0.11f, 0.11f, 1);
            text.color = Color.white;
        }
        Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.MouseClick);
        for (int i = 0; i < willShowUIs.Length; ++i)
        {
            willShowUIs[i].SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
