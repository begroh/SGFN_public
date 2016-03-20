using UnityEngine;
using System.Collections;

public class HUDIcon : MonoBehaviour
{
	public SpriteRenderer artwork;
	public SpriteRenderer conveyor;
	public SpriteRenderer bagged;

	void Awake()
	{
		GameObject child = gameObject.transform.Find("FoodIcon").gameObject;
		artwork = child.GetComponent<SpriteRenderer>();
		// artwork.enabled = true;

		child = gameObject.transform.Find("ConveyorIcon").gameObject;
		conveyor = child.GetComponent<SpriteRenderer>();
		// conveyor.enabled = false;

		child = gameObject.transform.Find("BaggedIcon").gameObject;
		bagged = child.GetComponent<SpriteRenderer>();
		// bagged.enabled = false;
	}

	public void SetSprite(Sprite art)
	{
		artwork.sprite = art;
	}

	public void SetConveyor(Sprite conv)
	{
		conveyor.sprite = conv;
	}

	public void SetBagged(Sprite bag)
	{
		bagged.sprite = bag;
	}
}
