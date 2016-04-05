using UnityEngine;
using System.Collections.Generic;

public class Score : MonoBehaviour
{
    private static Dictionary<Team, int> scores = new Dictionary<Team, int>();

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

        if (newScore >= 150)
        {
            scores[Team.RED] = 0;
            scores[Team.BLUE] = 0;
            Application.LoadLevel("Pregame");
        }
    }
}
