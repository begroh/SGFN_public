using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public PickupZone pickup;
	public bool shouldRespawn = true;

    void OnDestroy()
    {
		if (shouldRespawn) {
			pickup.StartRespawn ();
		}
    }
}
