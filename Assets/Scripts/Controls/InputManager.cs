using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public static event Action TapStarted, TapEnded, HoldEnded;
    public static event Action SwipeUp, SwipeDown, SwipeRight;

    [SerializeField] private GameObject touchIndicator;
    private Animator touchAnimator;

    private PlayerInput playerInput;
    private InputAction touchAction;
    private InputAction primaryPositionAction;

    [Header("Swipe Variables")]
    [SerializeField] private float minDistance = 0.2f;
    [SerializeField] private float maxTime = 1f;
    [SerializeField, Range(0,1f)] private float directionThreshold = 0.9f;
    private bool isTouching, isHolding, checkForHold;
    private float touchDuration;
    [Header("Hold Variables")]
    [SerializeField] private float timeUntilHold = 0.4f;
    private float startTouchTime, endTouchTime;
    private Vector3 startTouchPos, endTouchPos;

    private void Awake()
    {
        Instance = this;

        isTouching = false;
        touchAnimator = touchIndicator.GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        touchAction = playerInput.actions["Touch"];
        primaryPositionAction = playerInput.actions["PrimaryPosition"];
    }

    private void OnEnable()
    {
        touchAction.started += TouchStarted;
        touchAction.canceled += TouchEnded;
        NoteObject.HoldStarted += StartHold;
    }

    private void OnDisable()
    {
        touchAction.started -= TouchStarted;
        touchAction.canceled -= TouchEnded;
        NoteObject.HoldStarted -= StartHold;
    }

    private void TouchStarted(InputAction.CallbackContext context)
    {
        InputManager.TapStarted?.Invoke();
        isTouching = true;
        startTouchTime = Time.time;
        startTouchPos = Camera.main.ScreenToWorldPoint(primaryPositionAction.ReadValue<Vector2>());
        startTouchPos.z = 0;
        touchIndicator.transform.position = startTouchPos;
        touchAnimator.SetTrigger("screenTouched");
    }

    private void TouchEnded(InputAction.CallbackContext context)
    {
        InputManager.TapEnded?.Invoke();
        isTouching = false;
        endTouchTime = Time.time;
        endTouchPos = Camera.main.ScreenToWorldPoint(primaryPositionAction.ReadValue<Vector2>());
        endTouchPos.z = 0;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if ((Vector3.Distance(startTouchPos, endTouchPos) >= minDistance) &&
                ((endTouchTime - startTouchTime) < maxTime))
        {
            Debug.DrawLine(startTouchPos, endTouchPos, Color.red, 2f);
            Vector3 distance = endTouchPos - startTouchPos;
            Vector2 direction2D = new Vector2(distance.x, distance.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            InputManager.SwipeUp?.Invoke();
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            InputManager.SwipeDown?.Invoke();
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            InputManager.SwipeRight?.Invoke();
        }
    }

    private void StartHold()
    {
        checkForHold = true;
    }

    private void Update()
    {
        if (checkForHold)
        {
            if (isTouching && !isHolding)
            {
                touchDuration = Time.time - startTouchTime;
                if (touchDuration > timeUntilHold)
                {
                    isHolding = true;
                }
            }

            if (!isHolding && !isTouching) // player never started holding
            {
                InputManager.HoldEnded?.Invoke();
                checkForHold = false;
            }

            if (isHolding && !isTouching)
            {
                InputManager.HoldEnded?.Invoke();
                isHolding = false;
                checkForHold = false;
            }
        }
    }
}