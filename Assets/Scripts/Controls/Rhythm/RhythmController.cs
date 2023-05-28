using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    public float beatTempo; // how fast notes scroll
    public bool songIsRunning;

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
        songIsRunning = false;
    }
    #endregion

    void Start()
    {
        beatTempo /= 60f; // the units the notes scroll per second
    }

    void Update()
    {
        if (songIsRunning)
        {
            transform.position -= new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        }
    }
}
