using System.Collections.Generic;
using UnityEngine;

public class OrderManager {

    private static int[] numbers = {0, 1, 2, 3, 4, 5, 6, 7};

    private static Dictionary<Team, Order> orders;

    public static Order OrderForTeam(Team team)
    {
        if (orders == null)
        {
            orders = new Dictionary<Team, Order>();
        }

        Order order;
        if (orders.TryGetValue(team, out order))
        {
            return order;
        }
        else
        {
            order = GenerateOrder(team);
            return order;
        }
    }
    
    public static Order GenerateOrder(Team team, int length = 4)
    {
        if (length > 8)
        {
            length = 8;
        }
        else if (length < 1)
        {
            length = 1;
        }

        if (orders == null)
        {
            orders = new Dictionary<Team, Order>();
        }

        Shuffle(numbers);
        List<FoodType> foods = new List<FoodType>();

        for (int i = 0; i < length; i++)
        {
            foods.Add((FoodType) numbers[i]);
        }

        Order order = new Order(foods, (float) 4 * 10f, "I like turtles");

        orders[team] = order;
        return order;
    }

    public static bool CompleteOrderForTeam(Team team)
    {
        ShoppingList list = ShoppingList.ForTeam(team);
        Order order = OrderManager.OrderForTeam(team);

        if (!order.IsOrderComplete(list))
        {
            return false;
        }

        Score.AddForTeam(team, order.Points);
        list.Reset();
        GenerateOrder(team, 4); // pick a length for the new order
        // Notify the HUD

        return true;
    }

    private static void Shuffle(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            int loc = Random.Range(i, numbers.Length);
            int temp = numbers[i];

            numbers[i] = numbers[loc];
            numbers[loc] = temp;
        }
    }

	/*
	 * Called by FoodItems to place an indicator that a team needs
	 * an item
	 */
	public static bool FoodItemInTeamOrder(Team team, FoodType type)
	{
		// Order order = orders[team];
		foreach(FoodType itemType in OrderManager.OrderForTeam(team).Items)
		{
			if (itemType == type)
			{
				return true;
			}
		}
		return false;
	}
}
