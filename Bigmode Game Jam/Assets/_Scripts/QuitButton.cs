using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitGame);
    }
    private void QuitGame()
    {
        Application.Quit();
    }
}
