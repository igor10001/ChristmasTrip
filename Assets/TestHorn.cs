using System.Collections;
using UnityEngine;

public class TestHorn : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeOutDuration = 0.2f; // Smoothly fade out when releasing
    private bool isHornActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isHornActive)
            {
                isHornActive = true;
                audioSource.pitch = Random.Range(0.95f, 1.05f); // Small variation for realism
                audioSource.loop = false; // No looping
                audioSource.Play();
            }
        }
        else if (Input.GetKey(KeyCode.H))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Restart immediately if it stops
            }
        }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            isHornActive = false;
            StartCoroutine(FadeOut(audioSource, fadeOutDuration)); // Smoothly fade out
        }
    }

    IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset volume for next use
    }
}