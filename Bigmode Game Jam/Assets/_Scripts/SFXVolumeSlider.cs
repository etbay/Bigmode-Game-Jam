using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Awake()
    {
        slider.onValueChanged.AddListener(SetVolume);

    }
    private void SetVolume(float value)
    {
        float volumeInDb = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        AudioManager.instance.mixer.SetFloat("SFXVolume", volumeInDb);
    }
}
