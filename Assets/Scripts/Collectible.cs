using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public PickupZone pickup;

    void OnDestroy()
    {
        //pickup.StartRespawn();
    }
}
