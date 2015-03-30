using UnityEngine;
using System.Collections;

public class EndMenu : MonoBehaviour {
	Ray ray;
	RaycastHit hit;
	public GameObject replay;
	public GameObject home;
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		if (InputController.HasTouchBegan ()) {
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.name.Equals (replay.name)) {
					Application.LoadLevel(Application.loadedLevel);
				}
				else if(hit.collider.name.Equals (home.name)){
					Application.LoadLevel (0);
				}
				
			}
		}
		
	}
}
