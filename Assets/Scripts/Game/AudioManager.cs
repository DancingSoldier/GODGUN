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
    public float volume;

    public List<string> scenesWithMusic;
    void MusicState(Scene scene)
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
            yield return null; // Odottaa seuraavaa framea
        }

        audioSource.volume = 0f;
        audioSource.Stop(); // Pysäytä soitto, kun äänenvoimakkuus on nolla
    }
    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // Päivitä musiikkitila, kun kohtaus vaihtuu
        MusicState(newScene);
    }



    void OnEnable()
    {
        // Rekisteröi tapahtuma kohtauksen vaihtoon
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDisable()
    {
        // Poista tapahtuma rekisteristä
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.volume = volume;

        // Aseta ensimmäisen kohtauksen musiikki oikein
        StartCoroutine(StartMusicDelay());
        Debug.Log(SceneManager.GetActiveScene().name);
    }

    private IEnumerator StartMusicDelay()
    {
        
        yield return new WaitForSeconds(1);
        MusicState(SceneManager.GetActiveScene());
    }


}