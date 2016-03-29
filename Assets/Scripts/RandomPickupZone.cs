using UnityEngine;
using System.Collections;

public class RandomPickupZone : PickupZone {

	public float checkRadius = 1;
	public LayerMask walls = -1;
	public LayerMask pickups = -1;
	public LayerMask players = -1;

	override protected void SpawnItem()
	{
		Collectible c = (Collectible) Instantiate(collectible);

		float newX = 0, newY = 0;
		Vector2 newPos = Vector3.zero;
		Collider2D coll = null;

		do {
			float minX = this.transform.position.x - (this.transform.localScale.x / 2f);
			float maxX = this.transform.position.x + (this.transform.localScale.x / 2f);

			float minY = this.transform.position.y - (this.transform.localScale.y / 2f);
			float maxY = this.transform.position.y + (this.transform.localScale.y / 2f);

			newX = Random.Range (minX, maxX);
			newY = Random.Range (minY, maxY);
			newPos = new Vector2(newX, newY);

			coll = Physics2D.OverlapCircle(newPos, checkRadius, walls | players);

		} while(coll != null);
			
		c.transform.position = newPos;

		c.pickup = this;
	}
}
