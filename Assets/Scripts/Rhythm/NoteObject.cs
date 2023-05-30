using System;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private bool isThereTouch;
    private bool canBeTapped;

    private bool wasNoteHit = false;
    public static event Action JumpNote;

    private readonly float finePos = 0.77f;
    private readonly float greatPos = 0.35f;
    private readonly float perfectPos = 0.15f;
    private readonly float centerPos = -2.04f;
    private float posDifference;

    #region Events
    private void OnEnable()
    {
        InputManager.TapStarted += TouchStarted;
        InputManager.TapEnded += TouchEnded;
    }
    private void OnDisable()
    {
        InputManager.TapStarted -= TouchStarted;
        InputManager.TapEnded -= TouchEnded;
    }
    private void TouchStarted()
    {
        isThereTouch = true;
    }
    private void TouchEnded()
    {
        isThereTouch = false;
    }
    #endregion

    private void Update()
    {
        if (isThereTouch && canBeTapped)
        {
            wasNoteHit = true;
            posDifference = Math.Abs(transform.position.x - centerPos);
            if (posDifference > finePos)
            {
                GameManager.instance.NoteMissed();
                Destroy(gameObject);
                return;
            }
            else if (posDifference > greatPos)
            {
                GameManager.instance.NoteHit(2);
            }
            else if (posDifference > perfectPos)
            {
                GameManager.instance.NoteHit(1);
            }
            else
            {
                GameManager.instance.NoteHit(0);
            }
            NoteObject.JumpNote?.Invoke();
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBeTapped = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBeTapped = false;
            if (!wasNoteHit)  // if the note exited with no tap
            {
                GameManager.instance.NoteMissed();
                Destroy(gameObject);
            }
        }
    }
}
