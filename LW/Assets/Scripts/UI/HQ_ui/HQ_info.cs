using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HQ_info : MonoBehaviour
{
    public TextMeshProUGUI lvlText;
    [SerializeField] private float hqLvl;
    void Start()
    {
        hqLvl = 1;
    }

    void Update()
    {
        lvlText.text = "BASE LEVEL "+ hqLvl;
    }
}
