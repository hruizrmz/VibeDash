using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action StartGame;
    public static event Action StopGame;
    private bool gameStarted = false;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true;
                GameManager.StartGame?.Invoke();
            }
        } else
        {
            if (player == null) GameManager.StopGame?.Invoke();
        }
    }
}
