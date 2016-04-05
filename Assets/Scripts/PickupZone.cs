using UnityEngine;
using System.Collections;

public class PickupZone : MonoBehaviour
{
    public Collectible collectible;
	protected bool isSpawning = false;

    void Start()
    {
        SpawnItem();
    }

	public void StartRespawn(float respawnTime)
	{
		if (!isSpawning) {
			Invoke ("SpawnItem", respawnTime);
			isSpawning = true;
		}
	}

    virtual protected void SpawnItem()
    {
		Collectible c = (Collectible) Instantiate(collectible);
        c.transform.position = this.transform.position;
        c.pickup = this;
		isSpawning = false;
    }

}
