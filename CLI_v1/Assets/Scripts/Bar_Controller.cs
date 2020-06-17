using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar_Controller : MonoBehaviour{

    public Slider slider;

    public void SetMaxValue(float my_max_value){

        this.slider.maxValue = my_max_value;
    }

    public void SetCurrentValue(float current_value){

        this.slider.value = current_value;
    }
}
