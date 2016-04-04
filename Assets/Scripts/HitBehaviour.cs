using UnityEngine;

public class HitBehaviour
{
    private float chargeVelocity = 3.0f;
    public bool canHit = false;

    private float lastChargeTime;
    private float waitTime = 0.05f;

    public Team team;

    public void Update(float velocity)
    {
        if (lastChargeTime + waitTime > Time.time)
            return;

        if (velocity < chargeVelocity)
        {
            canHit = false;
        }
    }

    public bool CanHit(HitBehaviour hitBehaviour)
    {
        if (hitBehaviour.canHit == true || team == hitBehaviour.team)
            return false;

        return canHit;
    }

    public void NotifyCharge()
    {
        canHit = true;
        lastChargeTime = Time.time;
    }
}
