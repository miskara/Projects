using UnityEngine;
using System.Collections;

public class Coconut : MonoBehaviour {

	public int hits,maxhits;
	public ScoreController controller;				// GameController
	public int pot;						// how big is the pot
	public int bounce;
	public Burpy burpy;
	public FruitSpawns spawns;
	public bool dragged;
	
	void Start () {
		pot = 50;
		dragged = false;
		spawns = GameObject.Find ("Fruitspawns").GetComponent<FruitSpawns> ();
		burpy = GameObject.FindGameObjectWithTag ("Burpy").GetComponent<Burpy> ();
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController>();
	}
	
	
	void OnTriggerEnter(Collider other){
		if (!other.name.Contains ("Game")) {
			if (other.gameObject.tag.Contains ("Player")) {
				hits++;
				if (hits == maxhits) {
					AddScore (other.name);
					FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_coconut_break", transform.position);
					PoolingSystem.DestroyAPS (this.gameObject);
					PoolingSystem.DestroyAPS (other.gameObject);
				}
				else{ 
					other.rigidbody.AddForce (0, bounce, 0, ForceMode.Impulse);  // make the object bounce away
					FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_coconut", transform.position);
				}
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
