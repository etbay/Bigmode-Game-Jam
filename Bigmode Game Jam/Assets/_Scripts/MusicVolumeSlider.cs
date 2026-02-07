using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Awake()
    {
        slider.onValueChanged.AddListener(SetVolume);

    }
    private void SetVolume(float value)
    {
        AudioManager.instance.mixer.SetFloat("MusicVolume", value);
    }
}
