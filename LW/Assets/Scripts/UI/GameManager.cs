using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject esc;
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
        Time.timeScale = 0f;
        isEscPopup = true;
    }

    void CloseEscMenu()
    {
        esc.SetActive(false);
        Time.timeScale = 1f;
        isEscPopup = false;
    }
}
