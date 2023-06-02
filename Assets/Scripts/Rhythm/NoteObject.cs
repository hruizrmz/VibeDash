using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    double timeInstantiated; // time of spawn
    public float assignedTime; // time until perfect tap in seconds

    private bool isThereTouch;
    private bool canBeTapped;

    private bool wasNoteHit = false;
    public int noteType = 0; // 0 jump, 1 long jump, 2 swipe up, 3 swipe down, 4 normal
    public static event Action JumpNote, LongJumpNote, SwipeUpNote, SwipeDownNote, ObstacleMissed;
    private readonly Dictionary<int, Action> noteTypeActions = new Dictionary<int, Action>();

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

    private void Start()
    {
        noteTypeActions.Add(0, JumpNote);
        noteTypeActions.Add(1, LongJumpNote);
        noteTypeActions.Add(2, SwipeUpNote);
        noteTypeActions.Add(3, SwipeDownNote);

        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    private void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2)); // x2 to get spawn time before and after noteTapX

        if ( t > 1 )
        {
            Destroy(gameObject);
        }
        else
        {
            // t = 0 spawn, t = 1 despawn, anything in between is lane
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnX, Vector3.right * SongManager.Instance.noteDespawnX, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if (isThereTouch && canBeTapped)
        {
            wasNoteHit = true;
            posDifference = Math.Abs(transform.position.x - centerPos);
            if (posDifference > finePos)
            {
                ScoreManager.Instance.NoteMissed();
                Destroy(gameObject);
                return;
            }
            else if (posDifference > greatPos)
            {
                ScoreManager.Instance.NoteHit(2);
            }
            else if (posDifference > perfectPos)
            {
                ScoreManager.Instance.NoteHit(1);
            }
            else
            {
                ScoreManager.Instance.NoteHit(0);
            }

            if (noteType != 4) noteTypeActions[noteType]?.Invoke();
            
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
                if (noteType == 2) ObstacleMissed?.Invoke(); // if punch was missed, obstacle trips player
                ScoreManager.Instance.NoteMissed(); 
                Destroy(gameObject);
            }
        }
    }
}
