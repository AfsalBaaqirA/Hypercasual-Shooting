using UnityEngine;

public class PlayerController : BaseController
{
    private GameInput gameInput;

    protected override void Awake()
    {
        base.Awake();

        gameInput = GameInput.Instance;
    }

    private void Update()
    {
        // Handle movement input
        HandleMovementInput(gameInput.GetMovementInputNormalized());
    }

    private void HandleMovementInput(Vector2 movementInput)
    {
        Vector2 movementVector = movementInput;

        this.movementDirection = new Vector3(movementVector.x, 0f, movementVector.y);
    }

    protected override void Move()
    {
        base.Move();
    }
}
