using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private LevelData levelData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelData.unlocked) {
            SetAllActive();
            levelName.text = levelData.levelName;
            if (levelData.completed)
            {
                rank.text = levelData.playerRank.ToString();
                time.text = string.Format("{0:00}:{1:00}.{2:000}",
                    levelData.playerTime.Minutes,
                    levelData.playerTime.Seconds,
                    levelData.playerTime.Milliseconds);
            }
            else
            {
                rank.text = "--";
                time.text = "--";
            }
        }
        else
        {
            SetAllInactive();
        }
        button.onClick.AddListener(PlayLevel);
    }
    void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
    private void PlayLevel()
    {
        AudioManager.instance.PlayPersistentSoundClip(AudioManager.instance.Play, 1f, false, false);
        SceneManager.LoadScene(levelData.sceneName);
    }
    private void SetAllInactive()
    {
        button.gameObject.SetActive(false);
        rank.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
    }
    private void SetAllActive()
    {
        button.gameObject.SetActive(true);
        rank.gameObject.SetActive(true);
        time.gameObject.SetActive(true);
    }
}
