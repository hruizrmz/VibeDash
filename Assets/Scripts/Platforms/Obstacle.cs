using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Animator anim;

    private void OnEnable()
    {
        NoteObject.SwipeRightNote += BreakObstacle;
    }

    private void OnDisable()
    {
        NoteObject.SwipeRightNote -= BreakObstacle;
    }
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void BreakObstacle()
    {
        anim.SetTrigger("breakObstacle");
    }

    private void DestroyObstacleObject()
    {
        Destroy(gameObject);
    }
}
