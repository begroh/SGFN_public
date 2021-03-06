using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    public enum Direction { FORWARD, REVERSE };

    private ConveyorZone zone;

    public float speed = 2.25f;    // 125 cm / second
    public float margin = 0.3f;    // 30 cm spacing
    public List<ConveyorBeltItem> items;
    public Vector2 startPosition;
    public Vector2 endPosition;

    public List<Player> players;
    public Team currentTeam = Team.NONE;

    private bool sandboxMode = false;

	public AudioClip checkoutSound;
	public AudioSource source;

	private int sandboxCount = 0;

    void Awake()
    {
        items = new List<ConveyorBeltItem>();
        players = new List<Player>();
        zone = transform.Find("Zone").gameObject.GetComponent<ConveyorZone>();

		source = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Setup start and end points
        Bounds bounds = GetComponent<SpriteRenderer>().sprite.bounds;
        float extent = (bounds.extents.y) * 0.95f;

        startPosition = (Vector2) (this.transform.position + transform.localRotation * (Vector2.up * extent));
        endPosition = (Vector2) (this.transform.position + transform.localRotation * (Vector2.down * extent));
    }

    void Update()
    {
        /*
         * If there is just a friendly player nearby, run the conveyor forward.
         * If there is just an enemy player nearby, run the conveyor backward.
         * If the conveyor is contested, don't run it.
         */
        if (!NoPlayersWithTeam(Team.RED) && !NoPlayersWithTeam(Team.BLUE))
        {
            speed = 0f;
            //print("fuk");
            Run(Direction.FORWARD);
        }
        else if (FriendlyPlayers())
        {
			if (sandboxMode)
				speed = 1.5f;
			else
				speed = 3f;
            Run(Direction.FORWARD);
        }
		else if (!EnemyPlayers())
		{
			speed = 0.75f;
            Run(Direction.FORWARD);
		}

        if (NoPlayersWithTeam(currentTeam))
        {
            SetItemColliders(true);
        }
        else
        {
            SetItemColliders(false);
        }
        // else if (EnemyPlayers())
        // {
        //     Run(Direction.REVERSE);
        // }
    }

    public List<ConveyorBeltItem> GetItems()
    {
        return new List<ConveyorBeltItem>(items);
    }

    /*
     * Attempt to add an item to the conveyor belt.
     * Returns whether or not there was room to add the item.
     */
    public bool DepositItem(Player player, FoodItem item)
    {
        if (HasRoom() && (player.GetTeam() == currentTeam))
        {
            GameObject obj = item.gameObject;
            obj.transform.parent = this.gameObject.transform;
            obj.transform.position = startPosition;

            item.player = player;
			item.hitBehaviour.NotifyOnConveyor(player.GetTeam());
			item.conveyor = this;

            items.Add(item);
			item.GetComponent<Collider2D>().enabled = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * Returns true if a friendly player is near the conveyor belt.
     */
    private bool FriendlyPlayers()
    {
        return PlayersPresent() && AllPlayerWithTeam(currentTeam);
    }

    /*
     * Returns true if an enemy player is near the conveyor belt.
     */
    private bool EnemyPlayers()
    {
        return PlayersPresent() && NoPlayersWithTeam(currentTeam);
    }

    private bool AllPlayerWithTeam(Team team) {
        bool all = true;
        foreach (Player player in players)
        {
            if (player.GetTeam() != team)
            {
                all = false;
                break;
            }
        }
        return all;
    }

    private bool NoPlayersWithTeam(Team team)
    {
        foreach (Player player in players)
        {
            if (player.GetTeam() == team)
            {
                return false;
            }
        }
        return true;
    }

    private bool PlayersPresent()
    {
        return players.Count > 0;
    }

    /*
     * If direction is FORWARD, move the conveyor belt items toward endPosition,
     * if the direction is REVERSE, move the conveyor belt items away from endPosition.
     */
    private void Run(Direction direction)
    {
        // See if an item falls off the back of the conveyor
        if (direction == Direction.REVERSE && AvailableItem() != null)
        {
            FallOff(AvailableItem());
        }

        float maxDistance = Time.deltaTime * speed;
        if (sandboxMode)
        {
            maxDistance *= 1.5f;
        }

        if (direction == Direction.REVERSE)
        {
            maxDistance *= -1;
        }

        // Move all of the items on the belt
        // foreach (ConveyorBeltItem item in items)
		for (int i = items.Count - 1; i >= 0; i--)
        {
            Vector2 move = Vector2.MoveTowards(items[i].Position(), endPosition, maxDistance);
            items[i].Move(move);

            if (Vector2.Distance(items[i].Position(), endPosition) < 0.01)
            {
                // An item has reached the end of the belt, move it to the bag.
                MoveItemToBag(items[i]);
            }
        }
    }

    /*
     * Returns the item closest towards the start of the conveyor belt,
     * if it is close enough, otherwise returns null.
     */
    private ConveyorBeltItem AvailableItem()
    {
        ConveyorBeltItem item = LastItem();

        if (item != null && Vector2.Distance(item.Position(), startPosition) < 0.01)
        {
            return item;
        }
        else
        {
            return null;
        }
    }

    /*
     * Returns true if there is enough room to deposit an item.
     */
    public bool HasRoom()
    {
        ConveyorBeltItem last = LastItem();

        if (last == null)
        {
            return true;
        }
        else
        {
            Vector2 position = last.Position();
            float distance = Vector2.Distance(position, startPosition);
            return (distance > margin + last.Size());
        }
    }

    /*
     * Destroy an item that has fallen off the conveyor belt
     * Changes the team to the enemy team
     */
    private void FallOff(ConveyorBeltItem item)
    {
        items.Remove(item);
        item.player.LoseItem(item.AsFoodItem());
        Destroy(item.AsFoodItem().gameObject);

        if (items.Count == 0)
        {
            ChangeTeam(Team.NONE);
        }
    }

	/*
	 * Don't use this if you're not supposed to
	 * Or else
	 */
	public void RemoveItem(FoodItem item)
	{
		foreach (ConveyorBeltItem beltItem in items)
		{
			if (beltItem.AsFoodItem() == item)
			{
				items.Remove(item);
				return;
			}
		}
	}

    private void MoveItemToBag(ConveyorBeltItem item)
    {
        if (sandboxMode)
        {
			sandboxCount++;
			if (sandboxCount >= 2) {
				PlayerSandbox sandbox = transform.parent.GetComponent<PlayerSandbox> ();
				sandbox.Ready ();
			}
        }
        else
        {
            Score.AddForTeam(currentTeam, 5);
            ShoppingList.ForTeam(currentTeam).SetState(item.AsFoodItem().type, FoodState.BAGGED);

			if (!OrderManager.CompleteOrderForTeam (currentTeam)) {
				source.PlayOneShot (checkoutSound);
			}
            foreach (OrderHUD hud in Object.FindObjectsOfType(typeof(OrderHUD)))
            {
                hud.Refresh();
            }
        }

        items.Remove(item);
        Destroy(item.AsFoodItem().gameObject);

		if (!PlayersPresent() && items.Count == 0)
		{
			ChangeTeam(Team.NONE);
		}
    }

    private ConveyorBeltItem LastItem()
    {
        ConveyorBeltItem last = null;
        if (items.Count > 0) {
            last = items[items.Count - 1];
        }

        return last;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //print("enter");
        if (other.tag == "Player")
        {
            PlayerEnter(other.GetComponent<Player>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //print("exit");
        if (other.tag == "Player")
        {
            PlayerExit(other.GetComponent<Player>());
        }
    }

    /*
     * Called when a player enters the conveyor belt zone
     * Changes the current team if there is no team yet
     */
    private void PlayerEnter(Player player)
    {
        if (currentTeam == Team.NONE)
        {
            ChangeTeam(player.GetTeam());
        }
        players.Add(player);
    }

    /*
     * Called when a player exits the conveyor belt zone
     */
    private void PlayerExit(Player player)
    {
        while(players.Remove(player)) {}

		if (items.Count == 0) {
			ChangeTeam (Team.NONE);
		}
    }

    private void SetItemColliders(bool yes)
    {
        foreach (ConveyorBeltItem item in items)
        {
            item.AsFoodItem().GetComponent<Collider2D>().enabled = yes;
        }
    }

    /*
     * Change the currentTeam variable and update the material to reflect the currentTeam
     */
    private void ChangeTeam(Team team)
    {
		currentTeam = team;
		zone.ChangeTeam(team);
    }

    public void EnableSandbox()
    {
        sandboxMode = true;
    }
}
