using UnityEngine;
using System.Collections.Generic;

public class ShoppingCart : MonoBehaviour
{
    private Queue<FoodItem> cart;

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

	// public bool ContainsFullSandwich()
	// {
	// 	bool hasCheese = false, hasBread = false,
	// 		 hasCondiment = false, hasMeat = false;
	// 	foreach (FoodItem item in cart)
	// 	{
	// 		if (item.type == FoodType.CHEESE)
	// 			hasCheese = true;
	// 		else if (item.type == FoodType.BREAD)
	// 			hasBread = true;
	// 		else if (item.type == FoodType.CONDIMENT)
	// 			hasCondiment = true;
	// 		else if (item.type == FoodType.MEAT)
	// 			hasMeat = true;

	// 		return hasCheese && hasBread && hasCondiment && hasMeat;
	// 	}
	// }
}
