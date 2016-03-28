using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrderHUD : MonoBehaviour {

	public Team team;

	private Dictionary<FoodType, Sprite> icons;
	Order currentOrder;

	Sprite cheeseSprite;
	Sprite breadSprite;
	Sprite meatSprite;
	Sprite toppingSprite;
	Sprite spreadSprite;
	Sprite pickleSprite;

	List<HUDIcon> iconObjects;

	void Start()
	{
		icons = new Dictionary<FoodType, Sprite>();

		GetSprites();
		icons[FoodType.CHEESE] = cheeseSprite;
		icons[FoodType.BREAD] = breadSprite;
		icons[FoodType.MEAT] = meatSprite;
		icons[FoodType.TOPPING] = toppingSprite;
		icons[FoodType.SPREAD] = spreadSprite;
		icons[FoodType.PICKLE] = pickleSprite;

		Refresh();
	}

	void GetSprites()
	{
		cheeseSprite = gameObject.transform.Find("CheeseSprite").GetComponent<SpriteRenderer>().sprite;
		breadSprite = gameObject.transform.Find("BreadSprite").GetComponent<SpriteRenderer>().sprite;
		meatSprite = gameObject.transform.Find("MeatSprite").GetComponent<SpriteRenderer>().sprite;
		toppingSprite = gameObject.transform.Find("ToppingSprite").GetComponent<SpriteRenderer>().sprite;
		spreadSprite = gameObject.transform.Find("SpreadSprite").GetComponent<SpriteRenderer>().sprite;
		pickleSprite = gameObject.transform.Find("PickleSprite").GetComponent<SpriteRenderer>().sprite;
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
	}
}
