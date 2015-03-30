using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FleakAnims : MonoBehaviour {


	public GameObject[] chars;
	Ray ray;
	RaycastHit hit;
	void Start () {
	
		chars = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject go in chars) {
			go.GetComponent<Animator> ().SetBool ("Revealed", true);
		}
	}

	public void Next(){
		if (Application.loadedLevel != 16) Application.LoadLevel (Application.loadedLevel + 1);
		else
			Application.LoadLevel (0);
	}


	public void Previous(){
		if (Application.loadedLevel != 13)
			Application.LoadLevel (Application.loadedLevel - 1);
		else
			Application.LoadLevel (0);
	}

	void Update () {
		if (Input.GetKey (KeyCode.Escape)) Application.LoadLevel(0);
		if (InputController.HasTouchBegan ()) {
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.name.Contains ("Nex")) {
					Next ();
				} else if (hit.collider.name.Contains ("Prev")) {
					Previous ();
				}
				else if(hit.collider.name.Contains("Burpy")){
					Application.LoadLevel("Level10");
				}
			}
		}
	//	if (Input.GetKey (KeyCode.Escape)) {
	//		if(Application.loadedLevelName.Contains ("Fleak")) Application.LoadLevel ("JoejoeDances");
	//		else if (Application.loadedLevelName.Contains ("Joejoe")) Application.LoadLevel ("MorphyDances");
	//		else if(Application.loadedLevelName.Contains ("Morphy")) Application.LoadLevel ("Specials");
	//		else if(Application.loadedLevelName.Contains("Specials")) Application.LoadLevel ("MemoryMenuMain");
	//	}
	//	if(Input.GetKey(KeyCode.Menu)){
	//		if(Application.loadedLevelName.Contains ("Fleak")) Application.LoadLevel ("JoejoeDances");
	//		else if (Application.loadedLevelName.Contains ("Joejoe")) Application.LoadLevel ("MorphyDances");
	//		else if(Application.loadedLevelName.Contains ("Morphy")) Application.LoadLevel ("Specials");
	//		else if(Application.loadedLevelName.Contains("Specials")) Application.LoadLevel ("MemoryMenuMain");
	//
	//	}


	
	}
}
