using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Burpy : MonoBehaviour {

	
	ScoreController controller;				// GameController
	public int pot;							// how big is the pot
	public int bounce;
	int hits;  								// bounce to add on the flea that hits
	public FruitSpawns spawns;
	float chance = 15f;
	public Transform mouth;
	public List<GameObject> eaten = new List<GameObject>();
	public bool flipped = false;
	public bool full = false;
	public bool ended;
	public ParticleSystem[] lpadParticles;
	public ParticleSystem frtRain;
	public ParticleSystem inhale;
	public ParticleSystem sweat;
	public Vector3[] rainrotations;
	FMOD.Studio.EventInstance burpyFull;
	FMOD.Studio.PLAYBACK_STATE  burpyFullPlaybackState;
	bool burpyFullPlaying = false;
	public Animator anim;
	public Renderer rndr;
	public bool semi;
	public bool animating;
	public Vector3 minScale = new Vector3(12f,12f,12f);
	public Vector3 maxScale = new Vector3(15f,15f,15f);
	public Vector3 scale;
	public List<GameObject> drawn = new List<GameObject>();
	Vector3 camPos;


	void Start () {
		hits = 0;
		ended = false;
		scale = new Vector3 (0f, 0f, 0f);
		transform.localScale = minScale;
		for (int i =0; i<4; i++){
			lpadParticles[i].Stop ();
			lpadParticles[i].Clear ();
			lpadParticles[i].particleSystem.renderer.sortingLayerName = "UI";
		}
		frtRain.Stop ();
		frtRain.Clear ();
		inhale.Stop ();
		inhale.Clear ();
		sweat.Stop ();
		sweat.Clear ();
		sweat.renderer.sortingLayerName = "UI";
		frtRain.renderer.sortingLayerName = "UI";
		inhale.renderer.sortingLayerName = "UI";
		rndr = GameObject.FindGameObjectWithTag ("Burpym").GetComponent<Renderer>();
		anim = GetComponent<Animator> ();
		bounce = 3000;
		mouth = GameObject.Find ("Mouth").transform;
		if(Application.loadedLevelName.Contains ("7")) spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GiantbugFrtSpawns> ();
		else spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FruitSpawns> ();
		controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ScoreController> ();	
		burpyFull = FMOD_StudioSystem.instance.GetEvent("event:/01_sfx/burpy_full");

		camPos = GameObject.Find ("Main Camera").transform.position;
	}

	public IEnumerator PlayAndStopV2(ParticleSystem ps){
		ps.Play ();
		yield return new WaitForSeconds (1);
		ps.Stop ();
		yield return new WaitForSeconds (ps.duration);
		ps.Clear ();
	}

	void Awake(){
		bounce = 3000;
		rndr = GameObject.FindGameObjectWithTag ("Burpym").GetComponent<Renderer>();
		mouth = GameObject.Find ("Mouth").transform;
		burpyFull = FMOD_StudioSystem.instance.GetEvent("event:/01_sfx/burpy_full");
	}

	void Gravitate(GameObject obj){
		obj.GetComponent<Fruit> ().dragged = true;
	}

	public IEnumerator Flip(){
		flipped = true;
		anim.SetBool("Flip",true);
		yield return new WaitForSeconds (7.5f);
		if(!ended) StartCoroutine ("FlipBack");
	}

	public IEnumerator FlipBack(){
		anim.SetBool ("Flip", false);
		yield return new WaitForSeconds (3.1f);
		flipped = false;
		if(!ended) Invoke ("DrawFruits", 1.5f);
	}

	public void End(){
		sweat.Stop ();
		sweat.Clear ();
		burpyFull.getPlaybackState (out burpyFullPlaybackState);
		if (burpyFullPlaybackState != FMOD.Studio.PLAYBACK_STATE.STOPPED) {
			burpyFull.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		ended = true;
		CancelInvoke ();
		for (int i =0; i<4; i++){
			if(lpadParticles[i]!=null){
			lpadParticles[i].Stop ();
			lpadParticles[i].Clear ();
			lpadParticles[i].particleSystem.renderer.sortingLayerName = "UI";
		}
		}
		if (frtRain != null) {
			frtRain.Stop ();
			frtRain.Clear ();
		}
		if (inhale != null) {
			inhale.Stop ();
			inhale.Clear ();
		}
		if(sweat!=null){
		sweat.Stop ();
			sweat.Clear ();}
	}
	public void StartFlip(){
		if(!full) StartCoroutine ("Flip");
	}

	public void checkPot(){
		burpyFull.getPlaybackState (out burpyFullPlaybackState);
		scale = new Vector3 (12f+3*(pot/150f),12f+3*(pot/150f),12f+3*(pot/150f));
		transform.localScale = scale;
		if (pot >= 1500) {
			sweat.Play ();
			sweat.emissionRate=20.0f;
			full = true;
			semi = false;
			chance = 100f;
			rndr.material.mainTexture = Resources.Load ("Textures/Characters/BurpyFull") as Texture;
			if (!flipped) {
				anim.SetBool ("Full",true);
				if (burpyFullPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) burpyFull.start ();
			}
		}

		else if (pot < 75) {
			sweat.Stop ();
			sweat.Clear ();
			full = false;
			semi = false;
			chance = 0.0f;
			rndr.material.mainTexture = Resources.Load ("Textures/Characters/burby_diffuse") as Texture;
			burpyFull.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			anim.SetBool ("Full",false);
		}

		else if (pot >= 75 && pot < 1500) {
			sweat.Play ();
			sweat.emissionRate=3.0f;
			rndr.material.mainTexture=Resources.Load ("Textures/Characters/burby_diffuse_halffull") as Texture;
			semi = true;
			full = false;
			anim.SetBool ("Full",false);
		}
	}


	//public void Update(){
	//	burpyFull.getPlaybackState (out burpyFullPlaybackState);
	//}

	public void DrawFruits(){
		if (!flipped && !full) {
			//anim.ResetTrigger ("Hit_Fail");
			//anim.ResetTrigger ("Hit");
			anim.SetBool ("Hit_Fail",false);
			anim.SetBool ("Hit",false);
			//anim.SetTrigger ("Suck");
			StartCoroutine (Animate ("Suck"));
			StartCoroutine (PlayAndStopV2(inhale));
			for (int i= 0; i<5; i++) {
				int j = Random.Range (0, spawns.fruits.Count);
				GameObject obj = spawns.fruits [j];
				drawn.Add (obj);
				Gravitate (obj);
			}
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_suck", camPos);
		}
	}
	public IEnumerator SemiHit(){
		rndr.material.mainTexture = Resources.Load ("Textures/Characters/burby_diffuse_hit") as Texture;
		yield return new WaitForSeconds (0.25f);
		rndr.material.mainTexture = Resources.Load ("Textures/Characters/burby_diffuse_halffull") as Texture;
		if (hits >= 3) {
			rndr.material.mainTexture = Resources.Load ("Textures/Characters/burby_diffuse_redface_b") as Texture;
		}
	}

	public void FleakHit(Collider fleak){
		fleak.gameObject.GetComponent<Bullet> ().HitBurpy();
		float x = Random.Range (0,51);
		if (semi && !flipped) {
			hits++;
			chance = chance + 16.67f;
			StartCoroutine (SemiHit());
		}
		fleak.rigidbody.AddForce (0, bounce, 0, ForceMode.Impulse);  // make the object bounce away

		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_boing", camPos);
		if (x > 100 - chance && !flipped) {													// 15% chance to get pot from Burpy	
			anim.SetTrigger("Hit");
			anim.SetBool ("Hit_Fail",false);
			anim.SetBool ("Suck",false);
			AddScore (fleak.name);
			StartCoroutine (GradualShrink());
			for(int i = 0; i<drawn.Count; i++){
				drawn[i].GetComponent<Fruit>().dragged=false;
			}
			//FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_clear", GameObject.Find ("Main Camera").transform.position);
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_clear", camPos);

		} else {
			if (!flipped) {
				//anim.ResetTrigger ("Hit");
				//anim.ResetTrigger ("Suck");
				//anim.ResetTrigger ("Hit_Fail");

				//anim.SetBool ("Hit",false);
				//anim.SetBool ("Hit_Fail",false);
				//anim.SetBool ("Suck",false);
				//anim.SetBool ("Hit_Fail",true);
				StartCoroutine(Animate ("Hit_Fail"));

				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_burp", camPos);
			}
		}
	}



	public IEnumerator Animate(string s){

		if (!animating) {
			animating = true;
			anim.SetBool (s,true);
			yield return null;
			//yield return new WaitForSeconds(anim.animation.clip.length);
			anim.SetBool (s,false);
			animating=false;
		}
	}
	
	public IEnumerator GradualShrink(){
		hits = 0;
		for (int i = 0; i<20; i++) {
			scale = Vector3.Lerp (maxScale, minScale, (float)i / 20f);
			transform.localScale = scale;
			yield return new WaitForSeconds (0.05f);
		}
		rndr.material.mainTexture = Resources.Load ("Textures/Characters/burby_diffuse") as Texture;

	}

	public void FruitSwallow(int amount,Collider other){
		if (other.gameObject.GetComponent<Fruit> ()) {
			eaten.Add (other.gameObject);
			other.gameObject.GetComponent<Fruit> ().dragged = false;
			pot = pot + amount;
			checkPot ();
			PoolingSystem.DestroyAPS (other.gameObject);
		}
	}

	//void OnCollisionEnter(Collision other){
	//	if (other.gameObject.name.Contains ("Fleak")) {
	//		FleakHit (other.collider);
	//	} 
	//	//else {
	//	//	if(other.gameObject.GetComponent<Fruit>()){
	//	//		FruitSwallow(other.gameObject.GetComponent<Fruit>().pot,other.collider);
	//	//		//PoolingSystem.DestroyAPS(other.gameObject);
	//	//	}
	//	//	
	//	//}
	//
	//}

	//void OnTriggerStay(Collider other) {
	//	if (other.name.Contains ("Fleak")) {
	//		FleakHit (other);
	//	} 
	//	//else {
	//	//	if(other.GetComponent<Fruit>()){
	//	//		FruitSwallow(other.GetComponent<Fruit>().pot, other);
	//	//	}
	//	//}
	//}

	void OnTriggerEnter(Collider other){
		if (other.name.Contains ("Fleak")) {
			FleakHit (other);
		} 
	}
	
	void AddScore(string s){
		for (int i=1; i<5; i++) {
			if (s.Contains (i.ToString())) {
				StartCoroutine(PlayAndStopV2(lpadParticles[i-1]));
				frtRain.transform.rotation = Quaternion.Euler (rainrotations[i-1]);
				StartCoroutine (PlayAndStopV2(frtRain));
				controller.AddScore (i-1,pot);
				eaten.Clear ();
				pot = 0;

			}
		}
		checkPot ();
		if (burpyFullPlaying) {
			burpyFull.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			burpyFull.release();
			burpyFullPlaying = false;
		}

	}
}
