using UnityEngine;
using System.Collections;
using InControl;

public class TutorialVideo : MonoBehaviour {

	public MovieTexture myMovie;

	void Start()
	{
		myMovie.Stop ();
		myMovie.Play();
		StartCoroutine(FindEnd());
	}

	void Update () {
		if (InputManager.Devices[0].Action2.WasPressed ||
			InputManager.Devices[1].Action2.WasPressed ||
			InputManager.Devices[2].Action2.WasPressed ||
			InputManager.Devices[3].Action2.WasPressed)
		{
			Application.LoadLevel("Menu");
			Score.Reset();
		}
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myMovie);
	}


	private IEnumerator FindEnd()
	{
		while(myMovie.isPlaying)
		{
			yield return 0;
		}
		Application.LoadLevel("Menu");
	}

}
