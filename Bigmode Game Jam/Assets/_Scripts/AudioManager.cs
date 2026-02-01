using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private GameObject sfxObject;

    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    // Plays at the player's position
    public AudioSource PlayOmnicientSoundClip(AudioClip audioClip, float vol)
    {
        GameObject audioSource = Instantiate(sfxObject, player.transform.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = audioClip;
        audioSource.GetComponent<AudioSource>().volume = vol;
        audioSource.GetComponent<AudioSource>().Play();
        float clipLength = audioClip.length;
        StartCoroutine(KillAudioSource(audioSource, clipLength));
        return audioSource.GetComponent<AudioSource>();
    }

    // Plays at a passed transform's position, typically an enemy or something similar
    public AudioSource PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float vol)
    {        
        GameObject audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().clip = audioClip;
        audioSource.GetComponent<AudioSource>().volume = vol;
        audioSource.GetComponent<AudioSource>().Play();
        float clipLength = audioClip.length;
        StartCoroutine(KillAudioSource(audioSource, clipLength));
        return audioSource.GetComponent<AudioSource>();
    }

    private IEnumerator KillAudioSource(GameObject target, float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        Destroy(target); // Controls waiting until a sfx has finished to delete the gameobject audiosource
    }
}
