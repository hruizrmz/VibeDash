using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanesArray;
    public Note[] notesArray;
    public List<double> holdNotesList;
    public float songDelayInSeconds;

    public string midiFileLocation;
    public float noteTime; // the player react time from spawn to a perfect hit
    public float noteSpawnX; // where the note spawns
    public float noteTapX; // where the note can be tapped
    public float noteDespawnX // where the note disappears if untapped
    {
        get
        {
            return noteTapX - (noteSpawnX - noteTapX);
        }
    }

    #region Events
    private void OnEnable()
    {
        ScoreManager.StartGame += StartSong;
        ScoreManager.StopGame += StopSong;
    }
    private void OnDisable()
    {
        ScoreManager.StartGame -= StartSong;
        ScoreManager.StartGame -= StopSong;
    }
    private void StartSong()
    {
        Invoke(nameof(StartPlayback), songDelayInSeconds);
    }
    private void StartPlayback()
    {
        audioSource.Play();
    }
    private void StopSong()
    {
        audioSource.volume = 0.2f;
    }
    #endregion

    public static MidiFile midiFile;

    void Start()
    {
        Instance = this;
        ReadFromFile();
        GetDataFromMidi();
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiFileLocation);
    }

    private void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        notesArray = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(notesArray, 0);

        foreach (var lane in lanesArray) lane.SetTimeStamps(notesArray);

        double basicNoteLength = ((TimeSpan)SongManager.Instance.notesArray[0].LengthAs<MetricTimeSpan>(SongManager.midiFile.GetTempoMap())).TotalSeconds;
        foreach (var note in notesArray)
        {
            double noteLength = ((TimeSpan)note.LengthAs<MetricTimeSpan>(SongManager.midiFile.GetTempoMap())).TotalSeconds;
            if (noteLength > basicNoteLength) holdNotesList.Add(noteLength);
        }

        FindObjectOfType<ScoreManager>().totalNotes += notesArray.Length;
    }

    public static double GetAudioSourceTime() // how many seconds the song has been playing for
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
}
