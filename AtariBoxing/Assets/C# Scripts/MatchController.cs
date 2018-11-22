using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchController : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;
	private GameObject player1Collider;
	private GameObject player2Collider;
	private Text p1Name;
	private Text p1Score;
	private Text p2Name;
	private Text p2Score;
	private Text timer;
	float timeLeft = 30.0f;
	// Use this for initialization
	void Start () 
	{		
		 player1 = GameObject.Find("Player1");
		 player2 = GameObject.Find("Player2");
		 player1Collider = GameObject.Find("Player1Collider");
		 player2Collider = GameObject.Find("Player2Collider");
		 
		 p1Name = GameObject.Find("lblP1Name").GetComponent<Text>();
		 p1Score = GameObject.Find("lblP1Score").GetComponent<Text>();
		 p2Name = GameObject.Find("lblP2Name").GetComponent<Text>();
		 p2Score = GameObject.Find("lblP2Score").GetComponent<Text>();
		 timer = GameObject.Find("lblTimer").GetComponent<Text>();
		 
		 if (GlobalData.Character == "P1")
		 {
			 player1.AddComponent<LocalPlayerController>();
			 player2.AddComponent<RemotePlayerController>();
			 p1Name.text = GlobalData.LocalPlayerName;			 
			 p2Name.text = GlobalData.RemotePlayerName;			 
			 player1Collider.AddComponent<LocalCollider>();
			 player2Collider.AddComponent<RemoteCollider>();
		 }
		 else
		 {
			 player1.AddComponent<RemotePlayerController>();
			 player2.AddComponent<LocalPlayerController>();
			 p1Name.text = GlobalData.RemotePlayerName;			 
			 p2Name.text = GlobalData.LocalPlayerName;
			 player1Collider.AddComponent<RemoteCollider>();
			 player2Collider.AddComponent<LocalCollider>();
		 }		 		 
		 player1.AddComponent<tcpclient>();
	}
	
	// Update is called once per frame
	void Update () {	

		if (GlobalData.Character == "P1")
		 {			 
			 p1Score.text = GlobalData.LocalPlayerScore.ToString("00000000000");			 
			 p2Score.text = GlobalData.RemotePlayerScore.ToString("000000000000");			 
		 }
		 else
		 {			
			 p1Score.text = GlobalData.RemotePlayerScore.ToString("00000000000");
			 p2Score.text = GlobalData.LocalPlayerScore.ToString("00000000000");			 
		 }		 		 

		 timer.text = string.Format("0:{0}", ((int)timeLeft).ToString("00"));		
		 timeLeft -= Time.deltaTime;             		
         if(timeLeft < 0)
         {
			 GameOver();
         }	
	}

	void GameOver(){	
		//var connectionMananger = GameObject.Find("ConnectionManager");		 
		//DontDestroyOnLoad(connectionMananger);	
		SceneManager.LoadScene("GameOver");	
		Debug.Log("Game over called");
	}
}
