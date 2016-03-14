using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerNumber = 1;    // Joystick slot, default to 1
    public bool useKeyboard = true; // Use keyboard instead of controller, defaults to true for development

    private float speed = 4;
    private Rigidbody2D body;
    private PlayerControl.PlayerInput input;

    void Awake()
    {
        this.body = GetComponent<Rigidbody2D>();

        if (useKeyboard)
        {
            this.input = new PlayerControl.KeyboardAndMouseInput();
        }
        else
        {
            this.input = new PlayerControl.ControllerInput(this.playerNumber);
        }
    }

    void Update()
    {
        input.DetectInput(this);
    }

    /*
     * Called by a PlayerInput with the users desired move direction
     */
    public void HandleMoveDirection(Vector2 direction)
    {
        this.body.velocity = direction.normalized * speed;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        OnConveyorBelt(other.gameObject.GetComponent<ConveyorBelt>());
    }

    private void OnConveyorBelt(ConveyorBelt belt)
    {
        FoodItem bread = new FoodItem();

        belt.DepositItem(bread);
    }
}
