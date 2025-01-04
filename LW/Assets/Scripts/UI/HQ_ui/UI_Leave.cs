using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Leave : MonoBehaviour
{
    public UnityEngine.UI.Image button;
    public TextMeshProUGUI text;
    public GameObject[] willCloseUI;
    public void Click()
    {
        button.color = new Color(0.11f, 0.11f, 0.11f, 1);
        text.color = Color.white;
        Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.MouseClick);
        for (int i = 0; i < willCloseUI.Length; ++i)
        {
            willCloseUI[i].SetActive(false);
        }
        Time.timeScale = 1f;
    }
}
