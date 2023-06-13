using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private static event Action ResetPlayerHit;
    private Animator anim;
    private bool playerHit = false;

    private void OnEnable()
    {
        NoteObject.SwipeRightNote += EnablePlayerHit;
        ResetPlayerHit += DisablePlayerHit;
    }

    private void OnDisable()
    {
        NoteObject.SwipeRightNote -= EnablePlayerHit;
        ResetPlayerHit -= DisablePlayerHit;
    }
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && playerHit)
        {
            ResetPlayerHit?.Invoke();
            anim.SetTrigger("breakObstacle");
        }
    }

    private void EnablePlayerHit()
    {
        playerHit = true;
    }

    private void DisablePlayerHit()
    {
        playerHit = false;
    }

    private void DestroyObstacleObject()
    {
        Destroy(gameObject);
    }
}
