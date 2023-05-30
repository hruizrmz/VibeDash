using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    public float lifetime = 1f;
    private bool songIsRunning;

    #region Events
    private void OnEnable()
    {
        GameManager.StartGame += StartSong;
        GameManager.StopGame += StopSong;
    }
    private void OnDisable()
    {
        GameManager.StartGame -= StartSong;
        GameManager.StopGame -= StopSong;
    }
    private void StartSong()
    {
        songIsRunning = true;
    }
    private void StopSong()
    {
        Destroy(gameObject);
    }
    #endregion

    private void Update()
    {
        if (songIsRunning) Destroy(gameObject, lifetime);
    }
}