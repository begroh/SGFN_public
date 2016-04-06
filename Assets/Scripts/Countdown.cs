using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public int start = 3;

    private Text text;
    private float startTime;

    void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
        text.text = "" + start;
        text.enabled = true;
    }

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float diff = Time.time - startTime;
        int count = start - (int) diff;

        if (count <= 0)
        {
            Done();
        }
        else
        {
            text.text = "" + count;
        }
    }

    private void Done()
    {
        foreach (Player player in FindObjectsOfType(typeof(Player)))
        {
            player.EnableMove();
        }
        Destroy(gameObject);
    }
}
