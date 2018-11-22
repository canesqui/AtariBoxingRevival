using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Misc;
using UnityEngine.SceneManagement;

public class GameServerConnection : MonoBehaviour {  	
	#region private members 	
	private TcpClient socketConnection; 	
	private Thread clientReceiveThread; 	
	private string ip;
	private int port;
	private string token;
	private string lastReceivedMessage;
	const string protocolVersion = "TKSG10";			
	
	private Queue<string> changeScene = new Queue<string>();

	public InputField Ip;
	public InputField Port;
    public InputField Token;

	#endregion  	
	// Use this for initialization 	
	void Start () {	
		
	}  	
	// Update is called once per frame
	void Update () {        
		//Unity has some restrictions on thread processing. Scene or game objects can
		//only be change in the main thread. So far I did not find a better way to do this.
		if (changeScene.Count > 0){
			lock(changeScene){
				SceneManager.LoadScene(changeScene.Dequeue());	
			}			
		}		
	}  	
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer () { 		
		Debug.Log("ConnectToTcpServer called " + ip + port);
		
		socketConnection = new TcpClient(ip, port); 
		
		try {  			
			Debug.Log("Before creating the thread");
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start();			  					
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		}	
		SendRequest("PLAY", token); 		
	}  	
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData() { 		
		try { 						
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				Debug.Log("Listening for data running");
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 						
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData); 
						string[] strArr;
						strArr = serverMessage.Split(' ');
						Debug.Log("Data received: " + serverMessage + " Size: " + serverMessage.Length);
						Debug.Log("Protocol version: " + strArr[0]);
						Debug.Log("Rquest: "+strArr[1]);  
						if (strArr[0]=="TKSG10"){
							try
							{
								RequestHandler(strArr);		
							}
							catch (Exception e)	
							{
							    Debug.Log("Exception: " + e);
							}							
						}
						else{
							//For security reasons invalid requests will be ignored
							Debug.Log("Invalid request."); 						
						}												
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}

	private void RequestHandler(string[] request){
		Debug.Log("Request receive: "+ request);
		switch (request[1])
      	{
         case "QUEUE":
            //Change scene
			lock (changeScene)
			{
				changeScene.Enqueue("Waiting");
			}			
			Debug.Log("Sent to QUEUE");
            break;
         case "RES":
		    lastReceivedMessage = request[2];            
            break; 
		 case "PEER":
		 	GlobalData.GameSessionToken = request[2];
			GlobalData.Ip = request[3];		
			GlobalData.Port = int.Parse(request[4]);	 	
			GlobalData.LocalPlayerName = request[5].Replace('#', ' ');
			GlobalData.RemotePlayerName = request[6].Replace('#', ' ');
			GlobalData.Role = request[7];
			GlobalData.Character = request[8];
		    lock (changeScene)
			{
				changeScene.Enqueue("SampleScene");
			}
            break;  	        	 	
         default:
            Debug.Log("Unknow message received - " + request);
            break;   
      	}
	}  	
	
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	    
	public void SendRequest(string command, string token, string payload = null) {         			
		if (socketConnection == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 
	
				var message = protocolVersion + " " + command + " " + token;
				if (payload!=null){
					message = message + " " + payload;
				} 				
				// Convert string message to byte array.                 	
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);

				// Write byte array to socketConnection stream.                 	
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 				
				Debug.Log("Client sent his message - should be received by server - " + message );             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}

	public void OnSubmit(){
		var connectionMananger = GameObject.Find("ConnectionManager");		 
		DontDestroyOnLoad(connectionMananger);
		ip = Ip.text;
		port = int.Parse(Port.text);
		token = Token.text;	
		Debug.Log("OnSubmit called " + ip + " " + port + " " + token);
		ConnectToTcpServer();
					   
		 		
	} 
}