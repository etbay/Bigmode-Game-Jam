using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource sfxObject;

    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    public AudioSource PlayOmnicientSoundClip(AudioClip audioclip, float vol)
    {
        AudioSource audioSource = Instantiate(sfxObject, player.transform.position, Quaternion.identity);
        audioSource.clip = audioclip;
        audioSource.volume = vol;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource, clipLength);
        return audioSource;
    }
    public AudioSource PlaySoundClip(AudioClip audioclip, Transform spawnTransform, float vol)
    {        
        AudioSource audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioclip;
        audioSource.volume = vol;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource, clipLength);
        return audioSource;
    }
}
