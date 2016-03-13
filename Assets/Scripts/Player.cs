using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 4;
    private Rigidbody2D body;
    private PlayerControl.PlayerInput input;

    void Awake()
    {
        this.body = GetComponent<Rigidbody2D>();
        this.input = new PlayerControl.KeyboardAndMouseInput();
    }

    void Update()
    {
        input.DetectInput(this);
    }

    /*
     * Called by a PlayerInput with the users desired move direction
     */
    public void HandleMoveDirection(Vector2 direction)
    {
        this.body.velocity = direction.normalized * speed;
    }
}
