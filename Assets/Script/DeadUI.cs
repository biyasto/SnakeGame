using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeadUI : MonoBehaviour {

	private Text DeadText;
	void Awake () {
		DeadText = transform.Find ("DeadText").GetComponent<Text> ();
		DeadText.text="";

	}

	// Update is called once per frame
	void Update () {
		if (!GameHandler.GetStarted ()) {
			DeadText.text = "Press T to Start";
			DeadText.color = Color.green;
		} else if (GameHandler.GetStatus ()) {
			DeadText.text = "YOU DIED \n\npress R to Try again";
			DeadText.color = Color.red;
		}
		else 								DeadText.text="";
		
	}
}
