using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject esc;
    public GameObject setting;
    public bool isEscPopup = false;
    void Start()
    {
        esc.SetActive(false);
        isEscPopup = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isEscPopup)
            {
                CloseEscMenu();
            }
            else
            {
                OpenEscMenu();
            }
        }
    }

    void OpenEscMenu()
    {
        esc.SetActive(true);
        isEscPopup = true;
        Time.timeScale = 0f;
        
    }

    void CloseEscMenu()
    {
        esc.SetActive(false);
        isEscPopup = false;
        Time.timeScale = 1f;
        
    }
}
