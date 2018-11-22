using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col)
     {		
			GlobalData.RemotePlayerScore += 1;	
			Debug.Log("Point goes to remote player Local: " + GlobalData.LocalPlayerScore.ToString() + " : Remote: " + GlobalData.RemotePlayerScore.ToString());
     }
}
