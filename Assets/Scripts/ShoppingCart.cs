using System.Collections.Generic;

public class ShoppingCart
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
}
