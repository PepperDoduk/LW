using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class supportGuide : MonoBehaviour
{
    bool isFollowingMouse = true;
    Vector3 mousePos;
    public EAGLE500KGBOMB eagle500kgbomb;
    public GameObject support;



    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isFollowingMouse)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;
        }

        if (Input.GetMouseButtonDown(0))
        {

            isFollowingMouse = false;
            Instantiate(support, mousePos, Quaternion.identity);
            eagle500kgbomb.EAGLEStart();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            Color color = spriteRenderer.color;

            color.a = 0;

            spriteRenderer.color = color;
            StartCoroutine(A());
            gameObject.SetActive(false);
        }
    }

    public void StartGuide()
    {
        gameObject.SetActive(true);
        isFollowingMouse = true;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
    }

    IEnumerator A()
    {
        yield return new WaitForSeconds(4.3f);
        
    }
}
