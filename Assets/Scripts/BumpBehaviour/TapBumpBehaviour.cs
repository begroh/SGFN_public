using UnityEngine;

public class TapBumpBehaviour : BumpBehaviour
{
    private float lastBump = 0;
    private float bumpWait = 2f;
    private float strength = 75;

    public void Bump(Player player)
    {
        if (Time.time - lastBump > bumpWait || lastBump == 0)
        {
            Rigidbody2D body = player.gameObject.GetComponent<Rigidbody2D>();

            // Add a force in the direction that the player is moving
            body.AddForce(strength * body.velocity.normalized, ForceMode2D.Impulse);

            lastBump = Time.time;
        }
    }
}
