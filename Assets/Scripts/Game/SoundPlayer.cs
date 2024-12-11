using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip[] clips;

    IEnumerator PlaySpawnSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            float pitch = Random.Range(0.8f, 1.1f);
            audioSource.pitch = pitch;
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSource.PlayOneShot(clip, 0.8f);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
        StartCoroutine(PlaySpawnSound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
