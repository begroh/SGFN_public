using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public PickupZone pickup;
	public bool shouldRespawn = true;

	virtual protected void OnDestroy()
    {
		if (shouldRespawn && pickup != null) {
			pickup.StartRespawn (0f);
		}
    }
		
}
