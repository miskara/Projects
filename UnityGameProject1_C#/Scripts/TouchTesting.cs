using UnityEngine;
using System.Collections;

public class TouchTesting : InputController {


	public override void OnTouchBegan(){

		OnTouchBegan (this.gameObject);
	
	}

	public void OnTouchBegan(GameObject go){
	
		Debug.Log ("Touched object: " + this.name);

	}
}
