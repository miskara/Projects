using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {


	public void Update(){
		if (Input.GetKey (KeyCode.Escape)) {
			Application.LoadLevel (0);
		}
	}

	public void Home(){
		
		Application.LoadLevel (0);
	}
}
