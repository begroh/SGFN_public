using UnityEngine;
using System.Collections.Generic;

public class Score : MonoBehaviour
{
    private static Dictionary<Team, int> scores = new Dictionary<Team, int>();
	public static float MAX_SCORE = 250f;

    public static int ForTeam(Team team)
    {
        int score;
        if (scores.TryGetValue(team, out score))
        {
            return score;
        }
        else
        {
            return 0;
        }
    }

    public static void AddForTeam(Team team, float score)
    {
        int newScore = ForTeam(team) + (int)score;
        scores[team] = newScore;

        if (newScore >= MAX_SCORE)
        {
            WinMessage message = (WinMessage) FindObjectOfType(typeof(WinMessage));
            message.Win(team);
        }
    }

    public static void Reset()
    {
        scores[Team.RED] = 0;
        scores[Team.BLUE] = 0;
    }
}
