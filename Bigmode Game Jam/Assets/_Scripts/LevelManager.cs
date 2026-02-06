using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static bool GameRunning = true;
    [SerializeField] LevelData data;
    private int numEnemies;
    private int numKills;
    private float topSpeed;
    Ranking.Rank rank = Ranking.Rank.SRank;
    float scaleBeforePause = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GameRunning = true;
        Time.timeScale = 1f;
        numEnemies = 0;
    }
    public void EndLevel()
    {
        TimeSpan timerData = TimerManager.instance.GetTime();
        TimerManager.instance.StopTimer();
        double playerTime = timerData.TotalSeconds;
        rank = Ranking.GenerateRank(data.requirements, playerTime);
        UIManager.instance.EndScript(timerData, numKills, numEnemies, topSpeed, rank);
        GameRunning = false;
        PauseGame();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(data.nextLevel);
    }
    public void RegisterEnemy()
    {
        numEnemies += 1;
    }
    public void RegisterKill()
    {
        numKills += 1;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void TrackSpeed(float speed)
    {
        if (speed > topSpeed)
        {
            topSpeed = speed;
        }
    }
    public void PauseGame()
    {
        float scaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
        GameRunning = false;
    }
    public void ResumeGame()
    {
        Time.timeScale = scaleBeforePause;
        GameRunning = true;
    }
}
