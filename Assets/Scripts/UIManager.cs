using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject inGameScreen;
    public GameObject noteBar;
    public GameObject noteController;

    public TextMeshProUGUI inGameScoreText;
    public TextMeshProUGUI inGameComboText;

    public GameObject resultScreen;
    public TextMeshProUGUI perfectText, greatText, fineText, missText, scoreText, rankText;

    private void Awake()
    {
        inGameScreen.SetActive(false);
        noteBar.SetActive(false);
        resultScreen.SetActive(false);
    }

    public void ShowInGameScreen()
    {
        inGameScreen.SetActive(true);
        noteBar.SetActive(true);
        noteController.SetActive(true);
    }

    public void UpdateScoreVisual(int score, int combo)
    {
        inGameScoreText.text = string.Format("Score: {0}", score);
        inGameComboText.text = string.Format("Combo: {0}", combo);
    }

    public void ShowResultScreen(int perfect, int great, int fine, int miss, int score, string rank)
    {
        inGameScreen.SetActive(false);
        noteBar.SetActive(false);
        noteController.SetActive(false);

        resultScreen.SetActive(true);
        perfectText.text = string.Format("{0}", perfect);
        greatText.text = string.Format("{0}", great);
        fineText.text = string.Format("{0}", fine);
        missText.text = string.Format("{0}", miss);
        scoreText.text = string.Format("{0}", score);
        rankText.text = rank;
    }
}
