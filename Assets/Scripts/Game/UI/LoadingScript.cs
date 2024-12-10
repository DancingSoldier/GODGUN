using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour
{
    DialogueScript dialogue;
    public GameObject gameObject;
    void Awake()
    {
        dialogue = gameObject.GetComponent<DialogueScript>();
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(.5f);
        dialogue.StartDialogue();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
