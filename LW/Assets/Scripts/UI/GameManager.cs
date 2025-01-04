using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject esc;
    public bool isEscPopup = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isEscPopup)
            {
                esc.SetActive(false);
                Time.timeScale = 1f;
                
            }
            else
            {
                esc.SetActive(true);
                Time.timeScale = 0f;
                
            }
            isEscPopup = !isEscPopup;
        }
    }
}
