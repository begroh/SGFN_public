using UnityEngine;
using System.Collections;

public class HUDIcon : MonoBehaviour
{
	public SpriteRenderer artwork;
	public SpriteRenderer conveyor;
	public SpriteRenderer bagged;

	void Start()
	{
		GameObject child = gameObject.transform.Find("FoodIcon").gameObject;
		artwork = child.GetComponent<SpriteRenderer>();
		artwork.enabled = true;
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
