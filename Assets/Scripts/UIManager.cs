using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject inGameScreen, pauseScreen;
    public GameObject noteLanes;

    public TextMeshProUGUI countdownText;

    public TextMeshProUGUI inGameScoreText;
    public TextMeshProUGUI inGameComboText;

    public GameObject resultsScreen;
    public TextMeshProUGUI perfectText, greatText, fineText, missText, maxComboText, scoreText, rankText;

    private void Awake()
    {
        countdownText.gameObject.SetActive(true);
        inGameScreen.SetActive(false);
        resultsScreen.SetActive(false);
    }

    public void ShowInGameScreen()
    {
        pauseScreen.SetActive(false);

        inGameScreen.SetActive(true);
        noteLanes.GetComponent<SpriteRenderer>().enabled = true;
        for (int i = 0; i < noteLanes.transform.childCount; i++)
        {
            GameObject child = noteLanes.transform.GetChild(i).gameObject;
            child.SetActive(true);
        }
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
        noteLanes.SetActive(false);

        resultsScreen.SetActive(true);
        perfectText.text = string.Format("{0}", perfect);
        greatText.text = string.Format("{0}", great);
        fineText.text = string.Format("{0}", fine);
        missText.text = string.Format("{0}", miss);
        maxComboText.text = string.Format("{0}", maxCombo);
        scoreText.text = score;
        rankText.text = rank;
    }

    public void ShowPauseScreen()
    {
        inGameScreen.SetActive(false);
        noteLanes.GetComponent<SpriteRenderer>().enabled = false;
        for (int i = 0; i < noteLanes.transform.childCount; i++)
        {
            GameObject child = noteLanes.transform.GetChild(i).gameObject;
            child.SetActive(false);
        }

        pauseScreen.SetActive(true);
    }

    public bool SongCountdown(float second)
    {
        if (second >= 2)
        {
            countdownText.text = string.Format("READY...", (int)second);
            return false;
        }
        else if (second < 2 && second > 1.5)
        {
            countdownText.text = "START!";
            return false;
        }
        else if (second <= 1.5)
        {
            countdownText.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
