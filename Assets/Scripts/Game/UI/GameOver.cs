using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public TextMeshProUGUI timeText;
    public GameObject saveIcon;
    public Animator animator;
    private void Start()
    {
        gameOverScreen = transform.GetChild(4).gameObject;
        saveIcon = transform.GetChild(10).gameObject;
        animator = saveIcon.GetComponentInChildren<Animator>();


    }


    public void GameOverSetup(float elapsedTime)
    {
        gameOverScreen.SetActive(true);
        
        timeText.text = "Time: " + elapsedTime.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Combat");
        
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
