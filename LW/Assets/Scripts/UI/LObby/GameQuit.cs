using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameQuit : MonoBehaviour
{
    public UnityEngine.UI.Image button;
    public TextMeshProUGUI text;
    public void ClickToGameQuit()
    {
        button.color = new Color(0.11f, 0.11f, 0.11f, 1);
        text.color = Color.white;
        Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.MouseClick);
        Application.Quit();
    }

    public void ClickToGoLobby()
    {
        button.color = new Color(0.11f, 0.11f, 0.11f, 1);
        text.color = Color.white;
        Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.MouseClick);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lobby");
    }
}
