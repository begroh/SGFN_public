using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    private enum Direction { FORWARD, REVERSE };

    public GameObject foodConveyorBeltItemPrefab;

    private ConveyorZone zone;

    private float speed = 1.25f;    // 125 cm / second
    private float margin = 0.3f;    // 30 cm spacing
    private LinkedList<FoodConveyorBeltItem> items;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private List<Player> players;
    private Team currentTeam = Team.NONE;

    void Awake()
    {
        items = new LinkedList<FoodConveyorBeltItem>();
        players = new List<Player>();

        // Setup start and end points
        Bounds bounds = GetComponent<SpriteRenderer>().sprite.bounds;
        float extent = (bounds.extents.y * transform.localScale.y) * 0.95f;
        startPosition = (Vector2) (this.transform.position + transform.localRotation * (Vector2.up * extent));
        endPosition = (Vector2) (this.transform.position + transform.localRotation * (Vector2.down * extent));

        zone = transform.Find("Zone").gameObject.GetComponent<ConveyorZone>();
    }

    void Update()
    {
        /*
         * If there is just a friendly player nearby, run the conveyor forward.
         * If there is just an enemy player nearby, run the conveyor backward.
         * If the conveyor is contested, don't run it.
         */
        if (FriendlyPlayers())
        {
            Run(Direction.FORWARD);
        }
        else if (EnemyPlayers())
        {
            Run(Direction.REVERSE);
        }
    }

    public List<FoodConveyorBeltItem> GetItems()
    {
        return new List<FoodConveyorBeltItem>(items);
    }

    /*
     * Attempt to add an item to the conveyor belt.
     * Returns whether or not there was room to add the item.
     */
    public bool DepositItem(Player player, FoodItem item)
    {
        if (HasRoom() && (player.GetTeam() == currentTeam))
        {
            GameObject obj = Instantiate(foodConveyorBeltItemPrefab);
            obj.transform.parent = this.gameObject.transform;
            obj.transform.position = startPosition;
            FoodConveyorBeltItem beltItem = obj.GetComponent<FoodConveyorBeltItem>();
            beltItem.SetItem(item);
			beltItem.player = player;

            items.AddLast(beltItem);
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
        return PlayersPresent() && !AllPlayerWithTeam(currentTeam);
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
        if (direction == Direction.REVERSE)
        {
            maxDistance *= -1;
        }

        // Move all of the items on the belt
        foreach (FoodConveyorBeltItem item in items)
        {
            Vector2 move = Vector2.MoveTowards(item.Position(), endPosition, maxDistance);
            item.Move(move);

            if (Vector2.Distance(item.Position(), endPosition) < 0.01)
            {
                // An item has reached the end of the belt, move it to the bag.
                MoveItemToBag(item);
            }
        }
    }

    /*
     * Returns the item closest towards the start of the conveyor belt,
     * if it is close enough, otherwise returns null.
     */
    private FoodConveyorBeltItem AvailableItem()
    {
        FoodConveyorBeltItem item = LastItem();

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
        FoodConveyorBeltItem last = LastItem();

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
    private void FallOff(FoodConveyorBeltItem item)
    {
        items.Remove(item);
		item.player.LoseItem(item.Item());
        Destroy(((FoodConveyorBeltItem) item).gameObject);

        if (items.Count == 0)
        {
            ChangeTeam(Team.NONE);
        }
    }

    private void MoveItemToBag(FoodConveyorBeltItem item)
    {
        items.Remove(item);
		item.player.MoveItemToBag(item.Item());
        Destroy(((FoodConveyorBeltItem) item).gameObject);
    }

    private FoodConveyorBeltItem LastItem()
    {
        FoodConveyorBeltItem last = null;
        if (items.Last != null) {
            last = items.Last.Value;
        }

        return last;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerEnter(other.GetComponent<Player>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
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
        players.Remove(player);
    }

    /*
     * Change the currentTeam variable and update the material to reflect the currentTeam
     */
    private void ChangeTeam(Team team)
    {
        zone.ChangeTeam(team);
    }
}
