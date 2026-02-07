using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Awake()
    {
        slider.onValueChanged.AddListener(SetVolume);
    }
    private void Start()
    {
        float volumeDB = 0;
        float vol = 0;
        AudioManager.instance.mixer.GetFloat("MusicVolume", out volumeDB);
        if (volumeDB <= -80f) 
        {
            vol = volumeDB;
        }
        else
        {
            float linear = Mathf.Pow(10.0f, volumeDB / 20.0f);
            vol = Mathf.Clamp(linear, 0f, 1f);
        }
        slider.value = vol;
    }
    private void SetVolume(float value)
    {
        float volumeInDb = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        AudioManager.instance.mixer.SetFloat("MusicVolume", volumeInDb);
    }
}
