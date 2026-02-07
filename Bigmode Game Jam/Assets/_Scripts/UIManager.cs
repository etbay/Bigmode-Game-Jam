using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] ResultsScreen results;
    [SerializeField] UISlickometer slickometer;
    [SerializeField] GameObject crosshair;
    [SerializeField] TimerManager timer;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        slickometer.gameObject.SetActive(true);
        crosshair.SetActive(true);
        timer.gameObject.SetActive(true);
    }
    public void EndScript(TimeSpan time, int kills, int totalEnemies, float speed, Ranking.Rank plRank)
    {
        results.DisplayResults(time, kills, totalEnemies, speed, plRank);
        slickometer.gameObject.SetActive(false);
        crosshair.SetActive(false);
        timer.gameObject.SetActive(false);
    }
}
