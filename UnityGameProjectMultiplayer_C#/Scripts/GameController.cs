using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameController : MonoBehaviour {
	
	
	//ScoreController controller;					// GameController
	public FruitSpawns spawns;
	public bool gameStart;
	public Timer timer;
	public List<GameObject> players;
	public List<GameObject> inactive;
	int val;
	public bool playhidden = false;
	Ray ray;
	RaycastHit hit;
	public bool paused;
	//private string choose = "Select players by touching their launchpads! \n Press button on left to start the game";
	public SpriteRenderer pause;
	public Sprite sprtPause;
	public GameObject playWatch;
	//Vector3 playScale = new Vector3(2f,4f,2f);
//	Vector3 timeScale = new Vector3(8f,4f,7f);
	Vector3 scale;
	public GameObject PauseMenu;
	public GameObject clock;
	public Wobbler wobl;

	Vector3 hide = new Vector3 (0f, 0f, 0f);
	Vector3 max = new Vector3(4f,4f,2f);

	//Vector3 rdypos = new Vector3 (1.43f, 22f, 1.98f);
	//Vector3 hiddenpos = new Vector3 (1.43f,-55.0f,1.98f);
	
	
	void Start () {
		wobl = playWatch.GetComponent<Wobbler> ();
		playhidden = true;
		playWatch.transform.localScale = hide;
		for(int i = 1; i<5; i++){
			inactive.Add (GameObject.Find ("Player" + i));
		}
		timer = GameObject.FindGameObjectWithTag ("Timer").GetComponent<Timer> ();
		gameStart = false;
		if(Application.loadedLevelName.Contains ("7") ) {spawns = GameObject.Find ("Fruitspawns").GetComponent<GiantbugFrtSpawns>();}
		else{spawns = GameObject.Find ("Fruitspawns").GetComponent<FruitSpawns> ();}
		//spawns = GameObject.Find ("Fruitspawns").GetComponent<FruitSpawns> ();
		//controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController> ();	
	}
	public IEnumerator PlayAndStopV2(ParticleSystem ps){
		ps.Play ();
		yield return new WaitForSeconds (1);
		ps.Stop ();
		yield return new WaitForSeconds (ps.duration);
		ps.Clear ();
	}

	public void Pause(){
		if (!paused) {
			PauseMenu.SetActive(true);
			Time.timeScale = 0.0F;
			paused = !paused;
		} else {
			PauseMenu.SetActive(false);
			Time.timeScale = 1.0f;
			paused = !paused;
		}
	}
	
	void Awake(){
		timer = GameObject.FindGameObjectWithTag ("Timer").GetComponent<Timer> ();
		gameStart = false;
		if(Application.loadedLevelName.Contains ("7") ) {spawns = GameObject.Find ("Fruitspawns").GetComponent<GiantbugFrtSpawns>();}
		else{spawns = GameObject.Find ("Fruitspawns").GetComponent<FruitSpawns> ();}

	}
	
	public void Update(){

		if (!gameStart) {
			if (InputController.HasTouchBegan ()) {
				ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.name.Equals (playWatch.name)) {
						if (players.Count > 0) {
							playWatch.SetActive (false);
							clock.SetActive (true);
							pause.sprite = sprtPause;
							timer.decreaseTimeRemaining ();
							if (Application.loadedLevelName.Contains ("4"))
								timer.BurpyInvokes ();
							switch (players.Count) {
							case 1:
								/*if(Application.loadedLevelName.Contains ("7")) gbspwns.InvokeRepeating ("SpawnFruits", 0.0f, 2.0f);
								else*/
								spawns.InvokeRepeating ("SpawnFruits", 0.0f, 1.5f);
								spawns.repeatrate = 1.5f;

								break;
							case 2:
								/*if(Application.loadedLevelName.Contains ("7")) gbspwns.InvokeRepeating ("SpawnFruits", 0.0f, 1.66f);
								else*/
								spawns.InvokeRepeating ("SpawnFruits", 0.0f, 1f);
								spawns.repeatrate = 1.0f;
								break;
							case 3:
								/*if(Application.loadedLevelName.Contains ("7")) gbspwns.InvokeRepeating ("SpawnFruits", 0.0f, 1.0f);
								else*/
								spawns.InvokeRepeating ("SpawnFruits", 0.0f, 0.7f);
								spawns.repeatrate =0.7f;
								break;
							case 4:
								/*if(Application.loadedLevelName.Contains ("7")) gbspwns.InvokeRepeating ("SpawnFruits", 0.0f, 0.42857142857f);
								else*/
								spawns.InvokeRepeating ("SpawnFruits", 0.0f, 0.42857142857f);
								spawns.repeatrate = 0.42857142857f;
								break;
							}

							gameStart = true;
							for (int i=0; i<players.Count; i++) {
								players [i].GetComponent<TouchDragPowerV2> ().StartGame ();
							}
							for(int i = 0; i<inactive.Count; i++){
								inactive[i].SetActive(false);
							}
						}
						//else timer.time.text=choose+"\nYou must choose atleast 1 player!!";
					}
					else if (hit.collider.name.Contains("LaunchPad")){
						FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/ui_tap_2", Vector3.zero);
						string a = hit.collider.name;
						string b = string.Empty;
						for (int i =0; i<a.Length; i++) {
							if (char.IsDigit (a [i]))
								b += a [i];
							if (b.Length > 0)
								val = int.Parse (b);
						}
						if (players.Contains (GameObject.Find ("Player" + val))) {
							GameObject go = GameObject.Find ("Player" + val);
							go.GetComponent<TouchDragPowerV2> ().rmFirstFleak ();
							inactive.Add (go);
							players.Remove (GameObject.Find ("Player" + val));
							if(players.Count < 1 && playhidden==false){
								StopAllCoroutines();
								StartCoroutine (PlayButtonHide());
							}
							
						} 

						else {
							GameObject go = GameObject.Find ("Player" + val);
							players.Add (go);
							inactive.Remove (go);
							if(players.Count == 1){ 
								StopAllCoroutines();
								StartCoroutine (PlayButtonrdy());
							}
							go.GetComponent<TouchDragPowerV2> ().FirstFleak ();

							
						}
					}
				}
			}
		}
		else {
			if (InputController.HasTouchBegan ()) {
				ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.name.Contains ("time")) {
						Pause();
					}
				}
			}
		}
	}

	public IEnumerator PlayButtonHide(){
		if (playhidden == false) {
			wobl.wobble=false;
			wobl.sway=false;
			for (int i = 0; i<31; i++) {
				scale = Vector3.Lerp (max, hide, (float)i / 30f);
				playWatch.transform.localScale = scale;
				yield return new WaitForSeconds (0.0033f);
			}
			playhidden = true;


		}
	}

	public IEnumerator PlayButtonrdy(){
		if (playhidden==true){
			for (int i = 0; i<31; i++) {
				scale = Vector3.Lerp (hide, max, (float)i / 30f);
				playWatch.transform.localScale = scale;
				yield return new WaitForSeconds (0.0033f);
			}
			playhidden = false;
			wobl.wobble=true;
			wobl.sway=true;
		}
	}
}