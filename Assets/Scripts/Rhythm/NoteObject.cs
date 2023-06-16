using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    double timeInstantiated; // time of spawn
    public float assignedTime; // time until perfect tap in seconds

    private bool isThereTouch, isThereHold, isThereSwipeU, isThereSwipeD, isThereSwipeR;
    private bool canBeTapped;
    private bool wasNoteHit = false;
    private bool safeToEndHold = false;

    public int noteType = 0; // 0 tap, 1 hold, 2 swipe up, 3 swipe down, 4 swipe right, 5 hold body, 6 hold end
    public static event Action TapNote, HoldNote, SwipeUpNote, SwipeDownNote, SwipeRightNote, HoldStarted, HoldMissed, ObstacleMissed;
    private readonly Dictionary<int, Action> noteTypeActions = new Dictionary<int, Action>();

    private readonly float finePos = 1.4f;
    private readonly float greatPos = 0.9f;
    private readonly float perfectPos = 0.4f;
    private readonly float centerPos = -2.04f;
    private float posDifference;

    private int noteID;
    private bool spriteDisabled = true;

    #region Events
    private void OnEnable()
    {
        HoldMissed += DeleteHoldNote;
        InputManager.TapStarted += TouchStarted;
        InputManager.TapEnded += TouchEnded;
        InputManager.HoldEnded += HoldEnded;
        InputManager.SwipeUp += SwipeUp;
        InputManager.SwipeDown += SwipeDown;
        InputManager.SwipeRight += SwipeRight;
    }
    private void OnDisable()
    {
        HoldMissed -= DeleteHoldNote;
        InputManager.TapStarted -= TouchStarted;
        InputManager.TapEnded -= TouchEnded;
        InputManager.HoldEnded -= HoldEnded;
        InputManager.SwipeUp -= SwipeUp;
        InputManager.SwipeDown -= SwipeDown;
        InputManager.SwipeRight -= SwipeRight;
    }
    private void DeleteHoldNote() // for when a hold note is never started
    {
        if (noteID == (ScoreManager.Instance.currentNote - 1)) Destroy(gameObject);
    }
    private void TouchStarted()
    {
        if (canBeTapped) isThereTouch = true;
    }
    private void TouchEnded()
    {
        isThereTouch = false;
    }
    private void HoldEnded() // for when a hold note is started but finished/unfinished
    {
        if (noteID == (ScoreManager.Instance.currentNote))
        {
            if (noteType == 6)
            {
                if (safeToEndHold)
                {
                    isThereHold = false;
                }
                else
                {
                    ScoreManager.Instance.currentNote++;
                    ScoreManager.Instance.NoteMissed();
                    HoldMissed?.Invoke();
                }
            }
        }
    }
    private void SwipeUp()
    {
        if (canBeTapped) isThereSwipeU = true;
    }
    private void SwipeDown()
    {
        if (canBeTapped) isThereSwipeD = true;
    }
    private void SwipeRight()
    {
        if (canBeTapped) isThereSwipeR = true;
    }
    #endregion

    private void Start()
    {
        if (noteType != 5) // so runner doesn't crash with notes while respawning
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(), GetComponent<CircleCollider2D>());
        }

        // for the runner animations
        noteTypeActions.Add(0, TapNote);
        noteTypeActions.Add(1, HoldNote);
        noteTypeActions.Add(2, SwipeUpNote);
        noteTypeActions.Add(3, SwipeDownNote);
        noteTypeActions.Add(4, SwipeRightNote);

        timeInstantiated = assignedTime - SongManager.Instance.noteTime;

        noteID = ScoreManager.Instance.notesSpawned - 1;

        if (noteType == 6) isThereHold = true;
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
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnX, Vector3.right * SongManager.Instance.noteDespawnX, t);
            // t = 0 spawn, t = 1 despawn, anything in between is lane
            if (noteType == 5)
            {
                if (transform.localPosition.x <= (SongManager.Instance.noteTapX))
                {
                    Destroy(gameObject);
                }
            }
            if (spriteDisabled)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                spriteDisabled = false;
            }
        }

        if (canBeTapped)
        {
            if (noteType == 0 && isThereTouch)
            {
                wasNoteHit = true;
            }
            else if (noteType == 1 && isThereTouch)
            {
                ScoreManager.Instance.PlayHitSound();
                wasNoteHit = true;
                HoldStarted?.Invoke();
                // noteTypeActions[noteType]?.Invoke();
                Destroy(gameObject);
                return;
            }
            else if (noteType == 2 && isThereSwipeU)
            {
                wasNoteHit = true;
            }
            else if (noteType == 3 && isThereSwipeD)
            {
                wasNoteHit = true;
            }
            else if (noteType == 4 && isThereSwipeR)
            {
                wasNoteHit = true;
            }
            else if (noteType == 6 && !isThereHold)
            {
                wasNoteHit = true;
                safeToEndHold = false;
            }

            if (wasNoteHit)
            {
                if (noteType != 6) ScoreManager.Instance.PlayHitSound();
                ScoreManager.Instance.currentNote++;

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

                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator" && noteID == ScoreManager.Instance.currentNote)
        {
            canBeTapped = true;
            if (noteType == 6) safeToEndHold = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (noteType != 5 && noteType != 6) noteTypeActions[noteType]?.Invoke();
        if (collision.tag == "Activator")
        {
            canBeTapped = false;
            if (!wasNoteHit)
            {
                ScoreManager.Instance.currentNote++;
                ScoreManager.Instance.NoteMissed();
                if (noteType == 6) safeToEndHold = false;
                if (noteType == 4) ObstacleMissed?.Invoke();
                if (noteType == 1) HoldMissed?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
