using UnityEngine;

public class RetryButton : MonoBehaviour
{
    public void RestartLevel()
    {
        LevelManager.instance?.RestartLevel();
    }
    public void NextLevel()
    {
        Debug.Log("NextLevelClicked");
        LevelManager.instance?.NextLevel();
    }
}
