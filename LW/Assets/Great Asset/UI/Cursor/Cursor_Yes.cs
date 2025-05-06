using UnityEngine;

public class Cursor_Yes : MonoBehaviour
{
    public UICursorImage ui;
    public void OnClick()
    {
        ui.isImageUI = true;
    }
}
