using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
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
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://") )
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
        GetDataFromMidi();
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + midiFileLocation))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiFileLocation);
    }

    private void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);
    }

    public static double GetAudioSourceTime() // how many seconds the song has been playing for
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
}
