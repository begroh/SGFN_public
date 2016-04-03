using UnityEngine;
using System.Collections;

public class PickupZone : MonoBehaviour
{
    public Collectible collectible;

    void Start()
    {
        SpawnItem();
    }

	public void StartRespawn(float respawnTime)
	{
		Invoke("SpawnItem", respawnTime);
	}

    virtual protected void SpawnItem()
    {
        Collectible c = (Collectible) Instantiate(collectible);
        c.transform.position = this.transform.position;
        c.pickup = this;
    }

}
