using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public GameObject notePrefab;
    public GameObject holdBodyPrefab;
    public GameObject holdEndPrefab;
    private readonly List<NoteObject> notes = new();
    public List<double> timeStamps = new();

    public int laneID = 0; // 0 tap, 1 hold, 2 swipe up, 3 swipe down, 4 swipe right
    private int spawnIndex = 0; // keeps track of which note to spawn
    private int indexCurrentlySpawning = 0;

    private bool holdingStopped, isSpawning;

    #region Events
    private void OnEnable()
    {
        ScoreManager.StopGame += StopSong;
        InputManager.HoldEnded += HoldEnded;
    }
    private void OnDisable()
    {
        ScoreManager.StopGame -= StopSong;
        InputManager.HoldEnded -= HoldEnded;
    }
    private void StopSong()
    {
        Destroy(gameObject);
    }
    private void HoldEnded()
    {
        if (laneID == 1 && (spawnIndex == indexCurrentlySpawning)) holdingStopped = true;
    }
    #endregion

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan> (note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    void Update()
    {
        if (spawnIndex < timeStamps.Count) // until we go through all the notes in the lane
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            { 
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<NoteObject>());
                note.GetComponent<NoteObject>().assignedTime = (float)timeStamps[spawnIndex];

                if (laneID == 1)
                {
                    var noteLength = SongManager.Instance.holdNotesList[spawnIndex];
                    StartCoroutine(SpawnHoldBody(spawnIndex, noteLength, SongManager.GetAudioSourceTime(), timeStamps[spawnIndex]));
                }

                spawnIndex++;

                ScoreManager.Instance.notesSpawned++;
            }
        }
    }

    IEnumerator SpawnHoldBody(int index, double noteLength, double startTime, double centerTime)
    {
        isSpawning = true;
        double spawnLength = 0;
        double assignedTime, lastAssignedTime = 0;
        while (isSpawning)
        {
            if (spawnLength < SongManager.Instance.noteTime) holdingStopped = false;
            if (!holdingStopped)
            {
                if (spawnLength < noteLength)
                {
                    var note = Instantiate(holdBodyPrefab, transform);
                    spawnLength = (SongManager.GetAudioSourceTime() - startTime);
                    assignedTime = (float)(centerTime + spawnLength);
                    if (assignedTime != lastAssignedTime)
                    {
                        note.GetComponent<NoteObject>().assignedTime = (float)assignedTime;
                        lastAssignedTime = assignedTime;
                    }
                }
                else
                {
                    var note = Instantiate(holdEndPrefab, transform);
                    note.GetComponent<NoteObject>().assignedTime = (float)(centerTime + noteLength);
                    isSpawning = false;
                }
            }
            else
            {
                isSpawning = false;
                holdingStopped = false;
            }
            yield return null;
        }
        indexCurrentlySpawning++;
    }
}