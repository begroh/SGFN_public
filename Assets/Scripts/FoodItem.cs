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

	public bool isExploding = false;

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

	public void Explode() {
		isExploding = true;
		Invoke ("StopExploding", .5f);
	}

	private void StopExploding() {
		isExploding = false;
	}
}
