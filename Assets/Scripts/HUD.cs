﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Bagged means completed, required is things players are searching for
// Bonus will always have the same rendering, as they are outside the normal HUD
public enum HUDState { REQUIRED, CART, CONVEYOR, BAGGED, BONUS }

public class HUD : MonoBehaviour {

	public HUDIcon cheeseIcon;
	public HUDIcon breadIcon;
	public HUDIcon meatIcon;
	public HUDIcon condimentIcon;

	int numSandwiches = 0;
	public Text sandwichCount;

	List<SpriteRenderer> bonusSprites; // TODO since we might not use them

	void Start()
	{
		GetSprites();
		SetStartColors();
		sandwichCount = gameObject.GetComponentInChildren<Text>();
		sandwichCount.text = "x 0";
	}


	// public void OnItemStateChanged(Dictionary<FoodType, FoodState> foodStates)
	public void OnItemStateChanged(FoodType type, FoodState state)
	{
		// Change display of any items in the HUD that have changed state
		if (state == FoodState.ON_GROUND)
		{
			DisplayItemAsRequired(type);
		}
		else if (state == FoodState.IN_CART)
		{
			DisplayItemInCart(type);
		}
		else if (state == FoodState.ON_CONVEYOR)
		{
			DisplayItemOnConveyor(type);
		}
		else if (state == FoodState.BAGGED)
		{
			DisplayItemAsBagged(type);
		}
		// TODO Bonus
	}


	public void IncrementSandwiches()
	{
		numSandwiches++;
		sandwichCount.text = "x " + numSandwiches;
		ResetHUD();
	}

	void DisplayItemAsRequired(FoodType type)
	{
		HUDIcon icon = GetIconByType(type);
		Color tmp = icon.artwork.color;
		tmp.a = 0.5f;
		icon.artwork.color = tmp;
		//icon.conveyor.enabled = false;
		icon.bagged.enabled = false;
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
		// HUDIcon icon = GetIconByType(type);
		// TODO overlay sprite with something to signify conveyor
	}

	void DisplayItemAsBagged(FoodType type)
	{
		HUDIcon icon = GetIconByType(type);
		icon.bagged.enabled = true;
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
		// else if (type == FoodType.TOPPING)
		// {
		// 	return condimentIcon;
		// }
		return null;
	}

	void GetSprites()
	{
		breadIcon = gameObject.transform.Find("BreadIcon").GetComponent<HUDIcon>();
		breadIcon.artwork.enabled = true;
		breadIcon.bagged.enabled = false;

		cheeseIcon = gameObject.transform.Find("CheeseIcon").GetComponent<HUDIcon>();
		cheeseIcon.artwork.enabled = true;
		cheeseIcon.bagged.enabled = false;

		meatIcon = gameObject.transform.Find("MeatIcon").GetComponent<HUDIcon>();
		meatIcon.artwork.enabled = true;
		meatIcon.bagged.enabled = false;

		condimentIcon = gameObject.transform.Find("ToppingIcon").GetComponent<HUDIcon>();
		condimentIcon.artwork.enabled = true;
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

	public void ResetHUD()
	{
		DisplayItemAsRequired(FoodType.BREAD);
		DisplayItemAsRequired(FoodType.CHEESE);
		DisplayItemAsRequired(FoodType.MEAT);
		// DisplayItemAsRequired(FoodType.TOPPING);
	}
}
