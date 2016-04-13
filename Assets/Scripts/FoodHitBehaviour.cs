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
    private bool onConveyor = false;
	private bool friendlyOnConveyor = false;

    public void Update(float velocity)
    {
        if (dropTime + dropWaitTime < Time.time)
            recentlyDropped = false;

        if (lastChargeTime + waitTime > Time.time)
            return;

        if (velocity < chargeVelocity && !onConveyor)
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

    public bool CanPickup(Team otherTeam)
    {
        if (onConveyor && !friendlyOnConveyor)
		{
            return team != otherTeam;
		}

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

    public void NotifyOnConveyor(Team _team)
    {
    	team = _team;
		Debug.Log(team);
		onConveyor = true;
		friendlyOnConveyor = true; // always true when dropping off
    }

    public void NotifyOffConveyor()
    {
        this.team = Team.NONE;
		onConveyor = false;
    }

	public void NotifyFriendlyOnConveyor()
	{
		friendlyOnConveyor = true;
	}

	public void NotifyNoFriendlies()
	{
		friendlyOnConveyor = false;
	}
}
