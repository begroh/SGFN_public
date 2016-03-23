using UnityEngine;

public class ChargeBumpBehaviour : BumpBehaviour
{
    private float charge = 0;
    private float rate = 0.015f;

    private float strength = 125f;

    private float lastBump = 0;
    private float bumpWait = 2f;

    public void Update(Player player, bool bumping)
    {
        if (lastBump == 0 || Time.time - lastBump > bumpWait)
        {
            Rigidbody2D body = player.gameObject.GetComponent<Rigidbody2D>();

            if (bumping)
            {
                if (charge < 1.0f)
                {
                    charge += rate;
                }

                body.velocity = body.velocity * (1 - (charge * 0.75f));
            }
            else
            {
                if (charge > 0)
                {
                    Vector2 direction = player.transform.rotation * Vector2.right;
                    body.AddForce(charge * strength * direction, ForceMode2D.Impulse);
                    charge = 0;
                }
            }
        }
    }
}
