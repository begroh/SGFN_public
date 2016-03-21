using UnityEngine;
using System.Collections;

public enum FoodType { CHEESE, BREAD, MEAT, TOPPING, BONUS }

public class FoodItem : MonoBehaviour
{
    public FoodType _type;
    private string _name;
    private Sprite _sprite;

    public FoodType type
    {
        get { return _type; }
    }

    public Sprite sprite
    {
        get { return _sprite; }
    }

    public string name
    {
        get { return _name; }
    }

    public static bool operator == (FoodItem a, FoodItem b)
    {
		print ("comparing");
        return a._type == b._type;
    }

    public static bool operator != (FoodItem a, FoodItem b)
    {
        return a._type != b._type;
    }
}
