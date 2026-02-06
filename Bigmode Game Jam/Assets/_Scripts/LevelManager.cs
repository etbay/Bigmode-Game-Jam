using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] LevelData data;
    private float numEnemies;
    private float numKills;
    Ranking.Rank rank = Ranking.Rank.SRank;

    private void Awake()
    {
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
        Debug.Log(rank);
    }
    private void DisplayResults()
    {
        
    }
    public void NextLevel()
    {
        //LoadScene(data.nextLevel);
        
    }
    public void RegisterEnemy()
    {
        numEnemies += 1;
    }
    public void RegisterKill()
    {
        numKills += 1;
    }
}
