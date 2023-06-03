using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    double timeInstantiated; // time of spawn
    public float assignedTime; // time until perfect tap in seconds

    private bool isThereTouch, isThereSwipeU, isThereSwipeD, isThereSwipeR;
    private bool canBeTapped;
    private bool wasNoteHit = false;

    public int noteType = 0; // 0 tap, 1 hold, 2 swipe up, 3 swipe down, 4 swipe right
    public static event Action TapNote, HoldNote, SwipeUpNote, SwipeDownNote, SwipeRightNote, ObstacleMissed;
    private readonly Dictionary<int, Action> noteTypeActions = new Dictionary<int, Action>();

    private readonly float finePos = 1.2f;
    private readonly float greatPos = 0.6f;
    private readonly float perfectPos = 0.3f;
    private readonly float centerPos = -2.04f;
    private float posDifference;

    private int noteID;
    // public GameObject holdBarPrefab;
    // public GameObject holdEndPrefab;
    // add X number of bars depending on note length, and then spawn end after them
    // only start and end should have collider
    // make bars really tiny and hide their Destroy behind a white particle effect
    // https://www.reddit.com/r/gamedev/comments/m7dsa5/how_to_make_longheld_notes_in_a_unity_rhythm_game/
    // https://youtu.be/89KpbT_7Ysg?t=276

    #region Events
    private void OnEnable()
    {
        InputManager.TapStarted += TouchStarted;
        InputManager.TapEnded += TouchEnded;
        InputManager.SwipeUp += SwipeUp;
        InputManager.SwipeDown += SwipeDown;
        InputManager.SwipeRight += SwipeRight;
    }
    private void OnDisable()
    {
        InputManager.TapStarted -= TouchStarted;
        InputManager.TapEnded -= TouchEnded;
        InputManager.SwipeUp -= SwipeUp;
        InputManager.SwipeDown -= SwipeDown;
        InputManager.SwipeRight -= SwipeRight;
    }
    private void TouchStarted()
    {
        if (canBeTapped) isThereTouch = true;
    }
    private void TouchEnded()
    {
        isThereTouch = false;
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
        noteTypeActions.Add(0, TapNote);
        noteTypeActions.Add(1, HoldNote);
        noteTypeActions.Add(2, SwipeUpNote);
        noteTypeActions.Add(3, SwipeDownNote);
        noteTypeActions.Add(4, SwipeRightNote);

        timeInstantiated = assignedTime - SongManager.Instance.noteTime;

        noteID = ScoreManager.Instance.notesSpawned - 1;
        noteType = GetComponentInParent<Lane>().laneID;

        /*
        if (noteType == 2)
        {
            Instantiate(holdBarPrefab, transform);
        }
        */
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

        if (canBeTapped)
        {
            if (noteType == 0 && isThereTouch)
            {
                wasNoteHit = true;
            }
            else if (noteType == 1 && isThereTouch)
            {
                wasNoteHit = true;
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

            if (wasNoteHit)
            {
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

                noteTypeActions[noteType]?.Invoke();

                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator" && (noteID == ScoreManager.Instance.currentNote))
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
                ScoreManager.Instance.currentNote++;
                ScoreManager.Instance.NoteMissed(); 
                Destroy(gameObject);
            }
        }
    }
}
