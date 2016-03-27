using System.Collections.Generic;
using UnityEngine;

public class OrderManager {

    private static int[] numbers = {0, 1, 2, 3, 4, 5};
    
    public static Order GenerateOrder(int length = 4)
    {
        if (length > 6)
        {
            length = 6;
        }
        else if (length < 1)
        {
            length = 1;
        }

        Shuffle(numbers);
        List<FoodType> foods = new List<FoodType>();

        for (int i = 0; i < length; i++)
        {
            foods.Add((FoodType) numbers[i]);
        }

        return new Order(foods, (float) 4 * 10f, "I like turtles");
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

}
