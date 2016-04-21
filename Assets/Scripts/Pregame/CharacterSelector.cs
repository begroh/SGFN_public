using InControl;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    private InputDevice device;
    private int position;
    private int playerNumber;
    private SpriteRenderer renderer;
    private bool needReset = false;
    private string[] characters = {
        "Bigman",
        "Catman"
    };
    private string color;
    private Text text;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        text = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        position = 0;
    }

    void Start()
    {
        PlayerBox box = transform.parent.GetComponent<PlayerBox>();
        color = box.playerNumber < 3 ? "Red" : "Blue";
        this.playerNumber = box.playerNumber;
        SelectCharacter(position);
    }

    void Update()
    {
        if (device == null)
        {
            return;
        }

        if (needReset)
        {
            if (device.LeftStickX < 0.1f && device.LeftStickX > -0.1f)
            {
                ResetStick();
            }
        }
        else if (device.LeftStickX > 0.9f)
        {
            SelectCharacter(position - 1);
        }
        else if (device.LeftStickX < -0.9f)
        {
            SelectCharacter(position + 1);
        }

        while (!CharacterSelection.Available(playerNumber, characters[position]))
        {
            SelectCharacter(position + 1);
        }

        if (device.Action1.WasPressed)
        {
            PlayerBox box = transform.parent.GetComponent<PlayerBox>();
            string name = characters[position];
            box.SelectCharacter(name);
            Destroy(gameObject);
        }
    }

    private void ResetStick()
    {
        CancelInvoke();
        needReset = false;
    }

    public void SetInputDevice(InputDevice device)
    {
        this.device = device;
    }

    private void SelectCharacter(int newPosition)
    {
        Invoke("ResetStick", 0.2f);
        needReset = true;

        int a = newPosition;
        int b = characters.Length;
        position = (a % b + b) % b;

        string name = characters[position];
        text.text = "Press A to choose " + name;
        renderer.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name + color);
    }
}
