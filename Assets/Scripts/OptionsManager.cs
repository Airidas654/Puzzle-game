using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private OptionsSlider[] sliders;
    [SerializeField] private OptionsCheckbox[] checkboxes;

    private void Start()
    {
        float volume;
        volume = SoundManager.Instance.GetGlobalSoundVolume();
        sliders[0].SetValue(volume); // set value to sound volume
        volume = SoundManager.Instance.GetGlobalMusicVolume();
        sliders[1].SetValue(volume); // set value to music volume
        checkboxes[0].SetValue(GameManager.inst.GetOldMonitorEffects()); // set value to post effects
    }

    // Update is called once per frame
    private void Update()
    {
        SoundManager.Instance.ChangeGlobalSoundVolume(sliders[0].value); // set sound volume to value
        SoundManager.Instance.ChangeGlobalMusicVolume(sliders[1].value); // set music volume to value
        GameManager.inst.ChangeOldMonitorEffects(checkboxes[0].GetValue()); // set post effects volume to value
    }
}