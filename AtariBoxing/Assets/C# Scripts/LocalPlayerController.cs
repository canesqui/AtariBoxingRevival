using UnityEngine;
using System.Collections;
using Misc;

public class LocalPlayerController : MonoBehaviour {

	Animator anim;
	tcpclient sn;    
	// Use this for initialization
	void Start(){
		anim = GetComponent<Animator>();
		sn = GetComponent<tcpclient>();
	}

	public float speed = 2.5f;
	//called once per frame
	void Update ()
     {			 
		 anim.SetBool("LeftPunch", false);
		 anim.SetBool("RightPunch", false);
         if (Input.GetKey(KeyCode.LeftArrow))
         {
			 sn.Command(SupportedCommand.Left);
             transform.position += Vector3.left * speed * Time.deltaTime;			 
         }
         if (Input.GetKey(KeyCode.RightArrow))
         {
			 sn.Command(SupportedCommand.Right);
             transform.position += Vector3.right * speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.UpArrow))
         {
			 sn.Command(SupportedCommand.Up);
             transform.position += Vector3.up * speed * Time.deltaTime;
         }
         if (Input.GetKey(KeyCode.DownArrow))
         {
			 sn.Command(SupportedCommand.Down);
             transform.position += Vector3.down * speed * Time.deltaTime;
         }		 
		 if (Input.GetKey(KeyCode.A))
		 {			 
			 sn.Command(SupportedCommand.LeftPunch);
			 anim.SetBool("LeftPunch", true);	
		 }
		 if (Input.GetKey(KeyCode.D))
		 {
			 sn.Command(SupportedCommand.RightPunch);
			 anim.SetBool("RightPunch", true);	
		 }
     }	 
}
