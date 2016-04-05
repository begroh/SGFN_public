using UnityEngine;
using System.Collections;

public class DegradingCollectible : Collectible {

	public float destroyTime = 3f;
	public bool destroy = false;
	private float destroyTimer = 0f;
	private SpriteRenderer rend;
	private Color startColor;
	private int counter;

	void Start () {
		rend = gameObject.GetComponent<SpriteRenderer> ();
		startColor = rend.material.color;
	}

	// Update is called once per frame
	void Update () {
		if (destroy) {
			destroyTimer += Time.deltaTime;
			if (destroyTimer >= destroyTime) {
				Destroy (gameObject);
			}
		} else {
			destroyTimer = 0f;
		}

		if (destroyTimer + 2f > destroyTime) {
			counter++;
			if (counter % 3 != 0) {
				return;
			}

			if (rend.material.color == startColor) {
				rend.material.color = Color.black;
			} else {
				rend.material.color = startColor;
			}
		} else {
			rend.material.color = startColor;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "FoodPickup")
		{
			//destroy = true;
		}
	}

	override protected void OnDestroy()
	{
	}
}
