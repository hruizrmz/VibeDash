using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action StartGame;
    public static event Action StopGame;

    private bool isGameRunning;

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

        if (isGameRunning)
        {
            if (player == null || !SongManager.Instance.isSongPlaying)
            {
                GameManager.StopGame?.Invoke();
                isGameRunning = false;
            }
        }
    }
}
