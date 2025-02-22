using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyUI : MonoBehaviour
{
    public int money = 0;
    public int moneyLevel = 1;

    public Text moneyUI;
    public GameObject needMoreMoney;

    private bool isFading = false;

    void Start()
    {
        money = 0;
        needMoreMoney.gameObject.SetActive(false);
        InvokeRepeating("IncreaseMoney", 0f, 0.05f);
    }

    void IncreaseMoney()
    {
        money += moneyLevel;
    }

    void Update()
    {
        moneyUI.text = money + " ₽";
    }

    public void AddMoney(int money)
    {
        this.money += money;
    }

    public int ReturnMoney()
    {
        return money;
    }

    public void NeedMoreMoney()
    {
        Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.omg);
        needMoreMoney.SetActive(true);
        //if (!isFading)
        //{
        //    StartCoroutine(FadeInOut());
        //}
    }

    //IEnumerator FadeInOut()
    //{
    //    needMoreMoney.gameObject.SetActive(true);
    //    isFading = true;

    //    float durationIn = 0.1f;
    //    float durationOut = 0.2f;

    //    Color color = needMoreMoney.color;

    //    for (float t = 0; t <= durationIn; t += Time.deltaTime)
    //    {
    //        color.a = Mathf.Lerp(0f, 1f, t / durationIn);
    //        needMoreMoney.color = color;
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(0.1f);

    //    for (float t = 0; t <= durationOut; t += Time.deltaTime)
    //    {
    //        color.a = Mathf.Lerp(1f, 0f, t / durationOut);
    //        needMoreMoney.color = color;
    //        yield return null;
    //    }

    //    isFading = false;
    //    needMoreMoney.gameObject.SetActive(false);
    //}
}
