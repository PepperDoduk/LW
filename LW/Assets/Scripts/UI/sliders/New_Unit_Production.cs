using UnityEngine;
using UnityEngine.UI;

public class New_Unit_Production : MonoBehaviour
{
    public Button productionButton;
    public Vector3 unitLocation;
    public Vector3 unitSize;
    public GameObject unitPrefab;
    public int prod_needMoney;
    public float coolTime; 
    public MoneyUI money;

    void Start()
    {
        productionButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (money.ReturnMoney() >= prod_needMoney)
        {
            money.AddMoney(-prod_needMoney);
            UnitProductionManager.Instance.AddToQueue(this);
        }
        else
        {
            money.NeedMoreMoney();
        }
    }
}
