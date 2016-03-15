using UnityEngine;
using System.Collections;

public enum FoodType { CHEESE, BREAD, MEAT, CONDIMENT, BONUS }

public class FoodItem : MonoBehaviour
{
    private FoodType _type;
    private Sprite _sprite;

    public FoodType type
    {
        get { return _type; }
    }

    public Sprite sprite
    {
        get { return _sprite; }
    }
}
