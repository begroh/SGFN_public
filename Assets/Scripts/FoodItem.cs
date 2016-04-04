using UnityEngine;
using System.Collections;

public enum FoodType { CHEESE, BREAD, MEAT, MILK, FRUIT, DESSERT, VEGETABLE, MAYO, EXTRA }

public class FoodItem : MonoBehaviour, ConveyorBeltItem
{
    public FoodType _type;
    private string _name;
    private Sprite _sprite;
    private float size;
    private Player _player;
    private Rigidbody2D body;
    private float chargeVelocity = 5.0f;

	public SpriteRenderer redIndicator, blueIndicator;
    public bool canKill = false;
	public bool isExploding = false;

    void Awake()
    {
        // Set the size to be the size of the convex box of the sprite
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Vector3 size = sprite.bounds.size;
        this.size = Mathf.Max(size.x, size.y);

		if (_type != FoodType.EXTRA)
		{
			redIndicator = transform.Find("RedIndicator").GetComponent<SpriteRenderer>();
			redIndicator.color = Color.red;
			blueIndicator = transform.Find("BlueIndicator").GetComponent<SpriteRenderer>();
			blueIndicator.color = Color.blue;
		}
	}

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (body.velocity.magnitude < chargeVelocity)
        {
            canKill = false;
        }
		if (_type != FoodType.EXTRA)
			SetIndicators();
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
		bool inRedOrder = OrderManager.FoodItemInTeamOrder(Team.RED, _type)
			&& FoodItemNotBagged(Team.RED);
		bool inBlueOrder = OrderManager.FoodItemInTeamOrder(Team.BLUE, _type)
			&& FoodItemNotBagged(Team.BLUE);

		if (ShoppingCart.FoodItemInCart(_type))
		{
			redIndicator.gameObject.SetActive(false);
			blueIndicator.gameObject.SetActive(false);
			return;
		}

		Quaternion leftRot = Quaternion.Euler(0, 0, 70);
		Quaternion topRot = Quaternion.Euler(0, 0, 180);
		Quaternion rightRot = Quaternion.Euler(0, 0, 290);
		if (inRedOrder && inBlueOrder)
		{
			redIndicator.gameObject.SetActive(true);
			blueIndicator.gameObject.SetActive(true);
			redIndicator.transform.rotation = leftRot;
			redIndicator.transform.position = transform.position + 2*Vector3.up - 0.75f*Vector3.right;
			blueIndicator.transform.position = transform.position + 2*Vector3.up + 0.75f*Vector3.right;
			blueIndicator.transform.rotation = rightRot;
		}
		else if (inRedOrder)
		{
			blueIndicator.gameObject.SetActive(false);
			redIndicator.gameObject.SetActive(true);
			redIndicator.transform.position = transform.position + 2*Vector3.up;
			redIndicator.transform.rotation = topRot;
		}
		else if (inBlueOrder)
		{
			redIndicator.gameObject.SetActive(false);
			blueIndicator.gameObject.SetActive(true);
			blueIndicator.transform.position = transform.position + 2*Vector3.up;
			blueIndicator.transform.rotation = topRot;
		}
		else
		{
			redIndicator.gameObject.SetActive(false);
			blueIndicator.gameObject.SetActive(false);
		}
	}

	bool FoodItemNotBagged(Team team)
	{
		return ShoppingList.ForTeam(team).GetState(_type) != FoodState.BAGGED;
	}

	public void Explode() {
		isExploding = true;
		Invoke ("StopExploding", .5f);
	}

	private void StopExploding() {
		isExploding = false;
	}

	public bool StopCanKill()
	{
		return false;
	}
}
