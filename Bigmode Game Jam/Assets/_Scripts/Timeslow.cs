using System.Runtime.CompilerServices;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class Timeslow : MonoBehaviour
{
    [SerializeField] private AudioClip timeSlow;
    [SerializeField] private AudioClip timeResume;
    [SerializeField] private float slowFactor = 0.2f; // 1 is full speed, 0.2 is 1/5 speed

    private AudioSource audioSource = null; 
    //[SerializeField] private AudioClip slowedTimeAmbience;

    public static Timeslow instance;
    public static bool IsSlowed = false;
    private PlayerInputActions _inputActions;

    void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        IsSlowed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputActions.Player.Ability.WasPressedThisFrame() && !IsSlowed && Player.SlickValue > 1.0f)
        {
            if (audioSource != null)
            {
                audioSource.volume = 0f; // stops overlaying time related sfx
            }
            IsSlowed = true;
            ActivateSlowMode();
            SlickometerData.CurrentSlickDrainRate = SlickometerData.TimeslowSlickDrainRate;
        }
        else if (((Player.SlickValue <= 1.0f) && IsSlowed) || (Player.SlickValue > 1.0f && _inputActions.Player.Ability.WasPressedThisFrame() && IsSlowed))
        {
            if (audioSource != null)
            {
                audioSource.volume = 0f;
            }
            IsSlowed = false;
            DeactivateSlowMode();
            SlickometerData.CurrentSlickDrainRate = SlickometerData.BaseSlickDrainRate;
        }
    }

    private void ActivateSlowMode()
    {
        Time.timeScale = slowFactor;
        audioSource = AudioManager.instance?.PlayOmnicientSoundClip(timeSlow, 1f, false, false);
        AudioManager.instance.TimeAudioStretch(0.6f);
    }

    private void DeactivateSlowMode()
    {
        Time.timeScale = 1f;
        audioSource = AudioManager.instance?.PlayOmnicientSoundClip(timeResume, 1f, false, false);
        AudioManager.instance.TimeAudioStretch(1f);
    }
}
