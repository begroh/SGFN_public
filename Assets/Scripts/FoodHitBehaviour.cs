using UnityEngine;

public class FoodHitBehaviour
{
    private Team team;
    private float chargeVelocity = 3.0f;
    public bool canHit = false;
    private bool recentlyDropped = false;

    private float lastChargeTime;
    private float waitTime = 0.05f;
    private float dropWaitTime = 0.5f;
    private float dropTime;

    public void Update(float velocity)
    {
        if (dropTime + dropWaitTime < Time.time)
            recentlyDropped = false;

        if (lastChargeTime + waitTime > Time.time)
            return;

        if (velocity < chargeVelocity)
        {
            canHit = false;
            team = Team.NONE;
        }
    }

    public bool CanHit(HitBehaviour hitBehaviour)
    {
        if (hitBehaviour.canHit == true || hitBehaviour.team == team)
            return false;

        return canHit;
    }

    public void NotifyHit()
    {
        canHit = false;
        recentlyDropped = true;
        dropTime = Time.time;
    }

    public bool CanPickup(HitBehaviour hitBehaviour)
    {
        return !(recentlyDropped || canHit);
    }

    public void NotifyCharge(Team team)
    {
        canHit = true;
        lastChargeTime = Time.time;
        this.team = team;
    }

    public void NotifyDropped()
    {
        recentlyDropped = true;
        dropTime = Time.time;
    }
}
