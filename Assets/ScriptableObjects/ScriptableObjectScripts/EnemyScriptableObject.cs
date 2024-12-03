using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Characters/Enemy", order = 2)]
public class EnemyScriptableObject : ScriptableObject
{
    public float moveSpeed;
    public float angularSpeed;
    public float acceleration;

    public float destinationInterval;
    public int health;

    public AudioClip soundClip;
    public float soundVolume = 1;
    public float soundPitch = 1;
    public bool pitchRandomization = false;
    public bool onSpawn;
    public bool repeatAudio = false;
    public bool playAlways = false;
    public int chance = 10;
    public float Pitch()
    {
        if (pitchRandomization)
        {
            float pitch = soundPitch;

            float minPitch = pitch - 0.3f; 
            float maxPitch = pitch + 0.3f; 


            pitch = Random.Range(minPitch, maxPitch);
            return pitch;
        }
        else
        {
            return soundPitch;
        }
    }
    public bool Chance()
    {
        if (Random.Range(0, chance) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   
}
