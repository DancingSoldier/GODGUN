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
    
   
}
