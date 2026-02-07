using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSaveData", menuName = "Scriptable Objects/LevelSaveData")]
public class LevelSaveData : ScriptableObject
{
    public string levelName;
    [NonSerialized] public TimeSpan playerTime;
    public Ranking.Rank playerRank;
    public bool completed;
    public bool unlocked;
}

public static class LevelDataSaveUtility
{
    private static string Key(string levelName, string field)
    {
        return $"Level_{levelName}_{field}";
    }

    static bool CompareData(LevelSaveData oldData, LevelSaveData newData)
    {
        // level unlocked
        if (!oldData.unlocked && newData.unlocked)
        {
            return true;
        }

        // new completed run
        if (!oldData.completed && newData.completed)
        {
            return true;
        }

        // if we are comparing two valid times
        if (oldData.completed && newData.completed &&
            oldData.playerTime > TimeSpan.Zero &&
            newData.playerTime > TimeSpan.Zero &&
            newData.playerTime < oldData.playerTime)
        {

            // the new time is shorter then the last
            return true;
        }

        return false;
    }

    public static void SmartSave(LevelSaveData newData)
    {
        // Create a temporary copy to load old data into
        LevelSaveData oldData = ScriptableObject.CreateInstance<LevelSaveData>();

        // Copy static identifiers so keys match
        oldData.levelName = newData.levelName;

        // Load previously saved data
        Load(oldData);

        // Compare old vs new
        if (CompareData(oldData, newData))
        {
            Save(newData);
        }

        // Cleanup temp object
        ScriptableObject.Destroy(oldData);
    }

    // ---------- SAVE ----------
    public static void Save(LevelSaveData level)
    {
        // TimeSpan -> ticks (long split into 2 ints)
        long ticks = level.playerTime.Ticks;
        PlayerPrefs.SetInt(Key(level.levelName, "Time_Low"), (int)(ticks & 0xFFFFFFFF));
        PlayerPrefs.SetInt(Key(level.levelName, "Time_High"), (int)(ticks >> 32));

        PlayerPrefs.SetInt(Key(level.levelName, "Rank"), (int)level.playerRank);
        PlayerPrefs.SetInt(Key(level.levelName, "Completed"), level.completed ? 1 : 0);
        PlayerPrefs.SetInt(Key(level.levelName, "Unlocked"), level.unlocked ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Saved");
        Debug.Log($"Completion: {level.completed}");
        Debug.Log($"Ticks: {level.playerTime.Ticks}");
    }

    // ---------- LOAD ----------
    public static void Load(LevelSaveData level)
    {
        // Time
        int low = PlayerPrefs.GetInt(Key(level.levelName, "Time_Low"), 0);
        int high = PlayerPrefs.GetInt(Key(level.levelName, "Time_High"), 0);
        long ticks = ((long)high << 32) | (uint)low;
        level.playerTime = new TimeSpan(ticks);

        // Rank
        level.playerRank = (Ranking.Rank)
            PlayerPrefs.GetInt(Key(level.levelName, "Rank"), (int)Ranking.Rank.D);

        level.completed =
            PlayerPrefs.GetInt(Key(level.levelName, "Completed"), 0) == 1;

        level.unlocked =
            PlayerPrefs.GetInt(Key(level.levelName, "Unlocked"), 0) == 1;

        if(level.levelName.Equals("Tutorial"))
        {
            Debug.Log("Tutorial is unlocked");
            level.unlocked = true;
        }

        Debug.Log("Loaded");
        Debug.Log($"Completion: {level.completed}");
        Debug.Log($"Ticks: {level.playerTime.Ticks}");
    }

    // ---------- RESET (optional) ----------
    public static void Clear(LevelSaveData level)
    {
        PlayerPrefs.DeleteKey(Key(level.levelName, "Time_Low"));
        PlayerPrefs.DeleteKey(Key(level.levelName, "Time_High"));
        PlayerPrefs.DeleteKey(Key(level.levelName, "Rank"));
        PlayerPrefs.DeleteKey(Key(level.levelName, "Completed"));
        PlayerPrefs.DeleteKey(Key(level.levelName, "Unlocked"));
    }
}
