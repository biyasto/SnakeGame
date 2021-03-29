using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreWindow : MonoBehaviour {

	private Text scoreText;
	void Awake () {
		scoreText = transform.Find ("scoreText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text =GameHandler.GetScore().ToString();
	}
}
