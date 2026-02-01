using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private GameObject sfxObject;
    [SerializeField] private Transform player;
    [SerializeField] private float timeStretching = 0.6f;
    


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    // Plays at the player's position
    public AudioSource PlayOmnicientSoundClip(AudioClip audioClip, float vol, bool slowable)
    {
        GameObject obj = Instantiate(sfxObject, player.position, Quaternion.identity);
        AudioSource source = obj.GetComponent<AudioSource>();
        source.clip = audioClip;
        source.volume = vol;
        if (slowable && Timeslow.IsSlowed) // Does the sound pitch down and slow during slowed time
        {
            source.pitch = timeStretching;
        }        
        source.Play();
        float clipLength = audioClip.length;
        StartCoroutine(KillAudioSource(obj, clipLength));
        return source;
    }

    // Plays at a passed transform's position, typically an enemy or something similar
    public AudioSource PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float vol, bool slowable)
    {        
        GameObject obj = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);
        AudioSource source = obj.GetComponent<AudioSource>();
        source.clip = audioClip;
        source.volume = vol;
        if (slowable && Timeslow.IsSlowed) // Does the sound pitch down and slow during slowed time
        {
            source.pitch = timeStretching;
        }        
        source.Play();
        float clipLength = audioClip.length;
        StartCoroutine(KillAudioSource(obj, clipLength));
        return source;
    }

    private IEnumerator KillAudioSource(GameObject target, float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        Destroy(target); // Controls waiting until a sfx has finished to delete the gameobject audiosource
    }
}
