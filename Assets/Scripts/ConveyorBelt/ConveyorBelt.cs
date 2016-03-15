using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    private enum Direction { FORWARD, REVERSE };

    private enum Team { NONE, RED, BLUE };

    public GameObject foodConveyorBeltItemPrefab;
    public Material defaultMaterial;
    public Material blueMaterial;
    public Material redMaterial;

    private float speed = 0.5f;     // 50 cm / second
    private float margin = 0.1f;    // 10 cm spacing
    private LinkedList<ConveyorBeltItem> items;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private List<Player> players;
    private Team currentTeam = Team.NONE;

    void Awake()
    {
        items = new LinkedList<ConveyorBeltItem>();
        players = new List<Player>();
        startPosition = (Vector2) this.transform.position + Vector2.up;
        endPosition = (Vector2) this.transform.position + Vector2.down;
    }

    void Update()
    {
        /*
         * If there is just a friendly player nearby, run the conveyor forward.
         * If there is just an enemy player nearby, run the conveyor backward.
         * If the conveyor is contested, don't run it.
         */
        if (FriendlyPlayer() && !EnemyPlayer())
        {
            Run(Direction.FORWARD);
        }
        else if (EnemyPlayer() && !FriendlyPlayer())
        {
            Run(Direction.REVERSE);
        }
    }

    public List<ConveyorBeltItem> GetItems()
    {
        return new List<ConveyorBeltItem>(items);
    }

    /*
     * Attempt to add an item to the conveyor belt.
     * Returns whether or not there was room to add the item.
     */
    public bool DepositItem(FoodItem item)
    {
        if (HasRoom())
        {
            GameObject obj = Instantiate(foodConveyorBeltItemPrefab);
            obj.transform.parent = this.gameObject.transform;
            obj.transform.position = startPosition;
            FoodConveyorBeltItem beltItem = obj.GetComponent<FoodConveyorBeltItem>();
            beltItem.SetItem(item);

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
    private bool FriendlyPlayer()
    {
        return AnyPlayerWithTeam(currentTeam);
    }

    /*
     * Returns true if an enemy player is near the conveyor belt.
     */
    private bool EnemyPlayer()
    {
        return AnyPlayerWithTeam(EnemyTeam());
    }

    private bool AnyPlayerWithTeam(Team team) {
        bool present = false;
        foreach (Player player in players)
        {
            if ((team == Team.BLUE && player.playerNumber < 3) ||
                (team == Team.RED  && player.playerNumber > 2))
            {
                present = true;
                break;
            }
        }
        return present;
    }

    private Team EnemyTeam()
    {
        if (currentTeam == Team.RED)
        {
            return Team.BLUE;
        }
        else if (currentTeam == Team.BLUE)
        {
            return Team.RED;
        }
        else
        {
            return Team.NONE;
        }
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
        foreach (ConveyorBeltItem item in items)
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
        Destroy(((FoodConveyorBeltItem) item).gameObject);

        if (items.Count == 0)
        {
            ChangeTeam(EnemyTeam());
        }
    }

    private void MoveItemToBag(ConveyorBeltItem item)
    {
        items.Remove(item);
        Destroy(((FoodConveyorBeltItem) item).gameObject);
    }

    private ConveyorBeltItem LastItem()
    {
        ConveyorBeltItem last = null;
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
            ChangeTeam((player.playerNumber < 3) ? Team.BLUE : Team.RED);
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
        currentTeam = team;

        Renderer renderer = GetComponent<Renderer>();

        if (team == Team.BLUE)
        {
            renderer.material = blueMaterial;
        }
        else if (team == Team.RED)
        {
            renderer.material = redMaterial;
        }
        else
        {
            renderer.material = defaultMaterial;
        }
    }
}
