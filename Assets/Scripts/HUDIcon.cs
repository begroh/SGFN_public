using UnityEngine;
using System.Collections;

public class HUDIcon : MonoBehaviour
{
	public SpriteRenderer artwork;
	public SpriteRenderer bagged;

	void Awake()
	{
		GameObject child = gameObject.transform.Find("FoodIcon").gameObject;
		artwork = child.GetComponent<SpriteRenderer>();
		artwork.enabled = false;

		child = gameObject.transform.Find("BaggedIcon").gameObject;
		bagged = child.GetComponent<SpriteRenderer>();
		bagged.enabled = false;
	}

	public void ClearSprite()
	{
		artwork.color = new Color(1f, 1f, 1f, 1f);
		artwork.enabled = false;
		bagged.enabled = false;
	}

	public void SetSprite(Sprite art)
	{
		artwork.sprite = art;
		artwork.enabled = true;
	}

	public void SetBagged()
	{
		// bagged.enabled = true;
		artwork.color = new Color(0.2f, 0.2f, 0.2f, 1f);
	}
}
