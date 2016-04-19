using UnityEngine;
using UnityEngine.UI;

public class WinMessage : MonoBehaviour
{
    private Text text;

    void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
    }

    public void Win(Team team)
    {
        Color color;
        string name;
        if (team == Team.RED)
        {
            color = Color.red;
            name = "Red";
        }
        else
        {
            color = Color.blue;
            name = "Blue";
        }

        text.text = name + " Team Wins!";
        text.color = color;
        text.enabled = true;
        Invoke("Load", 3f);
    }

    private void Load()
    {
        Score.Reset();
		ShoppingList.ForTeam(Team.RED).Reset();
		ShoppingList.ForTeam(Team.BLUE).Reset();
        Application.LoadLevel("Pregame");
    }
}
