using UnityEngine;
using InControl;
using UnityEngine.UI;
using System.Collections;

public class WinMessage : MonoBehaviour
{
    private Text text;

    void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
    }

    public void Win(Team team)
    {
    	Score.NotifyDone();
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
	Invoke("FadeToBlack",1.5f);
        Invoke("Load", 3f);
    }

    private void FadeToBlack()
    {
	StartCoroutine(StopProgress());
    }

    private void Load()
    {
		ShoppingList.ForTeam(Team.RED).Reset();
		ShoppingList.ForTeam(Team.BLUE).Reset();
    }

    IEnumerator StopProgress()
    {
    	float elapzedTime = 0f;
	while (elapzedTime < 1.5f)
	{
		if (Time.timeScale < .3f)
		{
			Time.timeScale = 1f;
        		Application.LoadLevel("Endgame");
		}
		else
		{
			Time.timeScale = Mathf.Lerp(1f, 0f, elapzedTime / 1.5f);
			elapzedTime += Time.deltaTime;
		}
		yield return null;
	}
    }
}
