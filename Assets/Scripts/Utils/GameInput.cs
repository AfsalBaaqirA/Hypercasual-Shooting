using UnityEngine;
// using UniversalMobileController;

public class GameInput : MonoBehaviour
{
    [SerializeField] private FloatingJoystick floatingJoyStick;

    public static GameInput Instance
    { get; private set; }

    private PlayerInputActions playerInputActions;

    [SerializeField] private InputType inputType;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable(); // Enable the player input actions
    }

    private void OnDestroy()
    {
        playerInputActions.Dispose(); // Dispose of the player input actions
    }

    public Vector2 GetMovementInputNormalized()
    {
        Vector2 inputVector = Vector2.zero;

        switch (inputType)
        {
            case InputType.Keyboard:
                inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>(); // Get the movement input
                break;
            case InputType.Mobile:
                inputVector = new Vector2(floatingJoyStick.GetHorizontalValue(), floatingJoyStick.GetVerticalValue()); // Get the movement input
                break;
        }
        inputVector = inputVector.normalized; // Normalize the input vector
        return inputVector;
    }
}

public enum InputType
{
    Keyboard,
    Mobile
}