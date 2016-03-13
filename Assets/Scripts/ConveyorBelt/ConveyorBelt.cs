using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    private enum Direction { FORWARD, REVERSE };

    private float speed = 0.1;      // 10 cm / second
    private float marign = 0.05;    // 5 cm spacing
    private LinkedList<ConveyorBeltItem> items;
    private Vector2 startPosition;
    private Vector2 endPosition;

    void Awake()
    {
        items = new LinkedList<ConveyorBeltItem>();
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
     * Attempt to take an item from the conveyor belt.
     * Returns null if there is no available item.
     */
    public ConveyorBeltItem TakeItem()
    {
        ConveyorBeltItem item = AvailableItem();

        if (item != null)
        {
            items.Remove(item);
        }
        else
        {
            return null;
        }
    }

    /*
     * Attempt to add an item to the conveyor belt.
     * Returns whether or not there was room to add the item.
     */
    public bool DepositItem(ConveyorBeltItem item)
    {
        if (HasRoom())
        {
            items.Add(item);
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
        return false;
    }

    /*
     * Returns true if an enemy player is near the conveyor belt.
     */
    private bool EnemyPlayer()
    {
        return false;
    }

    /*
     * If direction is FORWARD, move the conveyor belt items toward endPosition,
     * if the direction is REVERSE, move the conveyor belt items away from endPosition.
     */
    private void Run(Direction direction)
    {
        if (direction == Direction.REVERSE && AvailableItem() != null)
        {
            // An item is already waiting at the start of the belt, cannot reverse any more.
            return;
        }

        float maxDistance = Time.deltaTime * speed;
        if (direction == Direction.REVERSE)
        {
            maxDistance *= -1;
        }

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
        ConveyorBeltItem item = items.Last();

        if (Vector2.Distance(item.Position(), startPosition) < 0.01)
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
    private bool HasRoom()
    {
        ConveyorBeltItem last = AvailableItem();

        if (last == null)
        {
            return true;
        }
        else
        {
            Vector2 position = last.Position();
            return (Vector2.Distance(position, startPosition) > (margin + item.Size() / 2.0f);
        }
    }

    private void MoveItemToBag(ConveyorBeltItem item)
    {

    }
}
