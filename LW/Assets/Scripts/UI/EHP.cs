using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EHP : MonoBehaviour
{
    public Slider HPSlider;
    public Generator hq;
    float maxHp = 15000f; 

    // Start is called before the first frame update
    void Start()
    {
        UpdateSlider(); 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlider(); 
    }

    void UpdateSlider()
    {
        float currentHp = hq.ReturnHp();
        float normalizedHp = currentHp / maxHp; 
        HPSlider.value = normalizedHp;
    }
}
