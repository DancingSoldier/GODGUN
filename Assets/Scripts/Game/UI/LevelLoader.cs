using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelLoader : MonoBehaviour
{
    public LevelLoader levelLoader;




    public Animator animator;
    public float transitionTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        levelLoader = this;

        
    }


    public IEnumerator TransitionIn(string sceneName)
    {
        animator.SetTrigger("Start");
        Debug.Log("Playing Transition Start");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
        Debug.Log("Playing Transition End");
        animator.SetTrigger("End");
    }



    public void LoadScene(string sceneName)
    {

        StartCoroutine(levelLoader.TransitionIn(sceneName));
    }

}
