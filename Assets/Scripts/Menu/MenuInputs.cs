using UnityEngine;
using System.Collections;
using InControl;

public class MenuInputs : MonoBehaviour {

	void Update () {
        if (InputManager.Devices[0].MenuWasPressed ||
            InputManager.Devices[1].MenuWasPressed ||
            InputManager.Devices[2].MenuWasPressed ||
            InputManager.Devices[3].MenuWasPressed)
        {
            Application.LoadLevel("Pregame");
            Score.Reset();
        }

        if (InputManager.Devices[0].Action4.WasPressed ||
            InputManager.Devices[1].Action4.WasPressed ||
            InputManager.Devices[2].Action4.WasPressed ||
            InputManager.Devices[3].Action4.WasPressed)
        {
			Application.LoadLevel("TutorialVideo");
        }
			
	}

}
