using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    
    public TextMeshProUGUI textComponent;
    public string[] dialogueLines;
    public float textSpeed = 1;
    AudioSource audioSource;
    public AudioClip clip;
    private int index;

    public void StartDialogue()
    {
        index = 0;
        gameObject.SetActive(true);
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if(index < dialogueLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator TypeLine()
    {
        foreach (char c in dialogueLines[index].ToCharArray())
        {
            textComponent.text += c;
            if (audioSource != null && clip != null)
            {
                float pitch = Random.Range(1.2f, 3);
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(clip);
            }
            yield return new WaitForSeconds(textSpeed);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
        audioSource = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == dialogueLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = dialogueLines[index];
            }
        }
    }
}
