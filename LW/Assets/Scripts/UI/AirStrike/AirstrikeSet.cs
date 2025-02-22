using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AirstrikeSet : MonoBehaviour
{
    [SerializeField] private int num;
    [SerializeField] private Airstrike_display display;

    public UnityEngine.UI.Image button;
    public TextMeshProUGUI text;
    public GameObject[] willShowUIs;

    public MoneyUI money;

    public int needMoney;

    public void SetNum()
    {
        if (money.ReturnMoney() - 1 > needMoney)
        {
            money.AddMoney(-needMoney);
            display.SetBombing(num);
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

            if (button != null && text != null)
                gameObject.SetActive(false);
        }
        else
        {
            money.NeedMoreMoney();
        }
    }
}
