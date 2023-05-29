using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static event Action StartGame;
    public static event Action StopGame;
    private bool gameStarted = false;

    public AudioSource levelMusic;
    private GameObject player;

    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    private int currentScore;
    private int finePoints = 50;
    private int greatPoints = 150;
    private int perfectPoints = 300;
    private int fineNotes;
    private int missNotes;
    private int greatNotes;
    private int perfectNotes;
    private int currentCombo;
    private int comboPoints;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentScore = currentCombo = 0;
        fineNotes = missNotes = greatNotes = perfectNotes = 0;
        instance = this;
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true;
                levelMusic.Play();
                GameManager.StartGame?.Invoke();
            }
        } else
        {
            if (player == null)
            {
                levelMusic.volume = .20f;
                GameManager.StopGame?.Invoke();
            }
        }
    }

    public void NoteHit(int accuracy) // 0 perfect, 1 great, 2 fine
    {
        // Adding points based on combo
        currentCombo++;
        if ((currentCombo >= 2) && (currentCombo < 16))
        {
            comboPoints = 50;
        }
        else if ((currentCombo >= 16) && (currentCombo < 41))
        {
            comboPoints = 100;
        }
        else if ((currentCombo >= 41) && (currentCombo < 71))
        {
            comboPoints = 150;
        }
        else if ((currentCombo >= 71) && (currentCombo < 100))
        {
            comboPoints = 200;
        }
        else if (currentCombo >= 101)
        {
            comboPoints = 250;
        }
        else
        {
            comboPoints = 0;
        }
        currentScore += comboPoints;

        // Adding points based on hit accuracy
        switch (accuracy)
        {
            case 0: // perfect
                {
                    currentScore += perfectPoints;
                    perfectNotes++;
                    Debug.Log("perfect hit");
                    break;
                }
            case 1: // great
                {
                    currentScore += greatPoints;
                    greatNotes++;
                    Debug.Log("great hit");
                    break;
                }
            case 2: // fine
                {
                    currentScore += finePoints;
                    fineNotes++;
                    Debug.Log("fine hit");
                    break;
                }
            default:
                {
                    break;
                }
        }
        
        scoreText.text = "Score: " + currentScore;
        comboText.text = "Combo: " + currentCombo;
    }
    
    public void NoteMissed()
    {
        missNotes++;
        currentCombo = 0;
    }
}
