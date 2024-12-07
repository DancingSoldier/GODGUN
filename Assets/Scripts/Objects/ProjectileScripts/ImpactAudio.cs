using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudio : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clip;
    public float volume = 1f;
    public float pitch = 1f;
    void CreateRandomness()
    {
        

        float minPitch = pitch - 0.4f;
        float maxPitch = pitch + 0.4f;
        float minVol = volume - 0.2f;
        float maxVol = volume + 0.2f;
        pitch = Random.Range(minPitch, maxPitch);
        volume = Random.Range(minVol, maxVol);
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip, volume);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        
        CreateRandomness();
        
        


        

    }


}
