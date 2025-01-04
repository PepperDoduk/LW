using UnityEngine;

public class PopUpUI_Product : MonoBehaviour
{
    public GameObject[] ProductionButton;

    private void Start()
    {
        for (int i = 0; i < ProductionButton.Length; ++i)
        {
            ProductionButton[i].SetActive(false);
        }
    }
    public void Clicked()
    {
        for(int i=0; i< ProductionButton.Length; ++i)
        {
            ProductionButton[i].SetActive(true);
        }
    }
}
