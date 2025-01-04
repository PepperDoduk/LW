using UnityEngine;

public class HQ : MonoBehaviour
{
    public GameObject HQ_Info;
    public float hp;


    void Start()
    {
        hp = 15000;
        if (HQ_Info != null)
        {
            HQ_Info.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (HQ_Info != null)
        {
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.MouseClick);
            HQ_Info.SetActive(true);
        }
    }
}
