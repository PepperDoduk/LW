using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tasks;
    public GameObject[] checks;
    public GameObject Tutorial;

    public bool isRadioCheck;
    public bool isAirstrikeCheck;
    public bool isProduceCheck;
    public bool isNukeCheck;
    public bool isScrollCheck;
    public bool isClickCheck;

    private int checknum;

    public int taskNum;

    void Start()
    {
        Tutorial.SetActive(true);
        checknum = 0;
        taskNum = 1;
        isClickCheck = false ;
        isScrollCheck = false;
        isRadioCheck = false;
        isAirstrikeCheck = false;
        isProduceCheck = false;
        isNukeCheck = false;
        tasks[6].SetActive(false);

        for (int i=0; i< checks.Length; ++i)
        {
            checks[i].SetActive(false);
            tasks[i+3].SetActive(false);
        }
    }

    void Update()
    {
        if (taskNum == 1)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                checks[0].SetActive(true);
                isClickCheck = true;
            }

            if (scroll != 0)
            {
                checks[1].SetActive(true);
                isScrollCheck = true;
            }
            if (isRadioCheck)
            {
                checks[2].SetActive(true);
            }

            if (isRadioCheck && isClickCheck && isScrollCheck) 
            {
                StartCoroutine(Tutorial2());
            }
        }
        if(taskNum == 2)
        {
            if (isAirstrikeCheck)
            {
                checks[0].SetActive(true);
            }

            if (isProduceCheck)
            {
                checks[1].SetActive(true);
            }

            if (isNukeCheck)
            {
                checks[2].SetActive(true);
            }

            if(isAirstrikeCheck && isProduceCheck && isNukeCheck)
            {
                StartCoroutine(Tutorial3());
            }
        }


    }

    public void Radio()
    {
        isRadioCheck = true;
    }
    public void Airstrike()
    {
        isAirstrikeCheck = true;
    }
    public void Produce() 
    { 
        isProduceCheck = true;
    }

    public void Nuke()
    {
        isNukeCheck = true;
    }

    public IEnumerator Tutorial2 ()
    {
        taskNum = 2;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; ++i)
        {
            tasks[i].SetActive(false);
            checks[i].SetActive(false);
            tasks[i+3].SetActive(true);
        }
    }

    public IEnumerator Tutorial3()
    {
        taskNum = 3;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; ++i)
        {
            tasks[i+3].SetActive(false);
            checks[i].SetActive(false);
        }
        tasks[6].SetActive(true);
        yield return new WaitForSeconds(3);
        tasks[6].SetActive(false);
        Tutorial.SetActive(false);
    }

    }
