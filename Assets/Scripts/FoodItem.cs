using UnityEngine;
using System.Collections;

public enum FoodType { CHEESE, BREAD, MEAT, MILK, FRUIT, DESSERT, POTATO, MAYO, EXTRA }

public class FoodItem : MonoBehaviour, ConveyorBeltItem
{
    public FoodType _type;
    private string _name;
    private Sprite _sprite;
    private float size;
    private Player _player;

	public SpriteRenderer redIndicator, blueIndicator;

    void Awake()
    {
        // Set the size to be the size of the convex box of the sprite
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Vector3 size = sprite.bounds.size;
        this.size = Mathf.Max(size.x, size.y);
    }

    public FoodType type
    {
        get { return _type; }
    }

    // public Sprite sprite
    // {
    //     get { return _sprite; }
    // }

    // public string name
    // {
    //     get { return _name; }
    // }

    public static bool operator == (FoodItem a, FoodItem b)
    {
        return a._type == b._type;
    }

    public static bool operator != (FoodItem a, FoodItem b)
    {
        return a._type != b._type;
    }

    public float Size()
    {
        return size;
    }

    public Vector2 Position()
    {
        return transform.position;
    }

    public void Move(Vector2 move)
    {
        this.transform.position = (Vector3) move;
    }

    public FoodItem AsFoodItem()
    {
        return this;
    }

    public Player player {
        get { return _player; }
        set { _player = value; }
    }

	void SetIndicators()
	{
		bool inRedOrder = OrderManager.FoodItemInTeamOrder(Team.RED, _type);
		bool inBlueOrder = OrderManager.FoodItemInTeamOrder(Team.BLUE, _type);

		Quaternion leftRot = Quaternion.Euler(0, 0, 90);
		Quaternion topRot = Quaternion.Euler(0, 0, 180);
		Quaternion rightRot = Quaternion.Euler(0, 0, 270);
		if (inRedOrder && inBlueOrder)
		{
			redIndicator.gameObject.SetActive(true);
			blueIndicator.gameObject.SetActive(true);
			redIndicator.transform.rotation = leftRot;
			redIndicator.transform.position = new Vector3(-1, 2, 0);
			blueIndicator.transform.position = new Vector3(1, 2, 0);
			blueIndicator.transform.rotation = rightRot;
		}
		else if (inRedOrder)
		{
			blueIndicator.gameObject.SetActive(false);
			redIndicator.gameObject.SetActive(true);
			redIndicator.transform.localPosition = new Vector3(0, 2, 0);
			redIndicator.transform.rotation = topRot;
		}
		else if (inBlueOrder)
		{
			redIndicator.gameObject.SetActive(false);
			blueIndicator.gameObject.SetActive(true);
			blueIndicator.transform.localPosition = new Vector3(0, 2, 0);
			blueIndicator.transform.rotation = topRot;
		}
		else
		{
			redIndicator.gameObject.SetActive(false);
			blueIndicator.gameObject.SetActive(false);
		}
	}
}
