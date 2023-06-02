using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action StartGame;
    public static event Action StopGame;
    private bool gameStarted = false;
    private bool gameEnded = false;

    public static ScoreManager Instance;

    public AudioSource hitSFX;

    private GameObject player;
    public UIManager uiObject;

    public int totalNotes;
    public int notesSpawned;
    public int currentNote;

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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentScore = currentCombo = maxCombo = 0;
        Instance = this;
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true;
                uiObject.ShowInGameScreen();
                ScoreManager.StartGame?.Invoke();
            }
        }
        else
        {
            if (player == null && !gameEnded)
            {
                float totalHit = accuracyCounter[0] + accuracyCounter[1];
                float percentHit = (totalHit / totalNotes) * 100f;

                string rank;
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
                    accuracyCounter[0],accuracyCounter[1], accuracyCounter[2], accuracyCounter[3],
                    maxCombo, currentScore.ToString("D6"), rank);
                ScoreManager.StopGame?.Invoke();

                gameEnded = true;
            }
        }
    }

    public void NoteHit(int accuracy)
    {
        Instance.hitSFX.Play();

        if (accuracy != 2) // combo only counts with perfect and great
        {
            currentCombo++;
        } 
        else
        {
            currentCombo = 0;
        }

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
        maxCombo = currentCombo;
        currentCombo = 0;

        currentScore += accuracyPoints[3];
        accuracyCounter[3]++;
        
        Instantiate(accuracyEffects[3], hitEffectPosition, Quaternion.identity);
        uiObject.UpdateComboVisual(currentCombo);
    }
}