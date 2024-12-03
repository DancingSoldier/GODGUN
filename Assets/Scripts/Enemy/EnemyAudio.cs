using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public EnemyScriptableObject scriptableObject;
    AudioSource audioSource;
    AudioClip spawnClip;
    AudioClip passiveClip;
    private IEnumerator PlayAudio()
    {
        if(scriptableObject.onSpawn)
        {
            audioSource.pitch = scriptableObject.Pitch();
            audioSource.clip = scriptableObject.soundClip;
            audioSource.PlayOneShot(scriptableObject.soundClip, scriptableObject.soundVolume);
        }
        else
        {
            audioSource.pitch = scriptableObject.Pitch();
            audioSource.clip = scriptableObject.soundClip;
            audioSource.PlayDelayed(Random.Range(0, 5));
        }

        while (scriptableObject.repeatAudio)
        {
            yield return new WaitForSeconds(Random.Range(5, 15));
            if (scriptableObject.soundClip != null)
            {

                if (scriptableObject.playAlways)
                {
                    audioSource.pitch = scriptableObject.Pitch();
                    audioSource.PlayOneShot(scriptableObject.soundClip, scriptableObject.soundVolume);
                }
                else
                {
                    bool play = scriptableObject.Chance();
                    if(play)
                    {
                        audioSource.pitch = scriptableObject.Pitch();
                        audioSource.PlayOneShot(scriptableObject.soundClip, scriptableObject.soundVolume);
                    }
                }


            }
        }
        yield break;
    }
    void Start()
    {
        
        audioSource = transform.GetComponent<AudioSource>();

        StartCoroutine(PlayAudio());
        
    }
}
