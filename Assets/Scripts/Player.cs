using UnityEngine;
using System.Collections.Generic;

public enum FoodState {IN_CART, ON_CONVEYOR, BAGGED, ON_GROUND}
public enum Team { NONE = 0, RED = 1, BLUE = 2, YELLOW = 3, GREEN = 4 };

public class Player : MonoBehaviour
{
    public int playerNumber = 1;    // Joystick slot, default to 1
    public bool useKeyboard = true; // Use keyboard instead of controller, defaults to true for development

    public HUD playerHUD;

    private ShoppingCart cart;
	private List<FoodItem> bag;

    public float speed = 4;
    private Rigidbody2D body;
    private PlayerControl.PlayerInput input;
	public float hardHitVelocity = 5;

    public int maxHealth = 5;
    private int health;
    private Vector3 respawnLoc;

    private Gun gun;

    public float invincibleTime = 0.5f;
    private float lastTimeHit;
    private Renderer rend;
    private Color startColor;
    private bool invincible = false;
    private int counter;

    private TapBumpBehaviour leftBumpBehaviour;
    private ChargeBumpBehaviour rightBumpBehaviour;


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

		bag = new List<FoodItem>();
    }

    void Start()
    {
        gun = gameObject.GetComponentInChildren<Gun>();
		cart = gameObject.GetComponentInChildren<ShoppingCart> ();
        health = maxHealth;
        respawnLoc = transform.position;

        // This will need to be changed if we switch to sprites
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.material.color;

        this.leftBumpBehaviour = new TapBumpBehaviour();
        this.rightBumpBehaviour = new ChargeBumpBehaviour();
    }

    void Update()
    {
        input.DetectInput(this);
    }

    void FixedUpdate()
    {
        if (invincible)
        {
            counter++;
            if (counter % 5 != 0)
            {
                return;
            }

            if (lastTimeHit + invincibleTime < Time.time)
            {
                invincible = false;
                rend.material.color = startColor;
            }
            else if (rend.material.color == startColor)
            {
                rend.material.color = Color.black;
            }
            else
            {
                rend.material.color = startColor;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            HandleBullet(other.gameObject.GetComponent<Bullet>());
        }
        else if (other.gameObject.tag == "WeaponPickup")
        {
            HandleWeaponPickup(other.gameObject.GetComponent<Gun>());
            Destroy(other.gameObject);
        }
    }
		
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "FoodPickup" || coll.gameObject.tag == "Player") {
			if (HardCollision (coll.gameObject, gameObject)) {
				cart.dropAllItems ();
				return;
			}
		}

		if (coll.gameObject.tag == "FoodPickup")
		{
				if (HandleFoodPickup (coll.gameObject.GetComponent<FoodItem> ())) {

				}
		}
	}

	/*
	 * Pass in two colliding gameObjects and compare their velocities to determine
	 * if they're colliding "hard"
	 */
	private bool HardCollision(GameObject go1, GameObject go2) {
		Vector2 vec1 = go1.GetComponent<Rigidbody2D> ().velocity;
		Vector2 vec2 = go2.GetComponent<Rigidbody2D> ().velocity;

		float mag = Mathf.Sqrt (Mathf.Pow (vec1.x - vec2.x, 2f) + Mathf.Pow (vec1.y - vec2.y, 2f));
		return (mag >= hardHitVelocity);
	}

    private void HandleWeaponPickup(Gun newGun)
    {
        gun.Copy(newGun);
        input.SetInputOnHold(gun.holdToFire);
    }

    /*
     * Called when colliding with a bullet. Player takes damage and resets
     * position if they died
     */

    private void HandleBullet (Bullet bullet)
    {
        if (/*invincible ||*/ !bullet.fired)
            return;

        health -= bullet.damage;
        invincible = true; lastTimeHit = Time.time;
        if (health <= 0)
        {
            gun.Reset();
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

		return cart.Add (item);
    }

    /*
     * Called by a PlayerInput with the users desired move direction
     */
    public void HandleMoveDirection(Vector2 direction)
    {
        this.body.velocity = Vector2.Lerp(this.body.velocity, direction.normalized * speed, 0.20f);
    }

    public void HandleAimDirection(Vector2 dir)
    {
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
		gameObject.transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }

    /*
     * Called by a PlayerInput when the user is holding the shoot button
     */
    public void HandleShoot()
    {
        FoodItem item = cart.Fire();
    }

    public void HandleLeftBump(bool bumping)
    {
        leftBumpBehaviour.Update(this, bumping);
    }

    public void HandleRightBump(bool bumping)
    {
        rightBumpBehaviour.Update(this, bumping);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "ConveyorBelt")
        {
            OnConveyorBelt(other.gameObject.GetComponent<ConveyorBelt>());
        }
        else if (other.gameObject.tag == "Bullet")
        {
            HandleBullet(other.gameObject.GetComponent<Bullet>());
        }
    }

    private void OnConveyorBelt(ConveyorBelt belt)
    {
        if (cart.Count > 0)
        {
			FoodItem item = cart.Remove();

            if (belt.DepositItem(this, item))
            {
				
            }
            else
            {
                cart.Add(item);
            }
        }
    }

	public void LoseItem(FoodItem item)
	{
	}

	public void MoveItemToBag(FoodItem item)
	{
		bag.Add(item);
		if (ContainsFullSandwich())
		{
			playerHUD.IncrementSandwiches();
			bag.Clear();
		}
	}

    public Team GetTeam()
    {
        return (Team) playerNumber;
    }

	/*
	 * On completion of a sandwich, all food states go back to ON_GROUND
	 * so the player can collect them again
	 */
	/*
	void ResetFoodStates()
	{
		List<FoodType> keys = new List<FoodType>(foodStates.Keys);
		foreach (FoodType key in keys)
		{
			foodStates[key] = FoodState.ON_GROUND;
		}
	}
	*/

	bool ContainsFullSandwich()
	{
		bool hasCheese = false, hasBread = false,
			 hasTopping = true, hasMeat = false;
		foreach (FoodItem item in bag)
		{
			if (item.type == FoodType.CHEESE)
				hasCheese = true;
			else if (item.type == FoodType.BREAD)
				hasBread = true;
			else if (item.type == FoodType.TOPPING)
				hasTopping = true;
			else if (item.type == FoodType.MEAT)
				hasMeat = true;
		}
		return hasCheese && hasBread && hasTopping && hasMeat;
	}
}
