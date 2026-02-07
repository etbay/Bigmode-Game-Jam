using System;
using Tripolygon.UModelerX.Editor.Modelings;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public string sceneName;
    public string levelName;
    public Ranking.RankRequirements requirements;
    public string nextLevel;
    // Player stat recording
    public TimeSpan playerTime;
    public Ranking.Rank playerRank;
    public bool completed;
    public bool unlocked;
}
