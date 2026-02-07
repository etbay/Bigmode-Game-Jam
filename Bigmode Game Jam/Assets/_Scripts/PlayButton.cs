using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private LevelData level1;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayGame);
    }
    private void PlayGame()
    {
        SceneManager.LoadScene(level1.sceneName);
    }
}
