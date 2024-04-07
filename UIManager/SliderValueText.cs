using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueText : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI textComp;

    void Awake()
    {
        slider = transform.parent.GetComponentInChildren<Slider>(); //get the slider component from the parent
        textComp = GetComponent<TextMeshProUGUI>(); //get the text component from this object, the one who has this script
    }

    void Start()
    {
        UpdateText(slider.value); //update the text with the initial value of the slider
        slider.onValueChanged.AddListener(UpdateText); //add a listener to the slider to update the text when the value changes
    }

    void UpdateText(float value)
    {
        //si el slider es del numero de peces, redondear el valor
        if (slider.name == "Num_fishes_slider" || slider.name == "Distance_slider" || slider.name == "Nutritional_value_slider" || slider.name == "Nutritional_value_toxic_slider")
            textComp.text = Mathf.Round(value).ToString();
        else
        textComp.text = value.ToString("F2"); //update the text with the value of the slider
    }
}
