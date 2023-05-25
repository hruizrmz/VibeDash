using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Runner player;

    private PlayerInput playerInput;
    private InputAction touchAction;
    private InputAction holdAction;
    private InputAction primaryPositionAction;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Runner>();

        playerInput = GetComponent<PlayerInput>();
        touchAction = playerInput.actions["Touch"];
        holdAction = playerInput.actions["Hold"];
        primaryPositionAction = playerInput.actions["PrimaryPosition"];
    }

    private void OnEnable()
    {
        touchAction.started += Touched;
        holdAction.started += HoldStarted;
        holdAction.canceled += HoldEnded;
    }

    private void OnDisable()
    {
        touchAction.started -= Touched;
        holdAction.started -= HoldStarted;
        holdAction.canceled -= HoldEnded;
    }

    private void Touched(InputAction.CallbackContext context)
    {
        player.jumpInput = true;

        Vector3 position = Camera.main.ScreenToWorldPoint(primaryPositionAction.ReadValue<Vector2>());
        position.z = 0;
    }

    private void HoldStarted(InputAction.CallbackContext context)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(primaryPositionAction.ReadValue<Vector2>());
        position.z = 0;
    }

    private void HoldEnded(InputAction.CallbackContext context)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(primaryPositionAction.ReadValue<Vector2>());
        position.z = 0;
    }
}
