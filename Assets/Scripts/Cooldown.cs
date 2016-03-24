using UnityEngine;

public class Cooldown {

    private float waitTime;
    private float lastUse = 0f;

    public Cooldown(float waitTime)
    {
        this.waitTime = waitTime;
    }

    public bool Ready()
    {
        return (Time.time - lastUse > waitTime || lastUse == 0);
    }

    public void Reset()
    {
        lastUse = Time.time;
    }
}
