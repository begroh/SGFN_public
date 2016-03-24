using UnityEngine;
using System.Collections.Generic;

public class ShoppingCart : MonoBehaviour
{
	public float reloadTime;
	public float launchForce;

	private Queue<FoodItem> cart;
	private Player player;
	private float lastFireTime;

	void Start()
    {
        this.cart = new Queue<FoodItem>();
		this.player = (Player)gameObject.transform.parent.gameObject.GetComponent<Player>();
    }

    public int Count
    {
        get { return cart.Count; }
    }

    public bool Add(FoodItem item)
    {
        if (cart.Contains(item))
        {
            return false;
        }

        cart.Enqueue(item);

		// Move the fooditem to the player's possession
		print ("YEAH");
		Physics2D.IgnoreCollision (gameObject.GetComponent<Collider2D>(), player.gameObject.GetComponent<Collider2D> (), true);
		item.transform.position = gameObject.transform.position;
		item.transform.parent = gameObject.transform;
		item.GetComponent<Rigidbody2D>().isKinematic = true;

		UpdateFoodPositions ();

        return true;
    }

    public FoodItem Remove()
    {
        if (cart.Count > 0)
        {
            return cart.Dequeue();
        }

        return null;
    }

	public FoodItem Fire() {
		FoodItem item = null;

		if (lastFireTime + reloadTime < Time.time)
		{
			item = FireFoodItem ();
			if (item) {
				lastFireTime = Time.time;
			}
		}

		return item;
	}

	private FoodItem FireFoodItem()
	{
		FoodItem item = Remove ();

		if (item) {
			item.transform.position = item.transform.parent.position;
			item.transform.parent = null;
			Rigidbody2D body = item.GetComponent<Rigidbody2D>();
			body.isKinematic = false;
			body.AddForce(transform.right * launchForce);
			Physics2D.IgnoreCollision (item.GetComponent<Collider2D>(), player.gameObject.GetComponent<Collider2D> (), false);
			UpdateFoodPositions ();
		}

		return item;
	}

	// TODO write this function
	private void UpdateFoodPositions() {

	}

}
