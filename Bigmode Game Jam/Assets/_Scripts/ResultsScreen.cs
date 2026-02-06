using System;
using TMPro;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerTime;
    [SerializeField] TextMeshProUGUI playerKills;
    [SerializeField] TextMeshProUGUI topSpeed;
    [SerializeField] TextMeshProUGUI rank;

    public void DisplayResults(TimeSpan time, int kills, int totalEnemies, float speed, Ranking.Rank plRank)
    {
        gameObject.SetActive(true);
        playerTime.text = string.Format("Time: {0:00}:{1:00}.{2:000}", time.Minutes, time.Seconds, time.Milliseconds);
        playerKills.text = "Kills: " + kills + "/" + totalEnemies;
        topSpeed.text = "Top Speed: " + speed.ToString("F2") + "m/s";
        rank.text = "Final Rank: " + plRank.ToString();
    }
}
