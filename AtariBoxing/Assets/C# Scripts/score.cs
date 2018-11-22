using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class score : MonoBehaviour {

	public static int scoreValue = 0;
	TextMeshProUGUI scoreText;
	// Use this for initialization
	void Start () {
		//.text = "Hello World";
		scoreText = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Score: " + scoreValue);
		//scoreRemotePlayer.text = "Score " + scoreValue;
		scoreText.text = "Score: " + scoreValue;
	}
}
