using UnityEngine;
using System.Collections;

public enum FoodType { CHEESE, BREAD, MEAT, CONDIMENT, TOPPING }

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
}
