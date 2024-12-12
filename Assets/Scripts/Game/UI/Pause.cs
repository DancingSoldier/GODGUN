using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject settingsMenu;

    public void Continue()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0].gameObject;
        player.transform.GetComponent<InputManager>().TogglePause();
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
    }
    private void Start()
    {
        pauseScreen = transform.GetChild(7).gameObject;
        pauseScreen.SetActive(false);
    }
}
