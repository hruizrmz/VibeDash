using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action StartGame;
    public static event Action StopGame;

    public bool isGameRunning;

    public GameObject player;
    public UIManager uiObject;

    // Start is called before the first frame update
    void Start()
    {
        uiObject.ShowInGameScreen();
        GameManager.StartGame?.Invoke();
        isGameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
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
