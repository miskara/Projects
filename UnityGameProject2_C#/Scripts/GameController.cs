using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	RaycastHit hit;
	Ray ray;
	public enum GameDifficulty {Easy, Medium, Hard}
	public static int levels = 1;
	public static GameDifficulty diff = GameDifficulty.Easy;



	void Awake(){
		DontDestroyOnLoad (this);
	
	}
	void Start () {
		levels = PlayerPrefs.GetInt ("levels");
	}

	void Update () {
	
		if (InputController.HasTouchBegan ()) {
			Debug.Log ("Touch Detected!!");
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.gameObject.name.Contains ("Play")) {
					Application.LoadLevel (1); 
				}
			}
		}
	}
}