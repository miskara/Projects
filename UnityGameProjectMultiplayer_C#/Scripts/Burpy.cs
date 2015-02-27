using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Burpy : MonoBehaviour {

	
	ScoreController controller;			// GameController
	public int pot;						// how big is the pot
	public int bounce;					// bounce to add on the flea that hits
	public FruitSpawns spawns;
	public Transform mouth;
	public List<GameObject> eaten = new List<GameObject>();
	public float spinSpeed;
	public bool gameStart;
	public Timer timer;
	public List<GameObject> players;
	int val;

	FMOD.Studio.EventInstance burpyFull;
	bool burpyFullPlaying = false;

	void Start () {
		timer = GameObject.FindGameObjectWithTag ("Timer").GetComponent<Timer> ();
		gameStart = false;
		bounce = 3000;
		mouth = GameObject.Find ("Mouth").transform;
		pot = 100;
		spawns = GameObject.Find ("Fruitspawns").GetComponent<FruitSpawns> ();
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController> ();
		//InvokeRepeating ("DrawFruits", 10.0f, 10.0f);
		burpyFull = FMOD_StudioSystem.instance.GetEvent("event:/02_music/burpy_full");
	}
	/*void Update(){
	
		transform.Rotate(Vector3.up * 50 * Time.deltaTime);
	}*/

	void Gravitate(GameObject obj){
		Ray dir = new Ray(mouth.position, obj.transform.position);
		float dist = Vector3.Distance(obj.transform.position, mouth.position);
		obj.rigidbody.AddForce((-dir.direction)*(5 * dist));
		spawns.fruits.Remove (obj);
	}

	public void Update(){
		if(!gameStart){
			Ray ray;
			RaycastHit hit;
			if (InputController.HasTouchBegan ()) {
				ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.name.Contains ("Burpy")) {
						timer.InvokeRepeating ("decreaseTimeRemaining", 0.0f, 1.0f);
						InvokeRepeating("DrawFruits", 10.0f, 10.0f);
						spawns.InvokeRepeating ("SpawnFruits", 1.0f, 1.0f);
						gameStart=true;
						for(int i=0; i<players.Count; i++){
							players[i].GetComponent<TouchDragPowerV2>().gamestart = true;
							//players[i].GetComponent<TouchDragPowerV2>().FirstFleak();
						}
					}
					else if (hit.collider.name.Contains("LaunchPad")){
						Debug.Log (hit.collider.name);
						string a = hit.collider.name;
						string b = string.Empty;
						for (int i =0; i<a.Length; i++){
							if(char.IsDigit(a[i])) b += a[i];
							if(b.Length>0) val= int.Parse(b);
						}
						if(players.Contains(GameObject.Find ("Fleak"+val))){
							GameObject go = GameObject.Find ("Fleak"+val);
							go.GetComponent<TouchDragPowerV2>().rmFirstFleak();
							players.Remove (GameObject.Find ("Fleak"+val));

						}
						else{
							GameObject go = GameObject.Find ("Fleak"+val);
							players.Add(go);
							go.GetComponent<TouchDragPowerV2>().FirstFleak();

						}
			
					}
					else { Debug.Log (hit.collider.name);}
				}
		}
		}
	}
	
	public void DrawFruits(){
		if (pot > 400) {
			if (!burpyFullPlaying) {
				burpyFull.start();
				burpyFullPlaying = true;
			}
		}
		else {
			if (burpyFullPlaying) {
				burpyFull.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				burpyFull.release();
				burpyFullPlaying = false;
			}

			for (int i= 0; i<5; i++) {
				int j = Random.Range (0,spawns.fruits.Count);
				GameObject obj = spawns.fruits[j];
				Gravitate (obj);
			}
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_suck", transform.position);
		}
	}


	public void FleakHit(Collider fleak){
		float x = Random.Range (0,101);
		fleak.rigidbody.AddForce (0, bounce, 0, ForceMode.Impulse);  // make the object bounce away
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_boing", transform.position);
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_burp", transform.position);
		if (x > 85) {													// 15% chance to get pot from Burpy	
			AddScore (fleak.name);
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_clear", transform.position);

		}
	}


	public void FruitSwallow(int amount){
		pot = pot + amount;

	}

	void OnTriggerStay(Collider other) {
		if (!other.name.Contains ("Game")) {
			if (other.name.Contains ("Fleak")) {
				FleakHit (other);
			}
			else {
				PoolingSystem.DestroyAPS (other.gameObject);

			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.name.Contains ("Fleak")) {
			FleakHit (other);
		} 
		else {
			if(other.GetComponent<Spotmato>()){
				FruitSwallow(other.GetComponent<Spotmato>().pot);
				eaten.Add (other.gameObject);
			}
			else if(other.GetComponent<Nectaberry>()){
				FruitSwallow(other.GetComponent<Nectaberry>().pot);
				eaten.Add (other.gameObject);
			}
			else if(other.GetComponent<Pinkly>()){
				FruitSwallow(other.GetComponent<Pinkly>().pot);
				eaten.Add (other.gameObject);
			}
			//else{ 
			//	FruitSwallow (other.GetComponent<Coconut>().pot);
			//	eaten.Add(other.gameObject);
			//}
		}
	}
	
	void AddScore(string s){
		for (int i=1; i<5; i++) {
			if (s.Contains (i.ToString())) {	
				controller.AddScore(i-1,pot);
				pot =20;
			}
		}

		if (burpyFullPlaying) {
			burpyFull.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			burpyFull.release();
			burpyFullPlaying = false;
		}
	}
}
