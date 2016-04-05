using InControl;
using UnityEngine;

public class PlayerJoiner : MonoBehaviour
{
    private InputDevice device;

    public void SetInputDevice(InputDevice device)
    {
        this.device = device;
    }

    void Update()
    {
        if (device == null)
        {
            return;
        }

        if (device.Action1.WasPressed)
        {
            PlayerBox box = transform.parent.GetComponent<PlayerBox>();
            box.Join();
            Destroy(gameObject);
        }
    }
}
