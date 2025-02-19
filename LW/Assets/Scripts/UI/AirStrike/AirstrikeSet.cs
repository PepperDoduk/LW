using UnityEngine;

public class AirstrikeSet : MonoBehaviour
{
    [SerializeField] private int num;
    [SerializeField] private Airstrike_display display;
    void Start()
    {
        
    }

    public void SetNum()
    {
        display.SetBombing(num);
    }
}
