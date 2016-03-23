using UnityEngine;
using System.Collections.Generic;

public class ShoppingCart : MonoBehaviour
{
	public float reloadTime;
	public float launchForce;

	private Queue<FoodItem> cart;
	private float lastFireTime;

    public ShoppingCart()
    {
        this.cart = new Queue<FoodItem>();
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
		item.GetComponent<Rigidbody2D>().isKinematic = true;
		item.transform.parent = gameObject.transform;
		item.transform.position = gameObject.transform.position;
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

	public void Fire() {
		if (lastFireTime + reloadTime < Time.time)
		{
			FireFoodItem();
			lastFireTime = Time.time;
		}
	}

	private void FireFoodItem()
	{
		FoodItem item = Remove ();

		if (item) {
			item.transform.position = item.transform.parent.position;
			item.transform.parent = null;
			Rigidbody2D body = item.GetComponent<Rigidbody2D>();
			body.isKinematic = false;
			body.AddForce(transform.right * launchForce);
			UpdateFoodPositions ();
		}
			
	}

	// TODO write this function
	private void UpdateFoodPositions() {

	}

}
