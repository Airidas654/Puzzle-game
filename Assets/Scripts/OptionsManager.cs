using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] OptionsSlider[] sliders;
    [SerializeField] OptionsCheckbox[] checkboxes;

    void Start()
    {
        float volume;
        SoundManager.Instance.GetMusic(0).GetVolume(out volume);
        sliders[0].SetValue(volume); // set value to sound volume
        SoundManager.Instance.GetSound(0).GetVolume(out volume);
        sliders[1].SetValue(volume); // set value to music volume
        checkboxes[0].SetValue(GameManager.inst.GetOldMonitorEffects()); // set value to post effects
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // = sliders[0].value // set sound volume to value
        // = sliders[1].value // set music volume to value
        GameManager.inst.ChangeOldMonitorEffects(checkboxes[0].GetValue()); // set post effects volume to value
    }
}
