using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {
	RaycastHit hit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(InputController.HasTouchBegan()){
			Ray ray = Camera.main.ScreenPointToRay(InputController.GetTouchPosition());
			if (Physics.Raycast(ray, out hit)) {
				if(hit.collider.gameObject.name == gameObject.name){
					Application.LoadLevel(0);
				}
			}
		}
	}
}
