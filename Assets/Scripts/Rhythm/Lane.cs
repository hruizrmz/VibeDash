using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public GameObject notePrefab;
    List<NoteObject> notes = new List<NoteObject>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0; // keeps track of which note to spawn

    #region Events
    private void OnEnable()
    {
        ScoreManager.StopGame += StopSong;
    }
    private void OnDisable()
    {
        ScoreManager.StopGame -= StopSong;
    }
    private void StopSong()
    {
        Destroy(gameObject);
    }
    #endregion

    public void Start()
    {
        gameObject.SetActive(false);
        // change way to count total notes!!!
        FindObjectOfType<ScoreManager>().totalNotes = FindObjectsOfType<NoteObject>().Length;
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
        if (spawnIndex < timeStamps.Count) // until we go through all the notes
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<NoteObject>());
                note.GetComponent<NoteObject>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }
    }
}
