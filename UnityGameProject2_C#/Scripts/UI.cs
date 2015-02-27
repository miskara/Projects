using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UI : MonoBehaviour
{
	public  List<string> levels =new List<string>();
	public enum State {InGame}
	public State state = State.InGame;
	int score;

	public GameObject playmat;

	public GUIStyle style;
	public int level;
	public int currLevel;


	IEnumerator WaitForPlaymatToLoad(){
		yield return new WaitForSeconds(0.0f);
		Playmat.GetPlaymat().CreateLayout(Playmat.GetPlaymat().board.name);
	}

	public void OnGUI(){
	
	switch (state) {
		case State.InGame:
			Ingame ();
			break;
		}
	
	}


	void Start(){
		level = PlayerPrefs.GetInt ("Level");
		currLevel = PlayerPrefs.GetInt ("CurrentLevel");
		GameController.levels = PlayerPrefs.GetInt ("Level");
		Instantiate(playmat);
		StartCoroutine(WaitForPlaymatToLoad());
	}


	void Update(){

		if (Input.GetKey (KeyCode.Escape)) {
			PlayerPrefs.SetInt ("Level",1);
			GameController.levels = PlayerPrefs.GetInt ("Level");
			PlayerPrefs.SetInt ("World",1);
			PlayerPrefs.Save ();
		}
		if(Input.GetKey(KeyCode.Menu)){
			PlayerPrefs.Save ();
		}

	}

	void Ingame(){

		GUI.Label(new Rect(Screen.width/2 - 50, 25,100,20),"Score: " + Playmat.GetPlaymat().GetPointsString(), style);
		if(GUI.Button(new Rect(0,0,100,30),"Quit",style)){
			Destroy(Playmat.GetPlaymat().gameObject);
			Application.LoadLevel (0);
		}

		if(Playmat.GetPlaymat().gameWon){
			foreach(Card c in Playmat.GetPlaymat().all){
				c.fadeOut ();
			}
			PlayerPrefs.SetInt ("Level",currLevel+1);
			if(GUI.Button(new Rect(Screen.width*0.425f,Screen.height*0.31f,Screen.width*0.15f,Screen.height*0.15f),"Next Level?", style)){
				Destroy(Playmat.GetPlaymat().gameObject);
				Application.LoadLevel("Level"+(level+1));
			}
			if(GUI.Button(new Rect(Screen.width*0.425f,Screen.height/2 + 35,Screen.width*0.15f,Screen.height*0.15f),"Main Menu?", style)){
				Destroy(Playmat.GetPlaymat().gameObject);
				Application.LoadLevel (0);
			}
		}
	}

}
