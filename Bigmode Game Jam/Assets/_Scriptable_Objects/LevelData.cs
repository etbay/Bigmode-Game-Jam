using System;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public Ranking.RankRequirements requirements;
    public float numEnemies;
    public float topSpeed;
    public Scene nextLevel;
}
