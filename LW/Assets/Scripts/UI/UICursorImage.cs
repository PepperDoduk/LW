using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR;

public class UICursorImage : MonoBehaviour
{
    public GraphicRaycaster raycaster;  
    public EventSystem eventSystem;    
    public Image cursorImage;       
    public bool isImageUI;

    public Dictionary<string, Sprite> uiImageMap = new Dictionary<string, Sprite>();

    public Sprite defaultSprite;   
    public Sprite IAerialBomb;     
    public Sprite ICarpetBombing; 
    public Sprite ItacticalNuclearBomb;
    public Sprite IAK_train;
    public Sprite IRPG_train;
    public Sprite IT90_prod;
    public Sprite IT14_prod;    
    public Sprite IMSTA_prod;
    public Sprite IMi28_prod;
    public Sprite ISU57_prod;
    public Sprite ITITAN_prod;
    public Sprite UI_no;
    public Sprite UI_yes;

    public float maxSize = 100f;
    public int Pos;

    void Start()
    {
        isImageUI = true;
        cursorImage.gameObject.SetActive(false); 
        cursorImage.raycastTarget = false;   

        uiImageMap["Aerial bomb"] = IAerialBomb;
        uiImageMap["Carpet bombing"] = ICarpetBombing;
        uiImageMap["tactical nuclear bomb"] = ItacticalNuclearBomb;
        uiImageMap["AK_train"] = IAK_train;
        uiImageMap["RPG_train"] = IRPG_train;
        uiImageMap["T90_prod"] = IT90_prod;
        uiImageMap["T14_prod"] = IT14_prod;
        uiImageMap["MSTA_prod"] = IMSTA_prod;
        uiImageMap["Mi28_prod"] = IMi28_prod;
        uiImageMap["SU57_prod"] = ISU57_prod;
        uiImageMap["Titan_prod"] = ITITAN_prod;
        uiImageMap["UIyes"] = UI_yes;
        uiImageMap["UIno"] = UI_no;
    }

    void Update()
    {
        //Debug.Log(Input.mousePosition);
        if (IsPointerOverUI(out GameObject uiObject))
        {
            string uiName = uiObject.name;

            if (uiName == "UIyes")
            {
                cursorImage.sprite = uiImageMap["UIyes"];
                cursorImage.gameObject.SetActive(true);
                cursorImage.transform.position = GetCursorPosition();
                AdjustImageSize();
            }
            else if (uiImageMap.ContainsKey(uiName)&& isImageUI)  
            {
                cursorImage.sprite = uiImageMap[uiName];
                cursorImage.gameObject.SetActive(true);
                cursorImage.transform.position = GetCursorPosition();
                AdjustImageSize();
            }
            else
            {
                cursorImage.gameObject.SetActive(false);
            }
        }
        else
        {
            cursorImage.gameObject.SetActive(false); 
        }
    }


    private Vector3 GetCursorPosition()
    {
        if (Input.mousePosition.x > 960)  
        {
            return Input.mousePosition + new Vector3(-Pos, 0, 0);
        }
        else
        {
            return Input.mousePosition + new Vector3(Pos, 0, 0);
        }
    }

    private bool IsPointerOverUI(out GameObject uiObject)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        if (results.Count > 0)
        {
            uiObject = results[0].gameObject;
            return true;
        }

        uiObject = null;
        return false;
    }

    private void AdjustImageSize()
    {
        if (cursorImage.sprite == null) return;

        cursorImage.SetNativeSize();

        float width = cursorImage.rectTransform.sizeDelta.x;
        float height = cursorImage.rectTransform.sizeDelta.y;

        if (height > maxSize)
        {
            float scaleFactor = maxSize / height;
            cursorImage.rectTransform.sizeDelta = new Vector2(width * scaleFactor, height * scaleFactor);
        }
    }
}
