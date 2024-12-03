using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    public GameObject pauseScreen;
    

    public void Continue()
    {
        pauseScreen.SetActive(false);
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }


    private void Start()
    {
        pauseScreen = transform.GetChild(6).gameObject;
        pauseScreen.SetActive(false);
    }
}
