using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {


	RaycastHit hit;
	Ray ray;

	void Start () {
	
	}



	void Update () {


		if (InputController.HasTouchBegan ()) {
			Debug.Log ("Touch Detected!!");
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.gameObject.name.Contains ("Play")) {
					GameController.levels++;
					Application.LoadLevel (1); 
				}
			}
		}
	}
}

