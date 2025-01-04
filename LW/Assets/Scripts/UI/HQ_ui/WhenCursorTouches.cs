using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WhenCursorTouches : MonoBehaviour
{
    public UnityEngine.UI.Image button;
    public TextMeshProUGUI text;

    void Start()
    {
        Debug.Log(button.color);    
    }
    public void OnMouseEnter()
    {
        Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.MouseEnter);
        button.color = Color.white;
        text.color = new Color(0.11f, 0.11f, 0.11f, 1);
    }

    public void OnMouseExit()
    {
        button.color = new Color(0.11f,0.11f,0.11f,1);
        text.color = Color.white;
    }
}
