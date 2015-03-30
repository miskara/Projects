using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
	public Text scoretime;
	Ray ray;
	RaycastHit hit;
	public  List<string> levels =new List<string>();
	public enum State {InGame}
	public State state = State.InGame;
	int score;

	public GameObject playmat;

	public GUIStyle style;
	public int world;
	public int currWorld;
	public int level;
	public int currLevel;
	public ParticleSystem victory;
	public GameObject vicObj;
	public bool won;
	public Camera camera;
	public GameObject camobj;
	//public Transform camera;
	public Vector3 vicpos;
	public GameTimer timer;
	public GameObject sounds;

	FMOD.Studio.Bus masterBus;
	
	public Sprite spriteSoundOff;
	public Sprite spriteSoundOn;

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
		scoretime = GameObject.Find ("Text").GetComponent<Text>();
		sounds = GameObject.Find ("Mute");
		vicObj = Resources.Load ("Prefabs/effects/prtclVictory") as GameObject;
		timer = GameObject.Find ("Timer").GetComponent<GameTimer> ();
		camobj = GameObject.FindGameObjectWithTag ("MainCamera");
		camera = camobj.GetComponent<Camera> ();
		vicObj.transform.localScale = new Vector3 (1f, 1f, 1f) * (camera.orthographicSize / (5.5f));
		victory = vicObj.GetComponent<ParticleSystem> ();
		camera.nearClipPlane = 0.0f;
		vicpos = camera.transform.position + new Vector3 (0f, -10f, 0f);
		Instantiate(playmat);
		StartCoroutine(WaitForPlaymatToLoad());
		timer.StartTimer ();

		FMOD.Studio.System system = FMOD_StudioSystem.instance.System;
		system.getBus("bus:/", out masterBus);
		
		if (!PlayerPrefs.HasKey("muted")){
			PlayerPrefs.SetInt ("muted", 0);
		}
		
		if (PlayerPrefs.GetInt ("muted") == 1) {
			masterBus.setMute (true);
			sounds.GetComponent<SpriteRenderer>().sprite = spriteSoundOff;
		} else if (PlayerPrefs.GetInt ("muted") == 0) {
			masterBus.setMute (false);
			sounds.GetComponent<SpriteRenderer>().sprite = spriteSoundOn;
		}
		scoretime.text = "Score: " + playmat.GetComponent<Playmat> ().Points +"/" + playmat.GetComponent<Playmat> ().TotalPoints.ToString();
	}


	void Update(){
		if (Input.GetKey (KeyCode.Escape)) {
			Application.LoadLevel (0);
		}
		//	PlayerPrefs.SetInt ("Level",1);
		//	GameController.currentLVL = PlayerPrefs.GetInt ("Level");
		//	PlayerPrefs.SetInt ("World",1);
		//	PlayerPrefs.Save ();
		//}

		if (InputController.HasTouchBegan ()) {
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.name.Contains ("Home")) {
					Destroy(Playmat.GetPlaymat().gameObject);
			
					Application.LoadLevel(0);


				} else if (hit.collider.name.Contains ("Replay")) {
					Destroy(Playmat.GetPlaymat().gameObject);
					if(PlayerPrefs.GetInt ("CurrentLevel") > PlayerPrefs.GetInt ("Level")) PlayerPrefs.SetInt ("Level",PlayerPrefs.GetInt ("CurrentLevel"));
					Application.LoadLevel (Application.loadedLevel);


				} else if (hit.collider.name.Contains ("Mute")) {
					mute ();
				}
				else if (hit.collider.name.Contains("Next")){
					if(Application.loadedLevelName.Equals ("Level9")){
						Application.LoadLevel(19);
					}
					else{
						PlayerPrefs.SetInt ("CurrentLevel",PlayerPrefs.GetInt ("CurrentLevel")+1);
						Destroy(Playmat.GetPlaymat().gameObject);
						Application.LoadLevel("Level"+(PlayerPrefs.GetInt ("CurrentLevel")));
					}
				}
			}

		}
	}
	public void updateScore(){
		scoretime.text = "Score: " + Playmat.GetPlaymat ().GetPointsString ();
	}

	public void ShowTime(){
	
		scoretime.text = "Time: " + PlayerPrefs.GetString ("LevelTime");
	
	}
	public void mute () {
		if (PlayerPrefs.GetInt ("muted") == 1) { // UNMUTE
			masterBus.setMute (false);
			PlayerPrefs.SetInt ("muted", 0);
			sounds.GetComponent<SpriteRenderer>().sprite = spriteSoundOn;
		} 
		else { // MUTE
			masterBus.setMute (true);
			PlayerPrefs.SetInt ("muted", 1);
			sounds.GetComponent<SpriteRenderer>().sprite = spriteSoundOff;
		}
	}

	void Ingame(){

		//if (!won)
		//	GUI.Label (new Rect (Screen.width / 4 + 75, 0, Screen.width * 0.25f, Screen.height * 0.12f), "Score: " + Playmat.GetPlaymat ().GetPointsString (), style);
		//else GUI.Label (new Rect (Screen.width / 4 + 75, 0, Screen.width * 0.25f, Screen.height * 0.12f), "Time: " + PlayerPrefs.GetString("LevelTime"), style);
		//if(GUI.Button(new Rect(0,0,Screen.width*0.15f,Screen.height*0.12f),"Quit",style)){
		//	Destroy(Playmat.GetPlaymat().gameObject);
		//	Application.LoadLevel (0);
		//}

		if(Playmat.GetPlaymat().gameWon){
			if(PlayerPrefs.GetInt ("CurrentLevel"+1) > PlayerPrefs.GetInt ("Level")) PlayerPrefs.SetInt ("Level",PlayerPrefs.GetInt ("CurrentLevel"+1));
			if(!won){
				timer.StopTimer ();
				Instantiate(victory,vicpos,victory.transform.rotation);
				won=true;
			}
			foreach(Card c in Playmat.GetPlaymat().all){
				c.fadeOut ();
			}

			//TODO: These buttons are images from ICONS texture map
			//if(GUI.Button(new Rect(Screen.width*0.425f,Screen.height*0.25f,Screen.width*0.15f,Screen.height*0.15f),"Play again!", style)){
			//	if(PlayerPrefs.GetInt ("CurrentLevel") > PlayerPrefs.GetInt ("Level")) PlayerPrefs.SetInt ("Level",PlayerPrefs.GetInt ("CurrentLevel"));
			//	Destroy(Playmat.GetPlaymat().gameObject);
			//	Application.LoadLevel("Level"+(PlayerPrefs.GetInt ("CurrentLevel")));
			//}

			//if(GUI.Button(new Rect(Screen.width*0.425f,Screen.height*0.5f,Screen.width*0.15f,Screen.height*0.15f),"Next Level", style)){
			//	if(Application.loadedLevelName.Equals ("Level9")){
			//		Application.LoadLevel(19);
			//	}
			//	else{
			//	PlayerPrefs.SetInt ("CurrentLevel",PlayerPrefs.GetInt ("CurrentLevel")+1);
			//	Destroy(Playmat.GetPlaymat().gameObject);
			//	Application.LoadLevel("Level"+(PlayerPrefs.GetInt ("CurrentLevel")));
			//	}
			//}

			//if(GUI.Button(new Rect(Screen.width*0.425f,Screen.height*0.75f,Screen.width*0.15f,Screen.height*0.15f),"Main Menu", style)){
			//	Destroy(Playmat.GetPlaymat().gameObject);
			//	Application.LoadLevel (0);
			//}
		}
	}

}
