using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Awake()
    {
        slider.onValueChanged.AddListener(SetVolume);

    }
    private void SetVolume(float value)
    {
        AudioManager.instance.mixer.SetFloat("MasterVolume", value);
    }
}
