using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack", menuName = "Guns/Attack Audio Configuration", order = 5)]
public class AttackAudioScriptableObject : ScriptableObject
{
    public float volume = 1f;
    public float pitch = 1f;
    public AudioClip[] attackAudioClips;


    public void PlayAudio(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            float pitchLRange = pitch - .2f;
            float pitchHRange = pitch + .2f;

            audioSource.pitch = Random.Range(pitchLRange, pitchHRange);
            audioSource.PlayOneShot(attackAudioClips[Random.Range(0, attackAudioClips.Length)], volume);
        }
    }
}
