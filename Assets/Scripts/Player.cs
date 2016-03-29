using UnityEngine;
using System.Collections.Generic;

public enum FoodState {IN_CART, ON_CONVEYOR, BAGGED, ON_GROUND}
public enum Team { NONE = 0, RED = 1, BLUE = 2, YELLOW = 3, GREEN = 4 };

public class Player : MonoBehaviour
{
    public int playerNumber = 1;    // Joystick slot, default to 1
    public bool useKeyboard = true; // Use keyboard instead of controller, defaults to true for development

    public Team team;
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

    private float lastTimeHit;
    private Renderer rend;
    private Color startColor;
    private bool invincible = false;
    private int counter;
	private bool hardHit = false;

    public float deathTime, respawnDelay, invincibleDuration;
    private float invincibleTime;
    private bool canMove = true;

    private ChargeBumpBehaviour leftBumpBehaviour;
    private TapBumpBehaviour rightBumpBehaviour;

    public PortalManager portals;

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
        invincibleTime = deathTime + respawnDelay + invincibleDuration;
        cart = gameObject.GetComponentInChildren<ShoppingCart> ();
        health = maxHealth;
        respawnLoc = transform.position;

        // This will need to be changed if we switch to sprites
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.material.color;

        this.leftBumpBehaviour = new ChargeBumpBehaviour();
        this.rightBumpBehaviour = new TapBumpBehaviour();
    }

    void Update()
    {
        if (!canMove)
        {
            this.body.velocity = Vector2.zero;
            return;
        }

        input.DetectInput(this);
    }

    void FixedUpdate()
    {
        cart.UpdateFoodPositions();
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
        if (other.gameObject.tag == "Portal") {
            Vector3 newPos = gameObject.transform.position;
            Quaternion newRot = gameObject.transform.rotation;

            bool canPortal = portals.portalMove(other.gameObject.GetComponent<Portal>().portalID, ref newPos, ref newRot);

            if (canPortal) {
                gameObject.transform.position = newPos;
                gameObject.transform.rotation = newRot;
            }
        }
    }
        
    void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "FoodPickup") {
			FoodItem item = (FoodItem)coll.gameObject.GetComponent<FoodItem>();
			if (item.isExploding) {
				return;
			}
		}


        if (coll.gameObject.tag == "FoodPickup" || coll.gameObject.tag == "Player" && !invincible) {
            if (HardCollision (coll.gameObject, gameObject)) {
                if (cart.Count == 0)
                {
                    Die();
                }
                else
                {
                    cart.dropAllItems();
                }
				hardHit = true;
				Invoke ("HardHitStop", deathTime);
                return;
            }
        }

		if (coll.gameObject.tag == "FoodPickup" && !hardHit && coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 15)
        {
            HandleFoodPickup (coll.gameObject.GetComponent<FoodItem> ());
        }
    }

    /*
     * Pass in two colliding gameObjects and compare their velocities to determine
     * if they're colliding "hard"
     */
    private bool HardCollision(GameObject go1, GameObject go2) {
        //Vector2 vec1 = go1.GetComponent<Rigidbody2D> ().velocity;
        Vector2 vec1 = Vector2.zero;
        Vector2 vec2 = go2.GetComponent<Rigidbody2D> ().velocity;

        float mag = Mathf.Sqrt (Mathf.Pow (vec1.x - vec2.x, 2f) + Mathf.Pow (vec1.y - vec2.y, 2f));
        return (mag >= hardHitVelocity);
    }

    private void Die()
    {
        this.body.velocity = Vector2.zero;
        lastTimeHit = Time.time;
        invincible = true;
        canMove = false;
        Invoke("TeleportBack", deathTime);
        Invoke("EnableMove", respawnDelay + deathTime);
    }

    private void TeleportBack()
    {
        transform.position = respawnLoc;
    }

    private void EnableMove()
    {
        canMove = true;
    }

	private void HardHitStop()
	{
		hardHit = false;
	}

    /*
     * Called when picking up food. True if the player can pick it up. False if not
     */
    private bool HandleFoodPickup (FoodItem item)
    {
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
        if (invincible)
            return;

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

    public Team GetTeam()
    {
        return team;
    }
}
