using UnityEngine;
using UnityEngine.UI;

public class SettingsSceneController : MonoBehaviour
{
    public GameObject settingsEntryPrefab;

    void Awake()
    {
        float y = 0;
        foreach (SettingsEntry entry in Settings.Instance().GetEntries())
        {
            ShowEntry(entry, y);
            y -= 100;
        }
    }

    private void ShowEntry(SettingsEntry entry, float y)
    {
        GameObject obj = Instantiate(settingsEntryPrefab);

        Text name = obj.transform.Find("Name").gameObject.GetComponent<Text>();
        Text description = obj.transform.Find("Description").gameObject.GetComponent<Text>();

        InputField val = obj.transform.Find("ValueInput").gameObject.GetComponent<InputField>();
        val.onValueChange.AddListener(delegate { Settings.Instance().Set(entry.key, val.text); });

        description.text = entry.description;
        name.text = entry.key;
        val.text = entry.val;

        GameObject canvas = GameObject.Find("Canvas");
        obj.transform.parent = canvas.transform;
        obj.transform.localScale = new Vector3(1, 1, 1);
        RectTransform transform = obj.GetComponent<RectTransform>();
        Vector3 pos = transform.localPosition;
        pos.y += y;
        pos.x += 50;
        transform.localPosition = pos;
    }
}
