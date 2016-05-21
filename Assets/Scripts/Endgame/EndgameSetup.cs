using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

public class EndgameSetup : MonoBehaviour {

    void Start()
    {
        int redScore = Score.ForTeam(Team.RED);
        int blueScore = Score.ForTeam(Team.BLUE);

        if (redScore > blueScore)
        {
            SetupTeams(Team.RED, Team.BLUE);
        }
        else
        {
            SetupTeams(Team.BLUE, Team.RED);
        }

        foreach (Player player in FindObjectsOfType(typeof(Player)))
        {
            player.EnableMove();
        }
    }

    void Update()
    {
        if (InputManager.Devices[0].MenuWasPressed ||
            InputManager.Devices[1].MenuWasPressed ||
            InputManager.Devices[2].MenuWasPressed ||
            InputManager.Devices[3].MenuWasPressed)
        {
	    WinMessage.won = null;
            Application.LoadLevel("Menu");
            Score.Reset();
        }
    }

    private void SetupTeams(Team firstTeam, Team secondTeam)
    {
        Image first = transform.Find("FirstTeam").transform.Find("Progress").GetComponent<Image>();
        Image second = transform.Find("SecondTeam").transform.Find("Progress").GetComponent<Image>();

        FillBar(first, firstTeam);
        FillBar(second, secondTeam);
    }

    private void FillBar(Image bar, Team team)
    {
        bar.color = team == Team.BLUE ? Color.blue : Color.red;

        bar.fillAmount = Score.ForTeam(team) / Score.MAX_SCORE;
    }

}
