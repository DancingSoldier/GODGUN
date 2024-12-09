using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    Scene currentScene;
    public static AudioManager manager;
    public AudioSource audioSource;
    public AudioClip music;
    public AudioClip ambiance;
    public float volume;

    public List<string> scenesWithMusic;

    private void OnEnable()
    {
        // Rekisteröidy kohtauksen vaihtumisen tapahtumaan
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        // Poista tapahtuma rekisteristä
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.volume = volume;

        // Aseta ensimmäisen kohtauksen musiikki viiveellä
        StartCoroutine(StartMusicDelay(SceneManager.GetActiveScene()));
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // Aloita musiikkitilan päivitys viiveellä
        StartCoroutine(StartMusicDelay(newScene));
    }

    private IEnumerator StartMusicDelay(Scene scene)
    {
        // Viive ennen musiikin päivittämistä
        yield return new WaitForSeconds(2f);
        MusicState(scene);
    }

    private void MusicState(Scene scene)
    {
        if (scenesWithMusic.Contains(scene.name))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = volume;
                audioSource.clip = music;
                audioSource.loop = true;
                audioSource.Play();
                Debug.Log("Music started in scene: " + scene.name);
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                StartCoroutine(MusicFadeout(2));
                Debug.Log("Music stopped in scene: " + scene.name);
            }
        }
    }

    private IEnumerator MusicFadeout(float duration)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
