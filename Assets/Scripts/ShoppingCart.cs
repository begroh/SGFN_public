using UnityEngine;
using System.Collections.Generic;

public class ShoppingCart : MonoBehaviour
{
	public float reloadTime;
	public float launchForce;

	private Stack<FoodItem> cart;
	private float lastFireTime;

	void Start()
    {
        this.cart = new Stack<FoodItem>();
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

        cart.Push(item);
		gameObject.GetComponent<Collider2D> ().enabled = true;

		// Move the fooditem to the player's possession
		item.transform.position = gameObject.transform.position;
		item.transform.parent = gameObject.transform;
		item.GetComponent<Rigidbody2D>().isKinematic = true;
		item.GetComponent<Collider2D> ().enabled = false;

		UpdateFoodPositions ();

        return true;
    }

    public FoodItem Remove()
    {
        if (cart.Count > 0)
        {
			if (cart.Count == 1) {
				gameObject.GetComponent<Collider2D> ().enabled = false;
			}
			return cart.Pop();
        }

        return null;
    }

	public void dropAllItems() {
		int numItems = cart.Count;
		for (int i = 0; i < numItems; ++i) {
			FireFoodItem (Quaternion.AngleAxis (360f/numItems * i + 45f, Vector3.forward) * Vector3.right);
		}
	}

	public FoodItem Fire() {
		FoodItem item = null;

		if (lastFireTime + reloadTime < Time.time)
		{
			item = FireFoodItem (transform.right);
			if (item) {
				lastFireTime = Time.time;
			}
		}

		return item;
	}

	private FoodItem FireFoodItem(Vector2 forceDirection)
	{
		FoodItem item = Remove ();

		if (item) {
			item.transform.position = item.transform.parent.position + item.transform.parent.transform.right;
			item.transform.parent = null;
			Rigidbody2D body = item.GetComponent<Rigidbody2D>();
			body.isKinematic = false;
			body.AddForce(forceDirection * launchForce);
			item.GetComponent<Collider2D> ().enabled = true;
			UpdateFoodPositions ();
		}

		return item;
	}

	// TODO write this function
	public void UpdateFoodPositions() {
        if (cart.Count < 1)
        {
            return;
        }

		if (cart.Count == 1) {
			cart.Peek ().transform.position = transform.position;
			return;
		}

		float radius = 1f;
        float angle = 270.0f / cart.Count;

        float i = 0;
        //for (int i = 0; i < cart.Count; ++i)
        foreach (FoodItem item in cart)
        {
            ++i;
            Vector3 offset = Vector3.right * radius;
            Quaternion rotation = Quaternion.Euler(0, 0, angle * i + 200f);
            rotation = rotation * transform.rotation;

            Vector3 target = transform.position + (rotation * offset);
            Vector3 current = item.transform.position;

            //Debug.Log("rotating item " + i + " by " + angle);

            item.transform.position = Vector3.MoveTowards(current, target, 0.1f);
        }
	}

}
