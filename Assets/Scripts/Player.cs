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

    private Vector3 respawnLoc;

    private float lastTimeHit;
    private Renderer rend;
    private Color startColor;
    private bool invincible = false;
    private int counter;

    public float stunTime, invincibleDuration;
    private float invincibleTime;
    private bool canMove = true;

    private TapBumpBehaviour tapBumpBehaviour;

    public bool canKill = false;
    private float chargeVelocity = 10.0f;
    public HitBehaviour hitBehaviour = new HitBehaviour();

    public PortalManager portals;

	private SpriteRenderer indicator;

    void Awake()
    {
    	hitBehaviour.team = team;

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

		indicator = transform.Find("Indicator").GetComponent<SpriteRenderer>();
		SetIndicatorValues();
    }

    void Start()
    {
        cart = gameObject.GetComponentInChildren<ShoppingCart> ();

		invincibleTime = stunTime + invincibleDuration;
        respawnLoc = transform.position;

        // This will need to be changed if we switch to sprites
        rend = transform.Find("Body").GetComponent<SpriteRenderer>();
        startColor = rend.material.color;

        this.tapBumpBehaviour = new TapBumpBehaviour();
    }

    void Update()
    {

        if (!canMove)
        {
            // Replace with lerp to zero
            this.body.velocity = Vector2.Lerp(this.body.velocity, Vector2.zero, 0.25f);
            return;
        }

        input.DetectInput(this);

		if (HoldingOtherTeamItem())
		{
			indicator.gameObject.SetActive(true);
			indicator.transform.position = transform.position + 2*Vector3.up;
			indicator.transform.rotation = Quaternion.Euler(0,0,180);
		}
		else
		{
			indicator.gameObject.SetActive(false);
		}
    }

    void FixedUpdate()
    {
    	hitBehaviour.Update(body.velocity.magnitude);

        cart.UpdateFoodPositions();
        if (invincible)
        {
            counter++;
            if (counter % 3 != 0)
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
                rend.material.color = Color.clear;
            }
            else
            {
                rend.material.color = startColor;
            }
        }
    }
        
	void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "FoodPickup" && !invincible) {
            if (coll.gameObject.GetComponent<FoodItem>().hitBehaviour.CanHit(hitBehaviour)) {
                coll.gameObject.GetComponent<FoodItem>().hitBehaviour.NotifyHit();
				Die();
				cart.dropAllItems();
                body.AddForce(coll.contacts[0].normal * 5000);
                return;
            }
        }
        else if (coll.gameObject.tag == "Player" && !invincible) {
            if (coll.gameObject.GetComponent<Player>().hitBehaviour.CanHit(hitBehaviour)) {
	            cart.GiveItems(coll.gameObject.GetComponent<Player>().cart);
                Die();
                body.AddForce(coll.contacts[0].normal * 5000);
                return;
            }
        }

		if (coll.gameObject.tag == "FoodPickup")
        {
            HandleFoodPickup (coll.gameObject.GetComponent<FoodItem> ());
        }
    }

    private void Die()
    {
        this.body.velocity = Vector2.zero;
        lastTimeHit = Time.time;
        invincible = true;
        canMove = false;
        //Invoke("TeleportBack", deathTime);
        Invoke("EnableMove", stunTime);
    }

    private void TeleportBack()
    {
        transform.position = respawnLoc;
    }

    private void EnableMove()
    {
        canMove = true;
    }

    /*
     * Called when picking up food. True if the player can pick it up. False if not
     */
    private bool HandleFoodPickup (FoodItem item)
    {
        if (!item.hitBehaviour.CanPickup(team) || !canMove)
            return false;

		// Check to see if it's a potato and set it to not be destroyed
		DegradingCollectible collectible = item.gameObject.GetComponent<DegradingCollectible> ();
		if (collectible != null) {
			collectible.destroy = false;
			collectible.pickup.StartRespawn (5f);
		}

		if (item.conveyor != null)
		{
			item.conveyor.RemoveItem(item);
			item.hitBehaviour.NotifyOffConveyor();
			item.conveyor = null;
		}

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
        if (item)
            item.hitBehaviour.NotifyCharge(team);
    }

    public void HandleTapBump(bool bumping)
    {
        tapBumpBehaviour.Update(this, bumping);
        if (bumping)
           hitBehaviour.NotifyCharge(); 
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
            FoodItem item = cart.Remove(false);

			if (!item)
				return;

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

	void SetIndicatorValues()
	{
		if (team == Team.RED)
			indicator.color = Color.blue;
		else if (team == Team.BLUE)
			indicator.color = Color.red;
	}

	bool HoldingOtherTeamItem()
	{
		Team otherTeam = team == Team.BLUE ? Team.RED : Team.BLUE;
		Order o = OrderManager.OrderForTeam(otherTeam);
		foreach(FoodType itemType in o.Items)
		{
			if (cart.HasFoodType(itemType))
			{
				return true;
			}
		}
		return false;
	}
}
