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
        yipee.text = "*µý*";
        yield return Wait(0.6f);
        yipee.text = "*´Ü*";
        yield return Wait(0.6f);
        yipee.text = "*µý*";
        yield return Wait(1.2f);
        yipee.text = "*(ÇÇ¾Æ³ë)µû´Ù´Ù´Ü*";
        yield return Wait(0.4f);
        yipee.text = "*µû*";
        yield return Wait(0.13f);
        yipee.text = "*´Ü*";
        yield return Wait(0.13f);
        yipee.text = "*µû*";
        yield return Wait(0.13f);
        yipee.text = "*¤§ ¤¿*";
        yield return Wait(0.13f);
        yipee.text = "*¤¤*";
        yield return Wait(0.13f);
        yipee.text = "*µû*";
        yield return Wait(0.13f);
        yipee.text = "*¤¿*";
        yield return Wait(0.13f);
        yipee.text = "*µû*";
        yield return Wait(0.13f);
        yipee.text = "*µý*";
        yield return Wait(0.13f);
        yipee.text = "*´Ù´Ü*";
        yield return Wait(1f);
        yipee.text = "*µû´Ù´Ù´Ù´Ü*";
        yield return Wait(1f);
        yipee.text = "*µý*";
        yield return Wait(0.35f);
        yipee.text = "*µû*";
        yield return Wait(0.2f);
        yipee.text = "*´Ü*";
        yield return Wait(2.3f);
        yipee.text = "*µû´Ü*";
        yield return Wait(0.4f);
        yipee.text = "*µý*";
        yield return Wait(0.5f);
        yipee.text = "";
        yield return Wait(0.1f);
        yipee.text = "*µý*";
        yield return Wait(0.4f);
        yipee.text = "*µû´Ü*";
        yield return Wait(0.6f);
        yipee.text = "*»£*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*»£*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*»£*";
        yield return Wait(0.6f);
        yipee.text = "*(ÇÇ¾Æ³ë)µû¶ó´Ü µý µý*";
        yield return Wait(3f);
        yipee.text = "*µû´Ü µý µý*";
        yield return Wait(3.1f);
        yipee.text = "*»£*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*»£*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*»£*";
        yield return Wait(0.1f);
        yipee.text = "";
        yield return Wait(0.3f);
        yipee.text = "*»£*";
        yield return Wait(1.2f);
        yipee.text = "*(°³@Â¼´Â ±âÅ¸)*";
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
