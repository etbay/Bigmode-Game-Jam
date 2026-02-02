using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

using Debug = UnityEngine.Debug;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // certified single ton
    [SerializeField] public AudioMixer mixer;
    [SerializeField] private GameObject sfxObject;
    [SerializeField] private Transform player;
    [SerializeField] private float timeForAudioStretch = 0.5f;

    private AudioMixerGroup sfxGroup;
    private AudioMixerGroup timeSlowedGroup;
    private AudioMixerGroup environmentGroup;
    private AudioMixerGroup musicGroup;
    private Queue<GameObject> sfxPlayerPool = new Queue<GameObject>();

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

    private void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            // Add many sfxplayers to the pool to prevent runtime instantiation
            var sfxPlayer = Instantiate(sfxObject, sfxObject.transform.position, Quaternion.identity);
            sfxPlayer.gameObject.SetActive(false);
            sfxPlayerPool.Enqueue(sfxPlayer);
        }
    }

    // Plays at the player's position
    public AudioSource PlayOmnicientSoundClip(AudioClip audioClip, float vol, bool slowable, bool pitchRandomly)
    {
        GameObject obj = RequestSfxPlayerFromPool();
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
    public AudioSource PlaySoundClip(AudioClip audioClip, Vector3 spawnpos, float vol, bool slowable, bool pitchRandomly)
    {        
        GameObject obj = RequestSfxPlayerFromPool();
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

    public AudioSource PlaySoundClipFromList(AudioClip[] audioClips, Vector3 spawnpos, float vol, bool slowable, bool pitchRandomly)
    {        
        GameObject obj = RequestSfxPlayerFromPool();
        AudioSource source = obj.GetComponent<AudioSource>();
        int num = Random.Range(0, audioClips.Length);
        source.clip = audioClips[num];
        source.volume = vol;
        if (slowable) // Does the sound pitch down and slow during slowed time
        {
            source.outputAudioMixerGroup = timeSlowedGroup;
        }
        else source.outputAudioMixerGroup = sfxGroup;
        if (pitchRandomly)
        {
            source.pitch += Random.Range(-0.15f, 0.15f);
        }
        source.Play();
        float clipLength = source.clip.length;
        StartCoroutine(KillAudioSource(obj, clipLength));
        return source;
    }

    private IEnumerator KillAudioSource(GameObject target, float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        target.SetActive(false);
        sfxPlayerPool.Enqueue(target); // Controls waiting until a sfx has finished to delete the gameobject audiosource
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
    public AudioSource GetLoopableAudioSource(AudioClip audioClip, float vol, bool slowable, bool pitchRandomly)
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
            source.outputAudioMixerGroup = environmentGroup;
        }
        if (pitchRandomly)
        {
            source.pitch += Random.Range(-0.15f, 0.15f);
        }
        source.loop = true;
        float clipLength = audioClip.length;
        return source;
    }

    public IEnumerator FadeToVolume(AudioSource source, float initial, float final, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            source.volume = Mathf.Clamp01(Mathf.Lerp(initial, final, elapsedTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        source.volume = final;
    }
    private GameObject RequestSfxPlayerFromPool()
    {
        GameObject obj;
        if (sfxPlayerPool.Count > 0)
        {
            obj = sfxPlayerPool.Dequeue();
            obj.transform.position = player.transform.position;
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(sfxObject, player.position, Quaternion.identity);
        }
        return obj;
    }
}
