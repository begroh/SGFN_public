using System.Collections.Generic;

/**
 * ShoppingList list = ShoppingList.ForTeam(Team.RED);
 *
 * list.GetState(FoodItem.BREAD);
 *      => FoodState.ON_GROUND
 *
 * list.SetState(FoodItem.BREAD, FoodState.BAGGED);
 *
 * list.GetState(FoodItem.BREAD);
 *      => FoodState.BAGGED
 */
public class ShoppingList {

    private Dictionary<FoodItem, FoodState> items;

    private static Dictionary<Team, ShoppingList> lists;

    private ShoppingList()
    {
        items = new Dictionary<FoodItem, FoodState>();
    }

    public static ShoppingList ForTeam(Team team)
    {
        if (lists == null)
        {
            lists = new Dictionary<Team, ShoppingList>();
        }

        ShoppingList list;
        if (lists.TryGetValue(team, out list))
        {
            return list;
        }
        else
        {
            list = new ShoppingList();
            lists[team] = list;
            return list;
        }
    }

    public FoodState GetState(FoodItem item)
    {
        FoodState state;
        if (items.TryGetValue(item, out state))
        {
            return state;
        }
        else
        {
            return FoodState.ON_GROUND;
        }
    }

    public void SetState(FoodItem item, FoodState state)
    {
        items[item] = state;
    }
}
