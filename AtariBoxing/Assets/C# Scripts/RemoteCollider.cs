using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col)
     {		
			GlobalData.LocalPlayerScore += 1;				
			Debug.Log("Point goes to local player " + GlobalData.LocalPlayerScore.ToString() + " : Remote: " + GlobalData.RemotePlayerScore.ToString());
     }
}
