using System.Collections.Generic;
using UnityEngine;

/**
 * ShoppingList list = ShoppingList.ForTeam(Team.RED);
 *
 * list.GetState(FoodType.BREAD);
 *      => FoodState.ON_GROUND
 *
 * list.SetState(FoodType.BREAD, FoodState.BAGGED);
 *
 * list.GetState(FoodType.BREAD);
 *      => FoodState.BAGGED
 */
public class ShoppingList {

    private Dictionary<FoodType, FoodState> types;

    private static Dictionary<Team, ShoppingList> lists;

    private ShoppingList()
    {
        types = new Dictionary<FoodType, FoodState>();
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

    public FoodState GetState(FoodType type)
    {
        FoodState state;
        if (types.TryGetValue(type, out state))
        {
            return state;
        }
        else
        {
            return FoodState.ON_GROUND;
        }
    }

    public void SetState(FoodType type, FoodState state)
    {
        types[type] = state;
    }

    public void Reset()
    {
        foreach (FoodType type in FoodType.GetValues(typeof(FoodType)))
        {
            types[type] = FoodState.ON_GROUND;
            Debug.Log(types[type]);
        }
    }
}
