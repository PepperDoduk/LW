using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class yippee1 : MonoBehaviour
{
    public Text yipee;

    void Start()
    {
        StartCoroutine(Yeppe());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Yeppe()
    {
        yipee.text = "*��*";
        yield return Wait(0.6f);
        yipee.text = "*��*";
        yield return Wait(0.6f);
        yipee.text = "*��*";
        yield return Wait(1.2f);
        yipee.text = "*(�ǾƳ�)���ٴٴ�*";
        yield return Wait(0.4f);
        yipee.text = "*��*";
        yield return Wait(0.13f);
        yipee.text = "*��*";
        yield return Wait(0.13f);
        yipee.text = "*��*";
        yield return Wait(0.13f);
        yipee.text = "*�� ��*";
        yield return Wait(0.13f);
        yipee.text = "*��*";
        yield return Wait(0.13f);
        yipee.text = "*��*";
        yield return Wait(0.13f);
        yipee.text = "*��*";
        yield return Wait(0.13f);
        yipee.text = "*��*";
        yield return Wait(0.13f);
        yipee.text = "*��*";
        yield return Wait(0.13f);
        yipee.text = "*�ٴ�*";
        yield return Wait(1f);
        yipee.text = "*���ٴٴٴ�*";
        yield return Wait(1f);
        yipee.text = "*��*";
        yield return Wait(0.35f);
        yipee.text = "*��*";
        yield return Wait(0.2f);
        yipee.text = "*��*";
        yield return Wait(2.3f);
        yipee.text = "*����*";
        yield return Wait(0.4f);
        yipee.text = "*��*";
        yield return Wait(0.5f);
        yipee.text = "";
        yield return Wait(0.1f);
        yipee.text = "*��*";
        yield return Wait(0.4f);
        yipee.text = "*����*";
        yield return Wait(0.6f);
        yipee.text = "*��*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*��*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*��*";
        yield return Wait(0.6f);
        yipee.text = "*(�ǾƳ�)����� �� ��*";
        yield return Wait(3f);
        yipee.text = "*���� �� ��*";
        yield return Wait(3.1f);
        yipee.text = "*��*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*��*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*��*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*��*";
        yield return Wait(1.2f);
        yipee.text = "*(��@¼�� ��Ÿ)*";
        yield return Wait(1.2f);
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
