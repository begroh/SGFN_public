using UnityEngine;
using System.Collections;

public class DegradingCollectible : Collectible {

	public float destroyTime = 3f;
	public bool destroy = false;
	private float destroyTimer = 0f;
	private SpriteRenderer spRend;

	void Start () {
		spRend = gameObject.GetComponent<SpriteRenderer> ();
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
	}

	override protected void OnDestroy()
	{
	}
}
