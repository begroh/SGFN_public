using UnityEngine;
using System.Collections.Generic;

public class ShoppingCart : MonoBehaviour
{
    private Queue<FoodItem> cart;

    public int Count
    {
        get { return cart.Count; }
    }

    public void Add(FoodItem item)
    {
        cart.Enqueue(item);
        // TODO add item to HUD
    }

    public FoodItem Remove()
    {
        if (cart.Count > 0)
        {
            // TODO remove from HUD
            return cart.Dequeue();
        }

        return null;
    }
}
