using UnityEngine;

public class Parallax : MonoBehaviour
{
    private bool isGameRunning = false;
    private float scrollSpeed = 50;
    [SerializeField] private float spriteDepth = 1;
    [SerializeField] private float spawnX = 1;

    #region Events
    private void OnEnable()
    {
        ScoreManager.StartGame += StartBG;
        ScoreManager.StopGame += StopBG;
    }
    private void OnDisable()
    {
        ScoreManager.StartGame -= StartBG;
        ScoreManager.StopGame -= StopBG;
    }
    private void StartBG()
    {
        isGameRunning = true;
    }
    private void StopBG()
    {
        isGameRunning = false;
    }
    #endregion

    private void Update()
    {
        if (isGameRunning)
        {
            float velocity = scrollSpeed / spriteDepth;
            Vector2 pos = transform.position;
            pos.x -= velocity * Time.deltaTime;
            if (pos.x <= -spawnX) pos.x = spawnX;
            transform.position = pos;
        }
    }
}
