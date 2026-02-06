using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] LevelData data;
    [SerializeField] ResultsScreen results;
    private int numEnemies;
    private int numKills;
    private float topSpeed;
    Ranking.Rank rank = Ranking.Rank.SRank;

    private void Awake()
    {
        Time.timeScale = 1f;
        numEnemies = 0;
        if (instance == null)
        {
            instance = this;
        }
    }
    public void EndLevel()
    {
        TimeSpan timerData = TimerManager.instance.GetTime();
        TimerManager.instance.StopTimer();
        double playerTime = timerData.TotalSeconds;
        rank = Ranking.GenerateRank(data.requirements, playerTime);
        results.DisplayResults(timerData, numKills, numEnemies, topSpeed, rank);
        Debug.Log(rank);
    }
    private void NextLevel()
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
}
