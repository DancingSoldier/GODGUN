using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixer effects;
    public AudioMixer weapons;
    public void SetMainVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);

        Debug.Log(volume);
    }
    public void SetEffectVolume(float volume)
    {
        effects.SetFloat("Effects", volume);

        Debug.Log(volume);
    }
    public void SetWeaponsVolume(float volume)
    {
        weapons.SetFloat("Weapons", volume);

        Debug.Log(volume);
    }
    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
    public void SetFog()
    {
        ArenaManager.manager.FogToggle();

    }
}
