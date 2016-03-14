using UnityEngine;
using System.Collections;

public class PickupZone : MonoBehaviour
{
    public float respawnTime = 3.0f;
    public Collectible collectible;

    void Start()
    {
        SpawnItem();
    }

    public void StartRespawn()
    {
        Invoke("SpawnItem", respawnTime);
    }

    void SpawnItem()
    {
        Collectible c = (Collectible) Instantiate(collectible);
        c.transform.position = this.transform.position;
        c.pickup = this;
    }
}
