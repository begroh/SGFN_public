using UnityEngine;

public class Back : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            string level;
            switch (Application.loadedLevelName)
            {
                case "NewEricScene":
                    level = "Pregame";
                    break;
                default:
                    level = "Menu";
                    break;
            }
            Application.LoadLevel(level);
        }
    }
}
