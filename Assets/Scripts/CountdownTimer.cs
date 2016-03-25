using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {

	public float timeLeft = 100;

	Text text;

	void Awake ()
	{
		text = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;

		int minutesLeft = (int)timeLeft / 60;
		int secondsLeft = (int)timeLeft % 60;


		text.text = "" + minutesLeft + ":" + secondsLeft;
		if(timeLeft <= 0) {
			//Application.LoadLevel("gameOver");
		}
	}
}