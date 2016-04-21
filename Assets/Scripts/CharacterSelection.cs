using UnityEngine;
using System.Collections.Generic;

public class CharacterSelection
{
    private static Dictionary<int, string> characters = new Dictionary<int, string>();
    private static Dictionary<int, bool> ready = new Dictionary<int, bool>();

    public static void Set(int playerNumber, string character)
    {
        characters[playerNumber] = character;
    }

    public static string Get(int playerNumber)
    {
        string character;
        characters.TryGetValue(playerNumber, out character);
        return character;
    }

    public static void Reset()
    {
        characters = new Dictionary<int, string>();
        ready = new Dictionary<int, bool>();
    }

    public static void Join(int playerNumber)
    {
        ready[playerNumber] = false;
    }

    public static void Ready(int playerNumber)
    {
        ready[playerNumber] = true;
    }

    public static bool AllReady()
    {
        foreach (KeyValuePair<int, bool> entry in ready)
        {
            if (entry.Value == false)
            {
                return false;
            }
        }

        return true;
    }

    public static bool Available(int playerNumber, string character)
    {
        int otherPlayer = 0;

        switch(playerNumber) {
            case 1: otherPlayer = 2; break;
            case 2: otherPlayer = 1; break;
            case 3: otherPlayer = 4; break;
            case 4: otherPlayer = 3; break;
        }

        return Get(otherPlayer) != character;
    }
}
