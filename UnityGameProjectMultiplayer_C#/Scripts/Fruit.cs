using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fruit: MonoBehaviour {

	public Burpy burpy;
	public ScoreController controller;				// GameController
	public int pot;							// how big is the pot
	public FruitSpawns spawns;
	public bool dragged;
	public GameObject particle,particleSplsh;
	public Color frtHit1;
	public Color frtHit2;
	public Color frtHitSplash1;
	public Color frtHitSplash2;
	public GameObject parent;
	public Vector3 burpyMouth;
	public int val;
	Vector3 camPos;
	
	void Start () {
		dragged = false;
		if(Application.loadedLevelName.Contains ("7") ) spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GiantbugFrtSpawns> ();
		else spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FruitSpawns> ();
		//while (burpy=null) {
			burpy = GameObject.FindGameObjectWithTag ("Burpy").GetComponent<Burpy> ();
		//}
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController>();
		camPos = GameObject.Find ("Main Camera").transform.position;
	}

	void Awake(){
		dragged = false;
		if(Application.loadedLevelName.Contains ("7")) spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GiantbugFrtSpawns> ();
		else spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FruitSpawns> ();
			burpy = GameObject.FindGameObjectWithTag ("Burpy").GetComponent<Burpy> ();
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController>();

	}

	public void MoveTowards(){
		//the speed, in units per second, we want to move towards the target
		float speed = 30f;
		//move towards the center of the world (or where ever you like)
		//Vector3 targetPosition = burpyMouth.transform.position;
		
		Vector3 currentPosition = this.transform.position;
		//first, check to see if we're close enough to the target
		if (Vector3.Distance (currentPosition, burpyMouth) > .5f) { 
			Vector3 directionOfTravel = burpyMouth - currentPosition;
			//now normalize the direction, since we only want the direction information
			directionOfTravel.Normalize ();
			//scale the movement on each axis by the directionOfTravel vector components
			this.transform.Translate (
				(directionOfTravel.x * speed * Time.deltaTime),
				(directionOfTravel.y * speed * Time.deltaTime),
				(directionOfTravel.z * speed * Time.deltaTime),
				Space.World);
		} 
		else {
			burpy.FruitSwallow(pot,this.collider);
			dragged=false;
			if (parent != null) {
				parent.GetComponent<Spawn> ().full = false;
				spawns.full--;
			}

		
		}

	}

	public void Update(){
		if (dragged) {
			MoveTowards();
		}
	}

	public Color RandomColor(Color color1, Color color2){
		return new Color (Random.Range (color1.r, color2.r), Random.Range (color1.g, color2.g), Random.Range (color1.b, color2.b));
	}
	
	public void FleakCollided(string s){
		if (parent != null) {
			parent.GetComponent<Spawn> ().full = false;
		}
		spawns.full--;
		particle = PoolingSystem.Instance.InstantiateAPS ("prtclFrtHit", transform.position);
		particleSplsh = particle.transform.GetChild (0).gameObject;
		particle.particleSystem.renderer.sortingLayerName = "UI";
		particleSplsh.particleSystem.renderer.sortingLayerName = "UI";
		//particle.particleSystem.startColor = RandomColor (frtHit1, frtHit2);
		//particleSplsh.particleSystem.startColor = RandomColor(frtHitSplash1, frtHitSplash2);
		//string a = s;
		string b = string.Empty;
		for (int i =0; i<s.Length; i++) {
			if (char.IsDigit (s [i])) b += s [i];
			if (b.Length > 0) val = int.Parse (b);
		}
		particle.particleSystem.startColor = controller.scoretext[val-1].color;
		particleSplsh.particleSystem.startColor = controller.splshC[val-1];
		AddScore (s);
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_pointcollect", camPos);
		spawns.removeFruit (this.gameObject);
		PoolingSystem.DestroyAPS (this.gameObject);
	}

	public void OnTriggerEnter(Collider other){
		if (other.name.Contains ("Fleak")) {
			FleakCollided (other.name);
		} else if (other.name.Contains ("Burpy")) {
			burpy.FruitSwallow(pot,other);	
		}
	}

	public void OnCollisionEnter(Collision other){
		if (other.collider.name.Contains ("Fleak")) {
			FleakCollided (other.collider.name);
		} else if (other.collider.name.Contains ("Burpy")) {
			burpy.FruitSwallow(pot,other.collider);	
		}
	}

	void AddScore(string s){
		for (int i=1; i<5; i++) {
			if (s.Contains (i.ToString())) {	
				controller.AddScore(i-1,pot);
			}
		}
	}
}
