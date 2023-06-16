using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private bool isGameRunning;

    #region Events
    private void OnEnable()
    {
        GameManager.StartGame += StartPlatforms;
        GameManager.StopGame += StopPlatforms;
    }
    private void OnDisable()
    {
        GameManager.StartGame -= StartPlatforms;
        GameManager.StopGame -= StopPlatforms;
    }
    private void StartPlatforms()
    {
        isGameRunning = true;
    }
    private void StopPlatforms()
    {
        isGameRunning = false;
    }
    #endregion

    void Update()
    {
        if (isGameRunning) transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0f, 0f);
    }
}
