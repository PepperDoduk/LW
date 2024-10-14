using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class yippee : MonoBehaviour
{
    public Text yipee;
    void Start()
    {
        StartCoroutine(Yeppe());
        StartCoroutine(Yeppe2());
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Yeppe2()
    {
        yield return new WaitForSeconds(0.3f);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.TankHit);
    }

    IEnumerator Yeppe()
    {
        yipee.text = "세상에";
        yield return Wait(0.6f);
        yipee.text = "맙ㅂ소사";
        yield return Wait(0.6f);
        yipee.text = "이런ㅇ일이!!";
        yield return Wait(1.2f);
        yipee.text = "우리 엄마가";
        yield return Wait(0.4f);
        yipee.text = "콜";
        yield return Wait(0.13f);
        yipee.text = "콜라";
        yield return Wait(0.13f);
        yipee.text = "콜라를";
        yield return Wait(0.13f);
        yipee.text = "콜라를마";
        yield return Wait(0.13f);
        yipee.text = "콜라를마셔";
        yield return Wait(0.13f);
        yipee.text = "콜라를마셔도";
        yield return Wait(0.13f);
        yipee.text = "콜라를마셔도된";
        yield return Wait(0.13f);
        yipee.text = "콜라를마셔도된댔";
        yield return Wait(0.13f);
        yipee.text = "콜라를마셔도된댔어";
        yield return Wait(0.13f);
        yipee.text = "콜라를마셔도된댔어ㅅ요!!";
        yield return Wait(1);
        yipee.text = "이제 그냥";
        yield return Wait(1f);
        yipee.text = "콜라를 퍼먹ㄱ어도되요!!";
        yield return Wait(3f);
        yipee.text = "이거 개쌈@뽕해요!!";
        yield return Wait(3f);
        yipee.text = "이제 나는";
        yield return Wait(0.6f);
        yipee.text = "개쩌@는 포트나이트랑";
        yield return Wait(2.6f);
        yipee.text = "콜라를 스@근하게 즐기면 됨!!";
        yield return Wait(4.6f);
        yipee.text = "예!!";
        yield return Wait(0.4f);
        yipee.text = "삐!!";
        yield return Wait(0.4f);
        yipee.text = "";
    }

    IEnumerator Wait(float seconds)
    {
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
