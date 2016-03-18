using UnityEngine;

public class FoodConveyorBeltItem : MonoBehaviour, ConveyorBeltItem
{
    private FoodItem item;
	private Player _player;

    public void SetItem(FoodItem item)
    {
        this.item = item;
    }

    public FoodItem Item()
    {
        return item;
    }

	public Player player
	{
		get { return _player; }
		set { _player = value; }
	}

    /*
     * The size of the item as it is represented on the conveyor belt
     */
    public float Size()
    {
        return 1.25f;
    }

    /*
     * The position of the conveyor belt item
     */
    public Vector2 Position()
    {
        return this.transform.position;
    }

    /*
     * Move the conveyor belt by the vector
     */
    public void Move(Vector2 move)
    {
        this.transform.position = (Vector3) move;
    }
}
