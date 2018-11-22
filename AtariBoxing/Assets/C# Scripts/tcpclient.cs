using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Misc;

public class tcpclient : MonoBehaviour {  	
	#region private members 	
	private TcpClient socketConnection; 	
	private Thread clientReceiveThread; 	
	const string protocolVersion = "TKSG10";			
	RemotePlayerController remo;
	GameObject remotePlayer;
	#endregion  	
	// Use this for initialization 	
	void Start () {	
		
		remotePlayer = GameObject.Find("Player2");
		 		 		 
		remo = remotePlayer.GetComponent<RemotePlayerController>();
		
		ConnectToTcpServer(); 		    
	}  	
	// Update is called once per frame
	void Update () {        

	}  	
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer () { 		
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start();  		
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}  	
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData() { 		
		try { 			
			socketConnection = new TcpClient(GlobalData.Ip, GlobalData.Port);  			
			Byte[] bytes = new Byte[9];             
			while (true) { 				
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
			Debug.Log("Socket exception: " + socketException.ToString());         
		}     
	}

	private void RequestHandler(string[] request){		
		switch (request[1])
      	{
         case "R":
            remo.SendToCommandQueue("R");
            break;
         case "L":
            remo.SendToCommandQueue("L");
            break;
         case "U":		 	
			remo.SendToCommandQueue("U");        
            break;
		 case "D":
		 	remo.SendToCommandQueue("D");
		 	break;
		 case "RP":
		 	remo.SendToCommandQueue("RP");
		 	break;
		 case "LP":
		 	remo.SendToCommandQueue("LP");
		 	break;	 	
		 case "SCORE":
		 	if (GlobalData.Character == "P1")
		 	{
				GlobalData.RemotePlayerScore = int.Parse(request[3]);
				GlobalData.LocalPlayerScore = int.Parse(request[4]);
			 }else{
				GlobalData.RemotePlayerScore = int.Parse(request[4]);
				GlobalData.LocalPlayerScore = int.Parse(request[3]);
			 }
		 	break;	 
         default:
            Debug.Log("Unknow request received - " + request[1]);
            break;   
      	}
	}  	

	public void Command(SupportedCommand param)
	{
		
		switch (param)
		{
			case SupportedCommand.Up:
			  SendMessage("U");	
			  break;
			case SupportedCommand.Down:
			  SendMessage("D");	
			  break;
			case SupportedCommand.Right:
			  SendMessage("R");	
			  break;
			case SupportedCommand.Left:
			  SendMessage("L");	
			  break;
			case SupportedCommand.RightPunch:
			  SendMessage("RP");	
			  break;
			case SupportedCommand.LeftPunch:
			  SendMessage("LP");	
			  break;			  			  		  				
			default:
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
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}


	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	private void SendMessage(string message) {         				
		if (socketConnection == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 
	
				message = protocolVersion + " " + message;

				if (message.Length == 8){
					message = message + " ";
				} 				
				// Convert string message to byte array.                 
	
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message); 				
				// Write byte array to socketConnection stream.                 
	
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 					            
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	} 
}