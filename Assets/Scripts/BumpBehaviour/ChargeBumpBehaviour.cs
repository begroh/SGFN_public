using UnityEngine;

public class ChargeBumpBehaviour : BumpBehaviour
{
    private float charge = 0;
    private float rate = 0.015f;
    private float strength = 125f;

    private Cooldown cooldown;

    private Gun gun;

    public ChargeBumpBehaviour(Gun gun)
    {
        this.gun = gun;
        this.cooldown = new Cooldown(2f);
    }

    public void Update(Player player, bool bumping)
    {
        Rigidbody2D body = player.gameObject.GetComponent<Rigidbody2D>();

        if (bumping)
        {
            if (cooldown.Ready())
            {
                if (charge < 1.0f)
                {
                    charge += rate;
                }

                body.velocity = body.velocity * (1 - (charge * 0.75f));
            }
        }
        else if (charge > 0)
        {
            cooldown.Reset();
            Vector2 direction = gun.transform.rotation * Vector2.right;
            body.AddForce(charge * strength * direction, ForceMode2D.Impulse);
            charge = 0;
        }
    }
}
