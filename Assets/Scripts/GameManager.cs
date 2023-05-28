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
    public int currentScore;
    public int currentCombo;
    public int scorePerNote = 30;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentCombo = 1;
        currentScore = 0;
    }

    private void Start()
    {
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

    public void NoteHit()
    {
        Debug.Log("note hit");
        currentScore += scorePerNote;
        currentCombo++;
        scoreText.text = "Score: " + currentScore;
        comboText.text = "Combo x" + currentCombo;
    }
    
    public void NoteMissed()
    {
        Debug.Log("note miss");
    }
}
