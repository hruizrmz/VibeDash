using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action StartGame, PauseGame, UnPauseGame, StopGame;

    private bool isGameRunning;
    public bool isGamePaused;
    private bool pauseScreenCalled;

    private float currentTime;
    [SerializeField] private float countdownTime;

    public GameObject player;
    public UIManager uiObject;

    void Start()
    {
        currentTime = countdownTime + 1;
        GameManager.StartGame?.Invoke();
    }

    void Update()
    {
        if (!isGameRunning && currentTime>0)
        {
            currentTime -= 1 * Time.deltaTime;
            if (uiObject.SongCountdown(currentTime))
            {
                uiObject.ShowInGameScreen();
                isGameRunning = true;
            }
        }

        if (isGameRunning && !isGamePaused)
        {
            if (!SongManager.Instance.isSongPlaying)
            {
                GameManager.StopGame?.Invoke();
                isGameRunning = false;
            }
        }

        if (isGamePaused && !pauseScreenCalled) PauseGameFunction();
    }

    public void ResumeGameFunction()
    {
        GameManager.UnPauseGame?.Invoke();
        pauseScreenCalled = false;
        isGamePaused = false;
        Time.timeScale = 1.0f;
        uiObject.ShowInGameScreen();
        pauseScreenCalled = false;
    }
    
    public void PauseGameFunction()
    {
        GameManager.PauseGame?.Invoke();
        isGamePaused = true;
        Time.timeScale = 0.0f;
        uiObject.ShowPauseScreen();
        pauseScreenCalled = true;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) PauseGameFunction();
    }
}
