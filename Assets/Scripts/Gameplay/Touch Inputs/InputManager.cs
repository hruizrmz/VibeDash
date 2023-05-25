using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Runner player;

    private PlayerInput playerInput;
    private InputAction touchAction;
    private InputAction primaryPositionAction;

    [Header("Swipe Variables")]
    [SerializeField] private float minDistance = 0.2f;
    [SerializeField] private float maxTime = 1f;
    [SerializeField, Range(0,1f)] private float directionThreshold = 0.9f;
    private float startTouchTime, endTouchTime;
    private Vector3 startTouchPos, endTouchPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Runner>();

        playerInput = GetComponent<PlayerInput>();
        touchAction = playerInput.actions["Touch"];
        primaryPositionAction = playerInput.actions["PrimaryPosition"];
    }

    private void OnEnable()
    {
        touchAction.started += TouchStarted;
        touchAction.canceled += TouchEnded;
    }

    private void OnDisable()
    {
        touchAction.started -= TouchStarted;
        touchAction.canceled -= TouchEnded;
    }

    private void TouchStarted(InputAction.CallbackContext context)
    {
        player.jumpInput = true;
        startTouchTime = Time.time;
        startTouchPos = Camera.main.ScreenToWorldPoint(primaryPositionAction.ReadValue<Vector2>());
        startTouchPos.z = 0;
    }

    private void TouchEnded(InputAction.CallbackContext context)
    {
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
            Debug.Log("swipe up");
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("swipe down");
        }
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("swipe left");
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("swipe right");
        }
    }
}
