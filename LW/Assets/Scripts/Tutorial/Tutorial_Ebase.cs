using UnityEngine;
using System.Collections;

public class Tutorial_Ebase : MonoBehaviour
{
    public GameObject[] Units;
    public float x;
    public float y;
    public float z;
    public Vector3[] unitSize;

    private Vector3[] unitLocations;

    public double unitValue;
    public bool isTutorial;
    private bool isEnemyProduce;

    void Start()
    {
        isTutorial = true;
        Tutorial1();
    }

    private void Update()
    {
        if (!isTutorial)
        {
            Enemy();
        }
        
    }

    public IEnumerator Tutorial1()
    {
        for(int i=0; i<5; ++i)
        {
            Instantiate(Units[0], new Vector3(transform.position.x + x, transform.position.y + y, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(5f);
        StartCoroutine(Tutorial2());
    }

    public IEnumerator Tutorial2()
    {
        yield return new WaitForSeconds(0.3f);
        Instantiate(Units[1], new Vector3(transform.position.x + x, transform.position.y + y, 0), Quaternion.identity);
        yield return new WaitForSeconds(5f);
        StartCoroutine(Tutorial3());
    }

    public IEnumerator Tutorial3()
    {
        yield return new WaitForSeconds(0.3f);
        Instantiate(Units[2], new Vector3(transform.position.x + x, transform.position.y + y, 0), Quaternion.identity);
        yield return new WaitForSeconds(5f);
        TutorialEnd();
    }

    public void TutorialEnd()
    {
        isTutorial = false;
    }

    IEnumerator Enemy()
    {
        isEnemyProduce= true;
        int randomValue = Random.Range(0, 100);

        int unitIndex;

        if (randomValue < 87)
            unitIndex = 0;
        else if (randomValue < 98)
            unitIndex = 1;
        else
            unitIndex = 2;

        if (Units[unitIndex] == null)
        {
            yield break;
        }

        Instantiate(Units[unitIndex], new Vector3(transform.position.x + x, transform.position.y + y, 0), Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(1.0f,5.5f));
        isEnemyProduce = false;
    }
}
