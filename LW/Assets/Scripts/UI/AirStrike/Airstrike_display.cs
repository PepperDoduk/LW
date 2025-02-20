using UnityEngine;
using System.Collections;

public class Airstrike_display : MonoBehaviour
{
    public Long_range_radio radio;
    public GameObject tu160;
    public GameObject targetPos;

    public int airstrikeNum = 0;
    public CanvasGroup uiCanvasGroup;

    private bool isLaunched;

    public ScreenShake shake;

    public GameObject[] bombs;
    void OnEnable()
    {
        uiCanvasGroup.alpha = 1f;
        isLaunched = false;
    }

    public static class Bombs
    {
        public const int Aerial = 0;
        public const int Carpet = 1;
        public const int TacNuke = 2;
    }

    public void SetBombing(int bombNumber)
    {
        airstrikeNum = bombNumber;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            tu160.SetActive(true);
            tu160.transform.position = new Vector3(-400, 70, 0);
            targetPos.transform.position = new Vector3(Camera.main.transform.position.x, 5, 0);
            radio.isClick = false;
            uiCanvasGroup.alpha = 0f;

            StartCoroutine(Bombing());
            isLaunched = true;
        }
    }

    public IEnumerator Bombing(){
        yield return new WaitForSeconds(7);

        if (airstrikeNum == Bombs.Aerial)
        {
            shake.SetShake(0.7f, 0.7f, 1f);
            shake.TriggerShake();

            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.striker_explosion);
            GameObject airstrike = ObjectPoolManager.Instance.GetObjectFromPool(bombs[airstrikeNum], Quaternion.identity, new Vector3(1, 1, 1));
            airstrike.transform.position = targetPos.transform.position;

        }

        else if (airstrikeNum == Bombs.Carpet)
        {
            for (int i = 0; i < 8; ++i)
            {
                Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.striker_explosion);

                GameObject airstrike = ObjectPoolManager.Instance.GetObjectFromPool(bombs[0], Quaternion.identity, new Vector3(1, 1, 1));
                airstrike.transform.position = targetPos.transform.position + new Vector3 (i * Random.Range(5,10),0,0);

                shake.SetShake(0.7f, 0.7f, 1f);
                shake.TriggerShake();

                yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            }
        }

        else if (airstrikeNum == Bombs.TacNuke)
        {
            shake.SetShake(2f, 2f, 3f);
            shake.TriggerShake();

            GameObject airstrike = ObjectPoolManager.Instance.GetObjectFromPool(bombs[1], Quaternion.identity, new Vector3(1, 1, 1));
            airstrike.transform.position = targetPos.transform.position;
        }
            gameObject.SetActive(false);
    }
}
