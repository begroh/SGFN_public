using UnityEngine;

public class PlayerSandbox : MonoBehaviour
{
    public GameObject playerPrefab;
    public Collectible collectible;
    public GameObject conveyorPrefab;

    private int playerNumber;

    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;

        SpawnPlayer();
        SpawnItem();
        SpawnConveyor();
    }

    private void SpawnPlayer()
    {
        GameObject go = Instantiate(playerPrefab);
        go.transform.parent = transform;
        go.transform.position = transform.position;

        Player player = go.GetComponent<Player>();

        player.playerNumber = playerNumber;
        player.useKeyboard = false;
    }

    private void SpawnItem()
    {
        Collectible c = (Collectible) Instantiate(collectible);
        c.transform.parent = transform;
        c.transform.position = transform.position + Vector3.up * 7;
    }

    private void SpawnConveyor()
    {
        GameObject go = Instantiate(conveyorPrefab);
        ConveyorBelt belt = go.GetComponent<ConveyorBelt>();
        belt.transform.parent = transform;
        belt.transform.position = transform.position - Vector3.up * 7;
        belt.EnableSandbox();
    }

    public void Ready()
    {
        transform.parent.GetComponent<PlayerBox>().Ready();
    }
}
