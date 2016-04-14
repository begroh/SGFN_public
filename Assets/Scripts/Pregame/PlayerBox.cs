using InControl;
using System;
using UnityEngine;

public class PlayerBox : MonoBehaviour
{
    public int playerNumber;

    public GameObject playerJoinerPrefab;
    public GameObject characterSelectorPrefab;
    public GameObject playerSandboxPrefab;

    private InputDevice device;

    void Awake()
    {
        CharacterSelection.Reset(); // TODO move upwards
        device = GetDevice();
    }

    void Start()
    {
        GameObject joiner = Instantiate(playerJoinerPrefab);
        joiner.transform.parent = transform;
        joiner.transform.position = transform.position;
        joiner.GetComponent<PlayerJoiner>().SetInputDevice(device);
    }

    public void Join()
    {
        CharacterSelection.Join(playerNumber);
        GameObject selector = Instantiate(characterSelectorPrefab);
        selector.transform.parent = transform;
        selector.transform.position = transform.position;
        selector.GetComponent<CharacterSelector>().SetInputDevice(device);
    }

    public void SelectCharacter(string character)
    {
        CharacterSelection.Set(playerNumber, character);
        GameObject sandbox = Instantiate(playerSandboxPrefab);
        sandbox.transform.parent = transform;
        sandbox.transform.position = transform.position;
        sandbox.GetComponent<PlayerSandbox>().SetPlayerNumber(playerNumber);
    }

    public void Ready()
    {
        CharacterSelection.Ready(playerNumber);

        if (CharacterSelection.AllReady())
        {
            Application.LoadLevel("NewEricScene");
        }
    }

    private InputDevice GetDevice()
    {
        try
        {
            return InputManager.Devices[playerNumber - 1];
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogWarning("No joystick was found for player " + playerNumber);
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("No InControl InputManager, setup a InControlManager in this scene");
        }

        return null;
    }
}
