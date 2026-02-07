using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public void UpdateMusicVol(float newVal)
    {
        AudioManager.instance.mixer.SetFloat("MusicVolume", newVal);
    }
    public void UpdateMasterVol(float newVal)
    {
        AudioManager.instance.mixer.SetFloat("MasterVolume", newVal);
    }
    public void UpdateSFXVol(float newVal)
    {
        AudioManager.instance.mixer.SetFloat("SFXVolume", newVal);
    }
    public void ResumeGame()
    {
        LevelManager.instance?.ResumeGame();
    }
    public void GoToMenu()
    {
        //LevelManager.instance?.();
    }
}
