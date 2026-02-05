using UnityEngine;

public class WeaponSFXBank : MonoBehaviour
{
    [SerializeField] private AudioClip gunshotSound;
    [SerializeField] private AudioClip[] tracerSounds;
    [SerializeField] private AudioClip[] hitSounds;
    public AudioClip GunshotSound() { return gunshotSound; }
    public AudioClip[] TracerSounds() { return tracerSounds; }
    public AudioClip[] HitSounds() { return hitSounds; }
}
