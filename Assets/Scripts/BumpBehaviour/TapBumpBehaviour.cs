using UnityEngine;

public class TapBumpBehaviour : BumpBehaviour
{
    private float lastBump = 0;
    private float strength = 75;
    private Cooldown cooldown;

    public TapBumpBehaviour()
    {
        this.cooldown = new Cooldown(2f);
    }

    public void Update(Player player, bool bumping)
    {
        if (!bumping)
        {
            return;
        }

        if (cooldown.Ready())
        {
            Rigidbody2D body = player.gameObject.GetComponent<Rigidbody2D>();

            // Add a force in the direction that the player is moving
            body.AddForce(strength * body.velocity.normalized, ForceMode2D.Impulse);

            cooldown.Reset();
        }
    }
}
