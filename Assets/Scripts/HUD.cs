using UnityEngine;
using System.Collections.Generic;

// Bagged means completed, required is things players are searching for
// Bonus will always have the same rendering, as they are outside the normal HUD
public enum HUDState { REQUIRED, CART, CONVEYOR, BAGGED, BONUS }

public class HUD : MonoBehaviour {

	public SpriteRenderer cheeseSprite;
	public SpriteRenderer breadSprite;
	public SpriteRenderer meatSprite;
	public SpriteRenderer condimentSprite;

	public SpriteRenderer conveyorSprite;
	public SpriteRenderer baggedSprite;

	List<SpriteRenderer> bonusSprites; // TODO since we might not use them

	void Start()
	{
		GetSprites();
		SetStartColors();
	}

	public void OnItemStateChanged(FoodItem item, HUDState state)
	// public void OnItemStateChanged(Dictionary<FoodItem, FoodState> foodStates)
	{
		if (state == HUDState.REQUIRED)
		{
			DisplayItemAsRequired(item.type);
		}
		else if (state == HUDState.CART)
		{
			DisplayItemInCart(item.type);
		}
		else if (state == HUDState.CONVEYOR)
		{
			DisplayItemOnConveyor(item.type);
		}
		else if (state == HUDState.BAGGED)
		{
			DisplayItemAsBagged(item.type);
		}
	}

	void DisplayItemAsRequired(FoodType type)
	{
		SpriteRenderer sprend = GetSpriteByType(type);
		Color tmp = sprend.color;
		tmp.a = 0.5f;
		sprend.color = tmp;
	}

	void DisplayItemInCart(FoodType type)
	{
		SpriteRenderer sprend = GetSpriteByType(type);
		Color tmp = sprend.color;
		tmp.a = 1f;
		sprend.color = tmp;
	}

	void DisplayItemOnConveyor(FoodType type)
	{
		SpriteRenderer sprend = GetSpriteByType(type);
		// TODO overlay sprite with something to signify conveyor
	}

	void DisplayItemAsBagged(FoodType type)
	{
		SpriteRenderer sprend = GetSpriteByType(type);
		// TODO overlay sprite with something to signify bagged
	}

	// TODO Decide if multiple bonus items can be added
	public void AddBonusItemSprite(FoodItem item)
	{

	}

	SpriteRenderer GetSpriteByType(FoodType type)
	{
		if (type == FoodType.CHEESE)
		{
			return cheeseSprite;
		}
		else if (type == FoodType.BREAD)
		{
			return breadSprite;
		}
		else if (type == FoodType.MEAT)
		{
			return meatSprite;
		}
		else if (type == FoodType.CONDIMENT)
		{
			return condimentSprite;
		}
		return null;
	}

	void GetSprites()
	{
		GameObject child = gameObject.transform.Find("BreadIcon").gameObject;
		breadSprite = child.GetComponent<SpriteRenderer>();
		breadSprite.enabled = true;

		child = gameObject.transform.Find("CheeseIcon").gameObject;
		cheeseSprite = child.GetComponent<SpriteRenderer>();
		cheeseSprite.enabled = true;

		child = gameObject.transform.Find("MeatIcon").gameObject;
		meatSprite = child.GetComponent<SpriteRenderer>();
		meatSprite.enabled = true;

		child = gameObject.transform.Find("CondimentIcon").gameObject;
		condimentSprite = child.GetComponent<SpriteRenderer>();
		condimentSprite.enabled = true;

		// Conveyor and Bagged sprites start out disabled
		child = gameObject.transform.Find("ConveyorIcon").gameObject;
		conveyorSprite = child.GetComponent<SpriteRenderer>();
		child = gameObject.transform.Find("BaggedIcon").gameObject;
		baggedSprite = child.GetComponent<SpriteRenderer>();
	}

	void SetStartColors()
	{
		Color tmp = cheeseSprite.color;
		tmp.a = 0.5f;
		cheeseSprite.color = tmp;

		tmp = breadSprite.color;
		tmp.a = 0.5f;
		breadSprite.color = tmp;

		tmp = meatSprite.color;
		tmp.a = 0.5f;
		meatSprite.color = tmp;

		tmp = condimentSprite.color;
		tmp.a = 0.5f;
		condimentSprite.color = tmp;
	}
}
