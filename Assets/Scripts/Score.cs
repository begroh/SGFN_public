using System.Collections.Generic;

public class Score
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
        scores[team] = ForTeam(team) + (int)score;
    }
}
