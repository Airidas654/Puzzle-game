using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] OptionsSlider[] sliders;
    [SerializeField] OptionsCheckbox[] checkboxes;

    void Start()
    {
        sliders[0].SetValue(0.5f); // set value to sound volume
        sliders[1].SetValue(0.5f); // set value to music volume
        checkboxes[0].SetValue(true); // set value to post effects

    }

    // Update is called once per frame
    void Update()
    {
        // = sliders[0].value // set sound volume to value
        // = sliders[1].value // set music volume to value
        // = checkboxes[0].GetValue(); // set post effects volume to value
    }
}
