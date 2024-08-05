using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] playlist;
    private AudioSource audioSource;
    private int currentSong = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = playlist[++currentSong % playlist.Length];
            audioSource.Play();
        }
    }
}
