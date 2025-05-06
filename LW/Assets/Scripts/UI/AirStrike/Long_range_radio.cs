using UnityEngine;

public class Long_range_radio : MonoBehaviour
{
    public UnityEngine.UI.Image button;
    public float nowY;
    public float targetY;
    public float elapsed;
    public float duration;

    public GameManager manager; 

    public bool mouseExit;
    public bool isClick;

    public float up;

    public GameObject[] willShowUIs;

    public Vector3 newY;
    public float originalY;
    void Start()
    {
        originalY = transform.position.y;
        targetY = transform.position.y;
        nowY = transform.position.y;
        isClick = false;    
        mouseExit = true;    
    }
    void Update()
    {

        elapsed += Time.deltaTime;
        float newY = Mathf.Lerp(nowY, targetY, elapsed / duration);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);


    }
    public void OnMouseEnter()
    {
        if (!manager.isEscPopup)
        {
            mouseExit = false;
            nowY = originalY;
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.RadioOn);
            targetY = originalY + up;
            elapsed = 0;
        }
        
    }

    public void OnMouseExit()
    {
        if (!manager.isEscPopup)
        {
            mouseExit = true;
            if (mouseExit && !isClick)
            {
                ExitMouse();
            }
        }
        
    }

    private void ExitMouse() 
    {
        if (!manager.isEscPopup)
        {
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.RadioEnd);
            nowY = originalY + up;
            elapsed = 0;
            targetY = originalY;
        }
        
    }

    public void Clicked()
    {
        if (!manager.isEscPopup)
        {
            isClick = true;
            for (int i = 0; i < willShowUIs.Length; ++i)
            {
                willShowUIs[i].SetActive(true);
            }
        }

    }
}
