using System;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private bool isThereTouch;
    private bool canBeTapped;

    private bool wasNoteHit = false;
    public static event Action JumpNote;

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
            NoteObject.JumpNote?.Invoke();
            GameManager.instance.NoteHit();
            wasNoteHit = true;
            gameObject.SetActive(false);
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
            if (!wasNoteHit) GameManager.instance.NoteMissed(); // if the note exited with no tap
        }
    }
}
