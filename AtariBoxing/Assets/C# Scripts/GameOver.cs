using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class GameOver : MonoBehaviour {

	private Text p1Name;
	private Text p2Name;
	private Text p1Score;
	private Text p2Score;	
	GameServerConnection gsConnection;
	tcpclient clientConnection;
	GameObject clientConnectionManager;
	// Use this for initialization
	void Start () {
		 Debug.Log("Game Over script started!");
		 Debug.Log("Role: " + GlobalData.Role);
		 Debug.Log("Character: " + GlobalData.Character);
		 if (GlobalData.Role == "HOST")
		 {			     				
            gsConnection = GetComponent<GameServerConnection>();	
		 }
		 p1Name = GameObject.Find("lblP1Namego").GetComponent<Text>();
		 p2Name = GameObject.Find("lblP2Namego").GetComponent<Text>();
		 p1Score = GameObject.Find("lblP1Scorego").GetComponent<Text>();
		 p2Score = GameObject.Find("lblP2Scorego").GetComponent<Text>();
		 if (GlobalData.Character == "P1")
		 {	
			 
			 if (GlobalData.Role == "HOST"){
				
				try
				{
					clientConnection.SendRequest("SCORE", GlobalData.GameSessionToken, GlobalData.LocalPlayerScore.ToString() + " " + GlobalData.RemotePlayerScore.ToString());		 
					gsConnection.SendRequest("SCORE", GlobalData.GameSessionToken, GlobalData.LocalPlayerScore.ToString() + " " + GlobalData.RemotePlayerScore.ToString());		 	
				}
				catch (System.Exception e)
				{
					
					Debug.Log("Could not send score - " + e.ToString());
				}
			 	
			 }
			 
			 p1Name.text = GlobalData.LocalPlayerName.Truncate(11);
			 p2Name.text = GlobalData.RemotePlayerName.Truncate(11);
			 p1Score.text = GlobalData.LocalPlayerScore.ToString("000000000000");
			 p2Score.text = GlobalData.RemotePlayerScore.ToString("000000000000");
		 }
		 else
		 {	
			 if (GlobalData.Role == "HOST"){				
				try
				{
					clientConnection.SendRequest("SCORE", GlobalData.GameSessionToken, GlobalData.RemotePlayerScore.ToString() + " " + GlobalData.LocalPlayerScore.ToString());		 		 			 
					gsConnection.SendRequest("SCORE", GlobalData.GameSessionToken, GlobalData.RemotePlayerScore.ToString() + " " + GlobalData.LocalPlayerScore.ToString());		 
				}
			 	catch(System.Exception e )
				 {
					 Debug.Log("Could not send score - " + e.ToString());
				 }
			 }			 
			 p1Name.text = GlobalData.RemotePlayerName.Truncate(11);
			 p2Name.text = GlobalData.LocalPlayerName.Truncate(11);
			 p1Score.text = GlobalData.RemotePlayerScore.ToString("000000000000");
			 p2Score.text = GlobalData.LocalPlayerScore.ToString("000000000000");
		 }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
}
