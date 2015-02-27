using UnityEngine;
using System.Collections;

public class Spotmato: MonoBehaviour {

	public Burpy burpy;
	public ScoreController controller;				// GameController
	public int pot;							// how big is the pot
	public FruitSpawns spawns;
	public bool dragged;

	
	void Start () {
		pot = 10;
		dragged = false;
		spawns = GameObject.Find ("Fruitspawns").GetComponent<FruitSpawns> ();
		burpy = GameObject.FindGameObjectWithTag ("Burpy").GetComponent<Burpy> ();
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController>();
	}
	

	void OnTriggerEnter(Collider other){
		if (!other.name.Contains ("Game")) {
			if (other.gameObject.tag.Contains ("Player")) {
				AddScore (other.name);
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_pointcollect", transform.position);
				spawns.removeFruit (this.gameObject);
				PoolingSystem.DestroyAPS (this.gameObject);
				PoolingSystem.DestroyAPS (other.gameObject);
			} 
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
