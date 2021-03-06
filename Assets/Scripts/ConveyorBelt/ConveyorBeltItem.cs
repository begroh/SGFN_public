using UnityEngine;

public interface ConveyorBeltItem
{
    /*
     * The size of the item as it is represented on the conveyor belt
     */
    float Size();

    /*
     * The position of the conveyor belt item
     */
    Vector2 Position();

    /*
     * Move the conveyor belt by the vector
     */
    void Move(Vector2 move);

    /*
     * Return the FoodItem represented by this ConveyorBeltItem
     */
    FoodItem AsFoodItem();

    Player player { get; set; }
}
