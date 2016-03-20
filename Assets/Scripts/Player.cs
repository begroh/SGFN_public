using UnityEngine;
using System.Collections.Generic;

public enum FoodState {IN_CART, ON_CONVEYER, BAGGED, ON_GROUND}

public class Player : MonoBehaviour
{
    public int playerNumber = 1;    // Joystick slot, default to 1
    public bool useKeyboard = true; // Use keyboard instead of controller, defaults to true for development

    private ShoppingCart cart;
    private Dictionary<FoodType, FoodState> foodStates;

    public float speed = 4;
    private Rigidbody2D body;
    private PlayerControl.PlayerInput input;

    public int maxHealth = 5;
    private int health;
    private Vector3 respawnLoc;

    private Gun gun;

    public float invincibleTime = 0.5f;
    private float lastTimeHit;
    private Renderer rend;
    private Color startColor;
    private bool invincible = false;

    void Awake()
    {
        foodStates = new Dictionary<FoodType, FoodState>();
        foodStates.Add(FoodType.CHEESE, FoodState.ON_GROUND);
        foodStates.Add(FoodType.BREAD, FoodState.ON_GROUND);
        foodStates.Add(FoodType.MEAT, FoodState.ON_GROUND);
        foodStates.Add(FoodType.CONDIMENT, FoodState.ON_GROUND);
        foodStates.Add(FoodType.BONUS, FoodState.ON_GROUND);

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

    void Start()
    {
        gun = gameObject.GetComponentInChildren<Gun>();
        health = maxHealth;
        respawnLoc = transform.position;

        // This will need to be changed if we switch to sprites
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void Update()
    {
        input.DetectInput(this);
    }

    void FixedUpdate()
    {
        if (invincible)
        {
            if (lastTimeHit + invincibleTime < Time.time)
            {
                invincible = false;
                rend.material.color = startColor;
            }
            else if (rend.material.color == startColor)
            {
                rend.material.color = Color.white;
            }
            else
            {
                rend.material.color = startColor;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "FoodPickup")
        {
            if (HandleFoodPickup(other.gameObject.GetComponent<FoodItem>()))
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.tag == "Bullet")
        {
            HandleBullet(other.gameObject.GetComponent<Bullet>());
        }
    }

    private void HandleBullet (Bullet bullet)
    {
        if (invincible)
            return;

        health -= bullet.damage;
        invincible = true; lastTimeHit = Time.time;
        if (health <= 0)
        {
            health = maxHealth;
            transform.position = respawnLoc;
        }
    }

    /*
     * Called when picking up food. True if the player can pick it up. False if not
     */
    private bool HandleFoodPickup (FoodItem item)
    {
        FoodType type = item.type;
        FoodState state;
        foodStates.TryGetValue(type, out state);

        if (state != FoodState.ON_GROUND || !cart.Add(item))
        {
            return false;
        }

        foodStates[type] = FoodState.IN_CART;
        return true;
    }

    /*
     * Called by a PlayerInput with the users desired move direction
     */
    public void HandleMoveDirection(Vector2 direction)
    {
        this.body.velocity = direction.normalized * speed;
    }

    public void HandleAimDirection(Vector2 dir)
    {
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        gun.transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }

    /*
     * Called by a PlayerInput when the user is holding the shoot button
     */
    public void HandleShoot()
    {
        gun.Fire();
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
                foodStates[item.type] = FoodState.ON_CONVEYER;
            }
            else
            {
                cart.Add(item);
            }
        }
    }
}
