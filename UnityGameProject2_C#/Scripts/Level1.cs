using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour {
	
	
	RaycastHit hit;
	Ray ray;
	
	void Start () {
		
	}
	
	
	
	void Update () {
		if (InputController.HasTouchBegan ()) {
			Debug.Log ("Touch Detected!!");
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.gameObject.name.Contains ("1")) {
					Debug.Log(GameController.levels); 
				}
			}
		}
	}
}

