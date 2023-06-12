using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public AudioSource hitSFX, missSFX;

    public PersonaController persona;
    public UIManager uiObject;

    public GameObject particleEffect;
    public Vector3 particleSpawnPosition;

    public int totalNotes, notesSpawned, currentNote;

    private int currentScore;
    private int currentCombo;
    private int maxCombo;
    private int comboPoints;
    public Vector3 hitEffectPosition;
    [SerializeField] private GameObject[] accuracyEffects = new GameObject[4];
    private readonly Dictionary<int, int> accuracyPoints = new()
    {
        { 0, 100}, // perfect
        { 1, 50}, // great
        { 2, 20}, // fine
        { 3, 0} // miss
    };
    private readonly Dictionary<int, int> accuracyCounter = new()
    {
        { 0, 0}, // perfect
        { 1, 0}, // great
        { 2, 0}, // fine
        { 3, 0} // miss
    };
    private string rank;

    private void OnEnable()
    {
        GameManager.StopGame += CalculateResults;
    }

    private void OnDisable()
    {
        GameManager.StopGame -= CalculateResults;
    }

    private void Start()
    {
        currentScore = currentCombo = maxCombo = 0;
        Instance = this;
    }

    private void CalculateResults()
    {
        float totalHit = accuracyCounter[0] + accuracyCounter[1];
        float percentHit = (totalHit / totalNotes) * 100f;

        if (accuracyCounter[0] == totalNotes)
        {
            rank = "SS";
        }
        else
        {
            rank = (percentHit >= 90f) ? "S" :
              (percentHit >= 80f) ? "A" :
              (percentHit >= 65f) ? "B" :
              (percentHit >= 50f) ? "C" :
              (percentHit >= 30f) ? "D" :
              "E";
        }

        if (maxCombo < currentCombo)
        {
            maxCombo = currentCombo;
        }

        uiObject.ShowResultsScreen(
            accuracyCounter[0], accuracyCounter[1], accuracyCounter[2], accuracyCounter[3],
            maxCombo, currentScore.ToString("D6"), rank);
    }

    public void NoteHit(int accuracy)
    {
        currentCombo++;
        persona.UpdatePersonaCombo(currentCombo);
        Instantiate(particleEffect, particleSpawnPosition, Quaternion.identity);

        comboPoints = (currentCombo >= 2 && currentCombo < 16) ? 15 :
               (currentCombo >= 16 && currentCombo < 41) ? 30 :
               (currentCombo >= 41 && currentCombo < 71) ? 60 :
               (currentCombo >= 71 && currentCombo < 100) ? 100 :
               (currentCombo >= 101) ? 120 : 0;
        currentScore += comboPoints;

        currentScore += accuracyPoints[accuracy];
        accuracyCounter[accuracy]++;
        Instantiate(accuracyEffects[accuracy], hitEffectPosition, Quaternion.identity);

        uiObject.UpdateScoreVisual(currentScore.ToString("D6"));
        uiObject.UpdateComboVisual(currentCombo);
    }

    public void NoteMissed()
    {
        if (maxCombo < currentCombo)
        {
            maxCombo = currentCombo;
        }

        currentCombo = 0;
        persona.PlayPersonaMiss();
        Instance.missSFX.Play();

        currentScore += accuracyPoints[3];
        accuracyCounter[3]++;
        
        Instantiate(accuracyEffects[3], hitEffectPosition, Quaternion.identity);
        uiObject.UpdateComboVisual(currentCombo);
    }

    public void PlayHitSound()
    {
        Instance.hitSFX.Play();
    }
}