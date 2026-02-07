using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static bool gameRunning = true;
    public static bool gameEnded = false;
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
        gameRunning = true;
        Time.timeScale = 1f;
        numEnemies = 0;
    }
    private void Update()
    {
        #if UNITY_EDITOR
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            EndLevel();
        }
        #endif
    }
    public void EndLevel()
    {
        TimeSpan timerData = TimerManager.instance.GetTime();
        TimerManager.instance.StopTimer();
        double playerTime = timerData.TotalSeconds;
        rank = Ranking.GenerateRank(data.requirements, playerTime);
        UIManager.instance.EndScript(timerData, numKills, numEnemies, topSpeed, rank);
        gameRunning = false;
        gameEnded = true;
        PauseGame();
    }
    public void NextLevel()
    {
        AudioManager.instance.StopFilterMusic();
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
        AudioManager.instance.StopFilterMusic();
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        float scaleBeforePause = Time.timeScale;
        PlayerCharacter.instance.PauseSounds();
        Time.timeScale = 0f;
        gameRunning = false;
        AudioManager.instance.FilterMusic();
        if (!gameEnded)
        {
            UIManager.instance.PauseScript();
        }
    }
    public void ResumeGame()
    {
        PlayerCharacter.instance.ResumeSounds();
        Time.timeScale = scaleBeforePause;
        gameRunning = true;
        AudioManager.instance.StopFilterMusic();
        UIManager.instance.ClosePauseScript();
    }
}
