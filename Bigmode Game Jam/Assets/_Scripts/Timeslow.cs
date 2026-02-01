using System.Runtime.CompilerServices;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class Timeslow : MonoBehaviour
{
    [SerializeField] private AudioClip timeSlow;
    [SerializeField] private AudioClip timeResume;

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
        if (_inputActions.Player.Ability.WasPressedThisFrame() && !IsSlowed)
        {
            if (audioSource != null)
            {
                audioSource.volume = 0f;
            }
            IsSlowed = true;
            ActivateSlowMode();
            audioSource = AudioManager.instance?.PlayOmnicientSoundClip(timeSlow, 1f);
        }
        else if (_inputActions.Player.Ability.WasPressedThisFrame() && IsSlowed)
        {
            if (audioSource != null)
            {
                audioSource.volume = 0f;
            }
            IsSlowed = false;
            DeactivateSlowMode();
            audioSource = AudioManager.instance?.PlayOmnicientSoundClip(timeResume, 1f);
        }
    }

    private void ActivateSlowMode()
    {
        Time.timeScale = 0.2f;
    }

    private void DeactivateSlowMode()
    {
        Time.timeScale = 1f;
    }
}
