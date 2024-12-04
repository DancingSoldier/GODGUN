using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<GameObject> earnedPickups = new List<GameObject>();
    public List<GameObject> chosenPickups = new List<GameObject>();
    PlayerScriptableObject playerScriptableObject;
    private void Awake()
    {
        //luodaan singleton
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChoosePickup()
    {

    }

}
