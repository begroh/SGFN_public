using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OrderHUD : MonoBehaviour {

	public Team team;

	private Dictionary<FoodType, Sprite> icons;
	Order currentOrder;

	Sprite cheeseSprite;
	Sprite breadSprite;
	Sprite meatSprite;
	Sprite milkSprite;
	Sprite fruitSprite;
	Sprite dessertSprite;
	Sprite vegetableSprite;
	Sprite mayoSprite;

	List<HUDIcon> iconObjects;

	Text scoreText;
	Image progressBar;

	void Start()
	{
		icons = new Dictionary<FoodType, Sprite>();

		GetSprites();
		icons[FoodType.CHEESE] = cheeseSprite;
		icons[FoodType.BREAD] = breadSprite;
		icons[FoodType.MEAT] = meatSprite;
		icons[FoodType.MILK] = milkSprite;
		icons[FoodType.FRUIT] = fruitSprite;
		icons[FoodType.DESSERT] = dessertSprite;

		icons[FoodType.VEGETABLE] = vegetableSprite;
		icons[FoodType.MAYO] = mayoSprite;

		scoreText = gameObject.transform.Find("Score").GetComponent<Text>();

		progressBar = gameObject.transform.Find("Progress").GetComponent<Image>();
		progressBar.color = team == Team.BLUE ? Color.blue : Color.red;

		Refresh();
	}

	void GetSprites()
	{
		cheeseSprite = gameObject.transform.Find("CheeseSprite").GetComponent<SpriteRenderer>().sprite;
		breadSprite = gameObject.transform.Find("BreadSprite").GetComponent<SpriteRenderer>().sprite;
		meatSprite = gameObject.transform.Find("MeatSprite").GetComponent<SpriteRenderer>().sprite;
		milkSprite = gameObject.transform.Find("MilkSprite").GetComponent<SpriteRenderer>().sprite;
		fruitSprite = gameObject.transform.Find("FruitSprite").GetComponent<SpriteRenderer>().sprite;
		dessertSprite = gameObject.transform.Find("DessertSprite").GetComponent<SpriteRenderer>().sprite;
		vegetableSprite = gameObject.transform.Find("VegetableSprite").GetComponent<SpriteRenderer>().sprite;
		mayoSprite = gameObject.transform.Find("MayoSprite").GetComponent<SpriteRenderer>().sprite;
	}

	public void ReceiveOrder(Order order)
	{
		ClearOrder();

		HUDIcon[] orderIcons = GetComponentsInChildren<HUDIcon>();
		ShoppingList list = ShoppingList.ForTeam(team);
		// Loop through order and set the sprites on the HUD to match it
		int i = 0;
		foreach (FoodType type in order.Items)
		{
			orderIcons[i].SetSprite(icons[order.Items[i]]);
			if (list.GetState(type) == FoodState.BAGGED)
			{
				print (list.GetState(type));
				orderIcons[i].SetBagged();
			}
			i++;
		}
	}

	void ClearOrder()
	{
		HUDIcon[] currentIcons = GetComponentsInChildren<HUDIcon>();
		foreach (HUDIcon i in currentIcons)
		{
			i.ClearSprite();
		}
	}

	public void Refresh()
	{
		Order order = OrderManager.OrderForTeam(team);
		ReceiveOrder(order);
		scoreText.text = Score.ForTeam(team).ToString();
		progressBar.fillAmount = Score.ForTeam(team) / 150f;
	}
}
