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
        yipee.text = "����";
        yield return Wait(0.6f);
        yipee.text = "�����һ�";
        yield return Wait(0.6f);
        yipee.text = "�̷�������!!";
        yield return Wait(1.2f);
        yipee.text = "�츮 ������";
        yield return Wait(0.4f);
        yipee.text = "��";
        yield return Wait(0.13f);
        yipee.text = "�ݶ�";
        yield return Wait(0.13f);
        yipee.text = "�ݶ�";
        yield return Wait(0.13f);
        yipee.text = "�ݶ󸦸�";
        yield return Wait(0.13f);
        yipee.text = "�ݶ󸦸���";
        yield return Wait(0.13f);
        yipee.text = "�ݶ󸦸��ŵ�";
        yield return Wait(0.13f);
        yipee.text = "�ݶ󸦸��ŵ���";
        yield return Wait(0.13f);
        yipee.text = "�ݶ󸦸��ŵ��ȴ�";
        yield return Wait(0.13f);
        yipee.text = "�ݶ󸦸��ŵ��ȴ��";
        yield return Wait(0.13f);
        yipee.text = "�ݶ󸦸��ŵ��ȴ���!!";
        yield return Wait(1);
        yipee.text = "���� �׳�";
        yield return Wait(1f);
        yipee.text = "�ݶ� �۸Ԥ���ǿ�!!";
        yield return Wait(3f);
        yipee.text = "�̰� ����@���ؿ�!!";
        yield return Wait(3f);
        yipee.text = "���� ����";
        yield return Wait(0.6f);
        yipee.text = "��¼@�� ��Ʈ����Ʈ��";
        yield return Wait(2.6f);
        yipee.text = "�ݶ� ��@���ϰ� ���� ��!!";
        yield return Wait(4.6f);
        yipee.text = "��!!";
        yield return Wait(0.4f);
        yipee.text = "��!!";
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
