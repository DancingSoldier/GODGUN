using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro.Examples;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public GameObject deletePopup;
    public GameObject animationObject;
    Animator animator;

    public void Combat()
    {
        SceneManager.LoadScene("Combat");
    }


    public void ConfirmDeletion()
    {
        GameManager.manager.ResetData();
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

    public void ClearData()
    {
        deletePopup.SetActive(true);
    }

    public void CancelDeletion()
    {
        deletePopup.SetActive(false);
    }

    private IEnumerator ClearText(int seconds, TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(seconds);
        text.text = "";
    }
    void Start()
    {
        infoText.text = GameManager.manager.LoadData();
        StartCoroutine(GameManager.manager.ClearText(5,infoText));
        GameManager.manager.chosenPickups.Clear();
        animator = animationObject.GetComponent<Animator>();
        animator.Play("mainMenuBombAnimation");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        deletePopup.SetActive(false);
        Time.timeScale = 1;
    }


}
