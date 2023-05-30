using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject inGameScreen;
    public GameObject noteBar;
    public GameObject noteController;

    public TextMeshProUGUI inGameScoreText;
    public TextMeshProUGUI inGameComboText;

    public GameObject resultsScreen;
    public TextMeshProUGUI perfectText, greatText, fineText, missText, maxComboText, scoreText, rankText;

    private void Awake()
    {
        inGameScreen.SetActive(false);
        noteBar.SetActive(false);
        resultsScreen.SetActive(false);
    }

    public void ShowInGameScreen()
    {
        inGameScreen.SetActive(true);
        noteBar.SetActive(true);
        noteController.SetActive(true);
    }

    public void UpdateScoreVisual(string score)
    {
        inGameScoreText.text = score;
    }

    public void UpdateComboVisual(int combo)
    {
        inGameComboText.text = string.Format("x{0}", combo);
    }

    public void ShowResultsScreen(int perfect, int great, int fine, int miss, int maxCombo, string score, string rank)
    {
        inGameScreen.SetActive(false);
        noteBar.SetActive(false);
        noteController.SetActive(false);

        resultsScreen.SetActive(true);
        perfectText.text = string.Format("{0}", perfect);
        greatText.text = string.Format("{0}", great);
        fineText.text = string.Format("{0}", fine);
        missText.text = string.Format("{0}", miss);
        maxComboText.text = string.Format("{0}", maxCombo);
        scoreText.text = score;
        rankText.text = rank;
    }
}
