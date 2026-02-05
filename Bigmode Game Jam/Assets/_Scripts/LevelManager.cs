using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] LevelData data;
    Ranking.Rank rank = Ranking.Rank.SRank;

    private void Awake()
    {
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
    }

    public void NextLevel()
    {
        //LoadScene(data.nextLevel);
    }
}
