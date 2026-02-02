using UnityEngine;

public class WeaponSFXBank : MonoBehaviour
{
    [SerializeField] private AudioClip gunshotSound;
    [SerializeField] private AudioClip[] tracerSounds;
    public AudioClip GunshotSound() { return gunshotSound; }
    public AudioClip[] TracerSounds() { return tracerSounds; }
}
