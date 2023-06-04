using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Animator anim;

    private void OnEnable()
    {
        NoteObject.SwipeRightNote += BreakObstacle;
        NoteObject.ObstacleMissed += TripPlayer;
    }

    private void OnDisable()
    {
        NoteObject.SwipeRightNote -= BreakObstacle;
        NoteObject.ObstacleMissed -= TripPlayer;
    }
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void BreakObstacle()
    {
        anim.SetTrigger("breakObstacle");
    }

    private void TripPlayer()
    {
        anim.SetTrigger("tripPlayer");
    }

    private void DestroyObstacleObject()
    {
        Destroy(gameObject);
    }
}
