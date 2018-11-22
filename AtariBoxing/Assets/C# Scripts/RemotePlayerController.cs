using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RemotePlayerController : MonoBehaviour {

	Animator anim;    
    Queue<string> commandQueue;

	// Use this for initialization
	void Start(){
		anim = GetComponent<Animator>();
        commandQueue = new Queue<string>();    
	}

	public float speed = 2.5f;
    private string command;
	//called once per frame
	void Update ()
     {			 
         anim.SetBool("LeftPunch", false);
		 anim.SetBool("RightPunch", false);
         if (commandQueue.Count > 0)
         {
            lock(commandQueue){
                command = commandQueue.Dequeue();
            }                        
            switch (command)
            {
             case "L":
                transform.position += Vector3.left * speed * Time.deltaTime;
                break;
             case "R":
                transform.position += Vector3.right * speed * Time.deltaTime;
                break;
             case "U":
                transform.position += Vector3.up * speed * Time.deltaTime;   
                break;
             case "D":
                transform.position += Vector3.down * speed * Time.deltaTime;
                break;
             case "RP":
                anim.SetBool("RightPunch", true);	
                break;
             case "LP":
                anim.SetBool("LeftPunch", true);	
                break;
             default:
                break;
            }
         }
         /* 
         if (Input.GetKey(KeyCode.A))
         {
             transform.position += Vector3.left * speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.D))
         {
             transform.position += Vector3.right * speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.W))
         {
             transform.position += Vector3.up * speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.Z))
         {
             transform.position += Vector3.down * speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.C))
		 {			 
			 anim.SetBool("LeftPunch", true);	
		 }
		 if (Input.GetKey(KeyCode.V))
		 {			 
			 anim.SetBool("RightPunch", true);	
		 }	*/		 	 		 
     }

    public void SendToCommandQueue(string command)
    {                
        commandQueue.Enqueue(command);
    }
    	
}
