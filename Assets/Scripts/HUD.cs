using UnityEngine;
using System.Collections.Generic;

// Bagged means completed, required is things players are searching for
// Bonus will always have the same rendering, as they are outside the normal HUD
public enum HUDState { REQUIRED, CART, CONVEYOR, BAGGED, BONUS }

public class HUD : MonoBehaviour {

	public HUDIcon cheeseIcon;
	public HUDIcon breadIcon;
	public HUDIcon meatIcon;
	public HUDIcon condimentIcon;

	List<SpriteRenderer> bonusSprites; // TODO since we might not use them

	void Start()
	{
		GetSprites();
		SetStartColors();
		Invoke("ChangeStuff", 2);
	}

	void ChangeStuff()
	{
		DisplayItemInCart(FoodType.BREAD);
		DisplayItemInCart(FoodType.CHEESE);
		DisplayItemAsBagged(FoodType.CHEESE);
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
		HUDIcon icon = GetIconByType(type);
		Color tmp = icon.artwork.color;
		tmp.a = 0.5f;
		icon.artwork.color = tmp;
	}

	void DisplayItemInCart(FoodType type)
	{
		HUDIcon icon = GetIconByType(type);
		Color tmp = icon.artwork.color;
		tmp.a = 1f;
		icon.artwork.color = tmp;
	}

	void DisplayItemOnConveyor(FoodType type)
	{
		HUDIcon icon = GetIconByType(type);
		// TODO overlay sprite with something to signify conveyor
	}

	void DisplayItemAsBagged(FoodType type)
	{
		HUDIcon icon = GetIconByType(type);
		icon.bagged.enabled = true;
		// TODO overlay sprite with something to signify bagged
	}

	// TODO Decide if multiple bonus items can be added
	public void AddBonusItemSprite(FoodItem item)
	{

	}

	HUDIcon GetIconByType(FoodType type)
	{
		if (type == FoodType.CHEESE)
		{
			return cheeseIcon;
		}
		else if (type == FoodType.BREAD)
		{
			return breadIcon;
		}
		else if (type == FoodType.MEAT)
		{
			return meatIcon;
		}
		else if (type == FoodType.CONDIMENT)
		{
			return condimentIcon;
		}
		return null;
	}

	void GetSprites()
	{
		breadIcon = gameObject.transform.Find("BreadIcon").GetComponent<HUDIcon>();
		breadIcon.artwork.enabled = true;
		breadIcon.conveyor.enabled = false;
		breadIcon.bagged.enabled = false;

		cheeseIcon = gameObject.transform.Find("CheeseIcon").GetComponent<HUDIcon>();
		cheeseIcon.artwork.enabled = true;
		cheeseIcon.conveyor.enabled = false;
		cheeseIcon.bagged.enabled = false;

		meatIcon = gameObject.transform.Find("MeatIcon").GetComponent<HUDIcon>();
		meatIcon.artwork.enabled = true;
		meatIcon.conveyor.enabled = false;
		meatIcon.bagged.enabled = false;

		condimentIcon = gameObject.transform.Find("CondimentIcon").GetComponent<HUDIcon>();
		condimentIcon.artwork.enabled = true;
		condimentIcon.conveyor.enabled = false;
		condimentIcon.bagged.enabled = false;
	}

	void SetStartColors()
	{
		Color tmp = cheeseIcon.artwork.color;
		tmp.a = 0.5f;
		cheeseIcon.artwork.color = tmp;

		tmp = breadIcon.artwork.color;
		tmp.a = 0.5f;
		breadIcon.artwork.color = tmp;

		tmp = meatIcon.artwork.color;
		tmp.a = 0.5f;
		meatIcon.artwork.color = tmp;

		tmp = condimentIcon.artwork.color;
		tmp.a = 0.5f;
		condimentIcon.artwork.color = tmp;
	}
}
