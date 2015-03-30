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
	private PoolingSystem poolingSystem;
	public GameObject parent,clone;
	public Color frtHit1;
	public Color frtHit2;
	public Color frtHitSplash1;
	public Color frtHitSplash2;
	public GameObject particle,particleSplsh;
	public Vector3 prtclpos;
	public int val;
	Vector3 camPos;


	public Color RandomColor(Color color1, Color color2){
		return new Color (Random.Range (color1.r, color2.r), Random.Range (color1.g, color2.g), Random.Range (color1.b, color2.b));
	}

	void Start () {
		if(Application.loadedLevelName.Contains ("7") ) spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GiantbugFrtSpawns> ();
		else spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FruitSpawns> ();
		poolingSystem = PoolingSystem.Instance;

		dragged = false;
		burpy = GameObject.FindGameObjectWithTag ("Burpy").GetComponent<Burpy> ();
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController>();
		prtclpos = new Vector3 (0.0f, 4.0f, 0.0f);
		camPos = GameObject.Find ("Main Camera").transform.position;
	}
	void Awake(){
		if(Application.loadedLevelName.Contains ("7") ) spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GiantbugFrtSpawns> ();
		else spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FruitSpawns> ();
		poolingSystem = PoolingSystem.Instance;

		dragged = false;
		burpy = GameObject.FindGameObjectWithTag ("Burpy").GetComponent<Burpy> ();
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController>();

	}
	public void parseIntfromString(string s){
		//string s = other.name;
		string b = string.Empty;
		for (int i =0; i<s.Length; i++) {
			if (char.IsDigit (s [i])) b += s [i];
			if (b.Length > 0) val = int.Parse (b);
		}
	}

	public void setParticlesbrkn(){
		particle = PoolingSystem.Instance.InstantiateAPS ("prtclFrtHit", transform.position);
		particle.particleSystem.renderer.sortingLayerName = "UI";
		particleSplsh = particle.transform.GetChild (0).gameObject;
		particleSplsh.particleSystem.renderer.sortingLayerName = "UI";
		particle.particleSystem.startColor = controller.scoretext[val-1].color;
		particleSplsh.particleSystem.startColor =controller.splshC[val-1];
	}

	public void setParticles(){
		particle = PoolingSystem.Instance.InstantiateAPS ("prtclCocoHit", transform.position+prtclpos);
		particle.particleSystem.renderer.sortingLayerName = "UI";
		particle.particleSystem.startColor = controller.scoretext[val-1].color;
	}

	void OnTriggerEnter(Collider other){
		if (!other.name.Contains ("Game")) {
			if (other.name.Contains ("Fleak")) {
				if (hits > maxhits) {
					if(parent != null) parent.GetComponent<Spawn> ().full=false;
					spawns.full--;

					parseIntfromString (other.name);
					setParticlesbrkn();

					//particle.particleSystem.startColor = RandomColor (frtHit1, frtHit2);
					//particleSplsh.particleSystem.startColor = RandomColor(frtHitSplash1, frtHitSplash2);
					AddScore (other.name);
					FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_coconut", GameObject.Find("Main Camera").transform.position);
					other.gameObject.GetComponent<Bullet>().DecreaseBullets();
					PoolingSystem.DestroyAPS (other.gameObject);
					PoolingSystem.DestroyAPS (this.gameObject);
				}
				else{

					parseIntfromString (other.name);
					setParticles();
					//particle.particleSystem.startColor = RandomColor (frtHit1, frtHit2);
					other.rigidbody.AddForce (0, bounce, 0, ForceMode.Impulse);  // make the object bounce away
					other.collider.enabled=false;
					FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_coconut_break", camPos);
					//if(Application.loadedLevelName.Contains ("7")){ 
						clone = poolingSystem.InstantiateAPS ("frtCoconasBrkn", transform.position, transform.rotation,parent);
						clone.GetComponent<Coconut>().parent=parent;
					//}
					//else poolingSystem.InstantiateAPS ("frtCoconasBrkn", transform.position, transform.rotation);
					PoolingSystem.DestroyAPS (this.gameObject);

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
