using UnityEngine;

class ConveyorZone : MonoBehaviour
{
    private SpriteRenderer renderer;
    private float minOpacity = 0.25f;
    private float maxOpacity = 0.75f;
    private float step = 0.01f;
    private float opacity;
    private bool opacify = false;
    private Color color;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        opacity = maxOpacity;
        ChangeTeam(Team.NONE);
    }

    void FixedUpdate()
    {
        if (opacity < minOpacity)
        {
            opacify = true;
        }

        if (opacity > maxOpacity)
        {
            opacify = false;
        }

        if (opacify)
        {
            opacity += step;
        }
        else
        {
            opacity -= step;
        }

        renderer.color = new Color(color.r, color.g, color.b, opacity);
    }

    public void ChangeTeam(Team team)
    {
        switch (team)
        {
            case Team.RED:    color = new Color(1f, 0f, 0f, 1f); break;
            case Team.BLUE:   color = new Color(0f, 0f, 1f, 1f); break;
            case Team.YELLOW: color = new Color(1f, 1f, 0f, 1f); break;
            case Team.GREEN:  color = new Color(0f, 1f, 0f, 1f); break;
            case Team.NONE:   color = new Color(1f, 1f, 1f, 1f); break;
        }
    }
}
