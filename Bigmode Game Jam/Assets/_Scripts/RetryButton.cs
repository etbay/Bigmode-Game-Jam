using UnityEngine;

public class RetryButton : MonoBehaviour
{
    public void RestartLevel()
    {
        LevelManager.instance?.RestartLevel();
    }
    public void NextLevel()
    {
        LevelManager.instance?.NextLevel();
    }
}
