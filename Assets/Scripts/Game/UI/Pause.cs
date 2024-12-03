using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    public GameObject pauseScreen;
    

    private void Start()
    {
        pauseScreen = transform.GetChild(3).gameObject;
    }
}
