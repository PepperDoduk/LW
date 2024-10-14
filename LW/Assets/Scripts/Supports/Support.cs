using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Support : MonoBehaviour
{
    public Button productionButton;
    public GameObject support;
    public float x;
    public float y;
    public float z;

    public int prod_needMoney;

    Vector3 mousePos;

    public MoneyUI money;
    public supportGuide sup;

    void Start()
    {
        Button btn = productionButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
    }

    void TaskOnClick()
    {
        if (money.ReturnMoney() - 1 > prod_needMoney)
        {
            Debug.Log("Supporting Start!");
            money.AddMoney(-prod_needMoney);
            sup.StartGuide();   
            Debug.Log("Supporting Success");
        }
        else
        {
            money.NeedMoreMoney();
        }
    }

}
