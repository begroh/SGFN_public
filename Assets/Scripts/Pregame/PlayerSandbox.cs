using UnityEngine;
using UnityEngine.UI;

public class PlayerSandbox : MonoBehaviour
{
    public GameObject playerPrefab;
    public FoodItem[] collectible;
    public GameObject conveyorPrefab;

    private int playerNumber;
    private Text readyText;
    private Text instructionsText;

    void Awake()
    {
        readyText = transform.Find("ReadyText").GetComponent<Text>();
        readyText.enabled = false;

        instructionsText = transform.Find("InstructionsText").GetComponent<Text>();
    }

    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;

		Team team = SpawnPlayer();
        SpawnItem(-8f, team);
		SpawnItem(8f, team);
        SpawnConveyor();
    }

    private Team SpawnPlayer()
    {
        GameObject go = Instantiate(playerPrefab);
        go.transform.parent = transform;
        go.transform.position = transform.position;

        Player player = go.GetComponent<Player>();

        player.playerNumber = playerNumber;
        player.useKeyboard = false;
        player.EnableMove();
        if (playerNumber < 3)
        {
            player.team = Team.RED;
        }
        else
        {
            player.team = Team.BLUE;
        }

		return player.team;
    }

	private void SpawnItem(float pos, Team team)
    {
		int index = Random.Range (0, 5);
		FoodItem f = (FoodItem) Instantiate(collectible[index]);
        f.transform.parent = transform;
		f.transform.position = transform.position + Vector3.up * 3 + Vector3.right * pos;
		f.EnableSandbox (team);
    }

    private void SpawnConveyor()
    {
        GameObject go = Instantiate(conveyorPrefab);
        ConveyorBelt belt = go.GetComponent<ConveyorBelt>();
        belt.transform.parent = transform;
        belt.transform.position = transform.position - Vector3.up * 6.7f;
        belt.EnableSandbox();
    }

    public void Ready()
    {
        transform.parent.GetComponent<PlayerBox>().Ready();
        readyText.enabled = true;
        instructionsText.enabled = false;
    }
}
