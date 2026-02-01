using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

using Debug = UnityEngine.Debug;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] public AudioMixer mixer;
    private AudioMixerGroup sfxGroup;
    private AudioMixerGroup timeSlowedGroup;
    private AudioMixerGroup environmentGroup;
    private AudioMixerGroup musicGroup;

    [SerializeField] private GameObject sfxObject;
    [SerializeField] private Transform player;
    [SerializeField] private float timeForAudioStretch = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        sfxGroup = mixer.FindMatchingGroups("SFX")[0];
        timeSlowedGroup = mixer.FindMatchingGroups("Time")[0];
        environmentGroup = mixer.FindMatchingGroups("Environment")[0];
        musicGroup = mixer.FindMatchingGroups("Music")[0];
    }

    // Plays at the player's position
    public AudioSource PlayOmnicientSoundClip(AudioClip audioClip, float vol, bool slowable, bool pitchRandomly)
    {
        GameObject obj = Instantiate(sfxObject, player.position, Quaternion.identity);
        AudioSource source = obj.GetComponent<AudioSource>();
        source.clip = audioClip;
        source.volume = vol;
        if (slowable) // Does the sound pitch down and slow during slowed time
        {
            source.outputAudioMixerGroup = timeSlowedGroup;
        }
        else
        {
            source.outputAudioMixerGroup = sfxGroup;
        }
        if (pitchRandomly)
        {
            source.pitch += Random.Range(-0.15f, 0.15f);
        }
        source.Play();
        float clipLength = audioClip.length;
        StartCoroutine(KillAudioSource(obj, clipLength));
        return source;
    }

    // Plays at a passed transform's position, typically an enemy or something similar
    public AudioSource PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float vol, bool slowable, bool pitchRandomly)
    {        
        GameObject obj = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);
        AudioSource source = obj.GetComponent<AudioSource>();
        source.clip = audioClip;
        source.volume = vol;
        if (slowable) // Does the sound pitch down and slow during slowed time
        {
            source.outputAudioMixerGroup = timeSlowedGroup;
        }
        else
        {
            source.outputAudioMixerGroup = sfxGroup;
        }
        if (pitchRandomly)
        {
            source.pitch += Random.Range(-0.1f, 0.1f);
        }
        source.Play();
        float clipLength = audioClip.length;
        StartCoroutine(KillAudioSource(obj, clipLength));
        return source;
    }

    public AudioSource PlaySoundClipFromList(AudioClip[] audioClips, Transform spawnTransform, float vol, bool slowable, bool pitchRandomly)
    {        
        GameObject obj = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);
        AudioSource source = obj.GetComponent<AudioSource>();
        int num = Random.Range(0, audioClips.Length);
        source.clip = audioClips[num];
        source.volume = vol;
        if (slowable) // Does the sound pitch down and slow during slowed time
        {
            source.outputAudioMixerGroup = timeSlowedGroup;
        }
        else
        {
            source.outputAudioMixerGroup = sfxGroup;
        }
        if (pitchRandomly)
        {
            source.pitch += Random.Range(-0.1f, 0.1f);
        }
        source.Play();
        float clipLength = source.clip.length;
        StartCoroutine(KillAudioSource(obj, clipLength));
        return source;
    }

    private IEnumerator KillAudioSource(GameObject target, float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        Destroy(target); // Controls waiting until a sfx has finished to delete the gameobject audiosource
    }

    public void TimeAudioStretch(float finalValue)
    {
        float initialValue;
        timeSlowedGroup.audioMixer.GetFloat("TimeSlowedPitch", out initialValue);
        StartCoroutine(AudioPitchLerp(initialValue, finalValue, timeForAudioStretch));
    }
    
    private IEnumerator AudioPitchLerp(float initialValue, float finalValue, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            timeSlowedGroup.audioMixer.SetFloat("TimeSlowedPitch", Mathf.Lerp(initialValue, finalValue, elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        timeSlowedGroup.audioMixer.SetFloat("TimeSlowedPitch", finalValue);
    }
}
