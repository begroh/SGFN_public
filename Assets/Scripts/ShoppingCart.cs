using UnityEngine;
using System.Collections.Generic;

public class ShoppingCart : MonoBehaviour
{
	private float reloadTime = .1f;
	private float launchForce = 9000f;

	private Stack<FoodItem> cart;
	private Stack<FoodItem> extras;
	private float lastFireTime;

	void Start()
    {
        this.cart = new Stack<FoodItem>();
		this.extras = new Stack<FoodItem> ();
    }

    public int Count
    {
		get { return cart.Count + extras.Count; }
    }

    public bool Add(FoodItem item)
    {
		if (item.type == FoodType.EXTRA) {
			extras.Push (item);
		} else {
			if (cart.Contains(item)) {
				return false;
			}

			cart.Push(item);
		}

		// Move the fooditem to the player's possession
		item.transform.position = gameObject.transform.position;
		item.transform.parent = gameObject.transform;
		item.GetComponent<Rigidbody2D>().isKinematic = true;
		item.GetComponent<Collider2D> ().enabled = false;

		UpdateFoodPositions ();

        return true;
    }

	public FoodItem Remove(bool extraFirst)
    {
		if (extraFirst) {
			if (extras.Count > 0) {
				return extras.Pop ();
			}
		}

		if (cart.Count > 0)
        {
			return cart.Pop();
        }

        return null;
    }

	public void dropAllItems() {
		int numItems = cart.Count + extras.Count;
		for (int i = 0; i < numItems; ++i) {
			FoodItem item = FireFoodItem (Quaternion.AngleAxis (360f/numItems * i + 45f, Vector3.forward) * Vector3.right, launchForce/2);
		}
	}

	public FoodItem Fire() {
		FoodItem item = null;

		if (lastFireTime + reloadTime < Time.time)
		{
			item = FireFoodItem (transform.right, launchForce);
			if (item) {
				lastFireTime = Time.time;
			}
		}

		return item;
	}

	private FoodItem FireFoodItem(Vector2 forceDirection, float force)
	{
		FoodItem item = Remove (true);

		if (item) {
            item.hitBehaviour.NotifyDropped();
			item.transform.position = item.transform.parent.position + item.transform.parent.transform.right;
			item.transform.parent = null;

			Rigidbody2D body = item.GetComponent<Rigidbody2D>();
			body.isKinematic = false;
			body.AddForce(forceDirection * force);

			item.GetComponent<Collider2D> ().enabled = true;
			UpdateFoodPositions ();

			/*
			 * TODO Need to figure this part out 
			 */
			item.canKill = true;
			item.StopCanKill ();

			// Check to see if it's a potato and set it to be destroyed
			DegradingCollectible collectible = item.gameObject.GetComponent<DegradingCollectible> ();
			if (collectible != null) {
				collectible.destroy = true;
			}
		}

		return item;
	}

	public void UpdateFoodPositions() {
		if (cart.Count < 1 && extras.Count < 1)
        {
            return;
        }

		// If only cart
		if (cart.Count == 1 && extras.Count == 0) {
			cart.Peek ().transform.position = transform.position;
			return;
		}

		// If only extras
		if (extras.Count > 0 && cart.Count == 0) {
			foreach (FoodItem item in extras) {
				item.transform.position = transform.position;
			}
			return;
		}

		float extrasBool = extras.Count > 0 ? 1 : 0;
		float radius = 1f;
		float angle = 270.0f / (cart.Count + extrasBool);

        float i = 0;
        foreach (FoodItem item in cart)
        {
            
            Vector3 offset = Vector3.right * radius;
            Quaternion rotation = Quaternion.Euler(0, 0, angle * i + 270f);
            rotation = rotation * transform.rotation;

            Vector3 target = transform.position + (rotation * offset);
            Vector3 current = item.transform.position;

            item.transform.position = Vector3.MoveTowards(current, target, 0.1f);
			++i;
        }

		// Position all of the extras in the same place
		foreach (FoodItem item in extras) {
			Vector3 offset = Vector3.right * radius;
			Quaternion rotation = Quaternion.Euler(0, 0, angle * i + 270f);
			rotation = rotation * transform.rotation;

			Vector3 target = transform.position + (rotation * offset);
			Vector3 current = item.transform.position;

			item.transform.position = Vector3.MoveTowards(current, target, 0.1f);
		}
	}

	public bool HasFoodType(FoodType type)
	{
		foreach(FoodItem item in cart)
		{
			if (item.type == type)
			{
				return true;
			}
		}
		return false;
	}

	public static bool FoodItemInCart(FoodType type)
	{
		foreach(ShoppingCart c in Object.FindObjectsOfType(typeof(ShoppingCart)))
		{
			if (c.HasFoodType(type))
				return true;
		}
		return false;
	}
}
