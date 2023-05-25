using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action StopGame;
    public GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null) GameManager.StopGame?.Invoke();
    }

}
