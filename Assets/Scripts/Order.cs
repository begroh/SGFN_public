using System.Collections.Generic;

public class Order {

    private List<FoodType> items;
    private float points;
    private string name;

    public Order(List<FoodType> items, float points, string name)
    {
        this.items = items;
        this.points = points;
        this.name = name;
    }

    public List<FoodType> Items
    {
        get { return items; }
    }

    public float Points
    {
        get { return points; }
    }

    public string Name
    {
        get { return name; }
    }
}
