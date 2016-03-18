using System.Collections.Generic;

public class SettingsEntry {
    public string description;
    public string key;
    public string val;
}

/**
 * A global Settings confiuration
 * Used as a key value store for faster playtesting configurations
 */
public class Settings {

    private static Settings instance;

    private List<SettingsEntry> entries;

    private string[] defaultEntries = {
        "player_speed", "20.0", "the speed of the player, float",
        "player_health", "100.0", "the player health, float",
    };

    private Settings()
    {
        entries = new List<SettingsEntry>();

        for (int i = 0; i < defaultEntries.Length;)
        {
            SettingsEntry entry = new SettingsEntry();
            entry.key = defaultEntries[i++];
            entry.val = defaultEntries[i++];
            entry.description = defaultEntries[i++];
            entries.Add(entry);
        }
    }

    public static Settings Instance()
    {
        if (instance == null)
        {
            instance = new Settings();
        }

        return instance;
    }

    /*
     * Get the setting specified by 'key'
     * Returns null or the optional defaultValue if not found
     */
    public string Get(string key, string defaultValue = null)
    {
        SettingsEntry entry = FindEntry(key);

        if (entry != null)
        {
            return entry.val;
        }
        else
        {
            return defaultValue;
        }
    }

    /*
     * Set the setting specified by 'key' to 'val'
     * Removes the setting if 'val' is null
     */
    public void Set(string key, string val)
    {
        SettingsEntry entry = FindEntry(key);

        if (entry == null)
        {
            entry = new SettingsEntry();
            entry.key = key;
            entry.val = val;
            entries.Add(entry);
        }
        else
        {
            entry.val = val;
        }
    }

    public List<SettingsEntry> GetEntries()
    {
        return entries;
    }

    private SettingsEntry FindEntry(string key)
    {
        SettingsEntry entry = null;
        foreach (SettingsEntry e in entries)
        {
            if (e.key == key)
            {
                entry = e;
                break;
            }
        }

        return entry;
    }
}
