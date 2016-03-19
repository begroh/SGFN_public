using UnityEngine;
using System.Collections.Generic;

public enum FoodState {IN_CART, ON_CONVEYOR, BAGGED, ON_GROUND}

public class Player : MonoBehaviour
{
    public int playerNumber = 1;    // Joystick slot, default to 1
    public bool useKeyboard = true; // Use keyboard instead of controller, defaults to true for development

	public HUD playerHUD;

    private ShoppingCart cart;
    private Dictionary<FoodType, FoodState> foodStates;

    public float speed = 4;
    private Rigidbody2D body;
    private PlayerControl.PlayerInput input;

    void Awake()
    {
        foodStates = new Dictionary<FoodType, FoodState>();
        foodStates.Add(FoodType.CHEESE, FoodState.ON_GROUND);
        foodStates.Add(FoodType.BREAD, FoodState.ON_GROUND);
        foodStates.Add(FoodType.MEAT, FoodState.ON_GROUND);
        foodStates.Add(FoodType.CONDIMENT, FoodState.ON_GROUND);
        // foodStates.Add(FoodType.BONUS, FoodState.ON_GROUND);

        this.body = GetComponent<Rigidbody2D>();

        if (useKeyboard)
        {
            this.input = new PlayerControl.KeyboardAndMouseInput();
        }
        else
        {
            this.input = new PlayerControl.ControllerInput(this.playerNumber);
        }

        this.cart = new ShoppingCart();
    }

    void Update()
    {
        input.DetectInput(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "FoodPickup")
        {
            // TODO: put the code in this if in a function
            FoodItem item = other.gameObject.GetComponent<FoodItem>();
            FoodType type = item.type;
            FoodState state;
            foodStates.TryGetValue(type, out state);

            if (state != FoodState.ON_GROUND || !cart.Add(item))
            {
                return;
            }

            foodStates[type] = FoodState.IN_CART;
			playerHUD.OnItemStateChanged(type, foodStates[type]);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerFoodPickup (FoodItem item)
    {
    }

    /*
     * Called by a PlayerInput with the users desired move direction
     */
    public void HandleMoveDirection(Vector2 direction)
    {
        this.body.velocity = direction.normalized * speed;
    }

    /*
     * Called by a PlayerInput when the user is holding the shoot button
     */
    public void HandleShoot()
    {
        print("Shoot");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "ConveyorBelt")
        {
            OnConveyorBelt(other.gameObject.GetComponent<ConveyorBelt>());
        }
    }

    private void OnConveyorBelt(ConveyorBelt belt)
    {
        if (cart.Count > 0)
        {
            FoodItem item = cart.Remove();

            if (belt.DepositItem(this, item))
            {
                foodStates[item.type] = FoodState.ON_CONVEYOR;
				playerHUD.OnItemStateChanged(item.type, foodStates[item.type]);
            }
            else
            {
                cart.Add(item);
            }
        }
    }

	public void LoseItem(FoodItem item)
	{
		foodStates[item.type] = FoodState.ON_GROUND;
		playerHUD.OnItemStateChanged(item.type, foodStates[item.type]);
	}

	public void MoveItemToBag(FoodItem item)
	{
		foodStates[item.type] = FoodState.BAGGED;
		playerHUD.OnItemStateChanged(item.type, foodStates[item.type]);
	}
}
