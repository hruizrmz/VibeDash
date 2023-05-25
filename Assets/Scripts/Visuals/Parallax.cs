using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private bool isGameRunning;
    private float scrollSpeed = 50;
    [SerializeField] private float spriteDepth = 1;
    [SerializeField] private float spawnX = 1;

    private void OnEnable()
    {
        GameManager.StopGame += StopBG;
    }

    private void OnDisable()
    {
        GameManager.StopGame -= StopBG;
    }

    private void Start()
    {
        isGameRunning = true;
    }

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

    private void StopBG()
    {
        isGameRunning = false;
    }
}
