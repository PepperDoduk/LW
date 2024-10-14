using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_production : MonoBehaviour
{
    public Button productionButton;
    public GameObject Unit;
    public float x;
    public float y;
    public float z;

    public int prod_needMoney;

    public MoneyUI money;


    public float time = 0;
    public float Rtime = 0;

    private bool buttonActive = true;

    void Start()
    {
        Button btn = productionButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        time = Rtime;
    }

    void Update()
    {
        if (!buttonActive)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                productionButton.interactable = true;
                buttonActive = true;
                time = Rtime;
            }
        }
    }

    void TaskOnClick()
    {
        if (buttonActive) 
        {
            if (money.ReturnMoney() - 1 > prod_needMoney)
            {
                Debug.Log("Unit production Start!");
                money.AddMoney(-prod_needMoney);
                Instantiate(Unit, new Vector3(x, y, z), Quaternion.identity);
                Debug.Log("Unit production Success");

                productionButton.interactable = false;
                buttonActive = false;
            }
            else
            {
                money.NeedMoreMoney();
            }
        }
    }
}
