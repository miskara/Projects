using UnityEngine;
using System.Collections;

public class TouchDragPowerV2: TouchLogicV2{	

	public int player;								// playerID
	public int maxBullets = 10;
	public int bullets=0;
	public GameObject clone;						// used to clone the spawnable flea-bullet
	public GameObject cube;							// the launchable object
	private PoolingSystem poolingSystem;			// the instance pool;
	public SpriteRenderer launchpad;
	GUITexture myTex;								// player area
	Vector2 touchStart;								// touch position begin
	public Color orig;
	public float maxpower,minpower;
	public static bool gameEnded;					// is the game ended?
	private bool touched = false;					// is this player area already touched?
	//bool overHeat = false;
	public bool launchpadReady = true;
	public bool gamestart;
	public bool gameEnd;
	public Transform spawn;							// spawn point for the object
	public float destroyTime = 5.0f;				// time in which the object will disappear from the scene
	public float fmultiply/* = 1f*/;					// multiplier to make the object speed more pleasant
	private float holdTime = 0.0f;					// how long the player has touched the screen (affects the power)
	public float power;								// total power
	public float gravity;
	public float overheatingMeter;
	public float upvectorpower = 150f;
	public ParticleSystem prtclOverHeat;
	public ParticleSystem lpadRdy;

	FMOD.Studio.EventInstance launchEvent;
	FMOD.Studio.ParameterInstance launchParam;
	Vector3 camPos;

	public void StartGame(){
		gamestart = true;
	}

	void Start (){
		prtclOverHeat.Stop ();
		prtclOverHeat.particleSystem.renderer.sortingLayerName = "UI";
		lpadRdy.Stop ();
		lpadRdy.particleSystem.renderer.sortingLayerName = "UI";
		gamestart = false;
		overheatingMeter = 1.0f;
		gameEnded = false;
		poolingSystem = PoolingSystem.Instance;
		cube = Resources.Load ("Prefabs/Fleak" + player) as GameObject;
		Physics.gravity = new Vector3 (0f, -70, 0f);
		myTex = this.guiTexture;
		gameEnded = false;
		orig = launchpad.renderer.material.color;
		launchpad.renderer.material.SetColor("_Color",Color.Lerp(orig,Color.black,0.5f));

		launchEvent = FMOD_StudioSystem.instance.GetEvent ("event:/01_sfx/flea_launch");
		launchEvent.getParameter("launch", out launchParam);
		camPos = GameObject.Find ("Main Camera").transform.position;


	}


	public IEnumerator PlayAndStopV2(ParticleSystem ps){
		ps.Play ();
		yield return new WaitForSeconds (1);
		ps.Stop ();
		yield return new WaitForSeconds (ps.duration);
		ps.Clear ();
	}

	public void FirstFleak(){
		clone = poolingSystem.InstantiateAPS (cube.name, spawn.position, spawn.rotation);
		launchpad.renderer.material.SetColor("_Color",Color.Lerp(orig,Color.black,0f));
	}
	public void rmFirstFleak(){
		PoolingSystem.DestroyAPS (clone);
		launchpad.renderer.material.SetColor("_Color",Color.Lerp(orig,Color.black,0.5f));
	}

	public void ScoreScreen (){
		Ray ray;
		RaycastHit hit;
		if (InputController.HasTouchBegan ()) {
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.name.Contains ("Control")) {
					Application.LoadLevel ("menu");
					gameEnded = false;
					PoolingSystem.Instance.DeactiveAll ();
				}
			}
		}
	}

	public IEnumerator BulletCooldown(){
		launchpadReady = false;
		prtclOverHeat.Play ();
		CancelInvoke ("Cooldown");
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/launchpad_overheat", camPos);
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/launchpad_cooldown", camPos);
		for (int i = 0; i < 5; i++) {
			launchpad.renderer.material.SetColor("_Color",Color.Lerp(Color.black,orig,(float)i/(5)));
			yield return new WaitForSeconds(1.0f);
		}
		//yield return new WaitForSeconds (5.0f);
		launchpadReady = true;
		StartCoroutine (PlayAndStopV2(lpadRdy));
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/launchpad_ready", camPos);
		prtclOverHeat.Stop ();
		InvokeRepeating ("Cooldown",0f,0.15f);
	}

	public void Cooldown(){
		launchpad.renderer.material.SetColor("_Color",Color.Lerp(orig,Color.black,(float)bullets/(maxBullets+1)));
	}

	public override void OnTouchBegan (){
				// when there's a touch on the screen
		if (!gameEnded && gamestart) {
			if (myTex.HitTest (currPos) && !touched) {				// check if it hits this player area and the area is not already touched
				if (clone != null)
					startTouch ();
			}
		}
		else if (gameEnded) {
			ScoreScreen ();
		}
		
	}

	void startTouch (){
		holdTime = 0.0f;														// reset the time of the touch
		holdTime += Time.deltaTime;												// increase the time by the time of the frame
		touchStart = currPos;        											// get the position of the touch
		power = 0.0f;															// set current power to 0.0f
		touched = true;															// set the player area touched																		
	}

	public override void OnTouchStayed (){
		holdTime += Time.deltaTime;
	}
	
	public override void OnTouchMoved (){											// when the touch moves on the screen
		if (myTex.HitTest (currPos) && touched) {									// check if it hits the player area and the touch has began on this area
			holdTime += Time.deltaTime;												// increase the time of touch
			if (!myTex.HitTest (currPos)) {
				StartCoroutine (endTouch ());
			}
		}
	}
	public void DestroyFleaInstant(GameObject obj){
		if(clone!=null)
		PoolingSystem.DestroyAPS (obj);
		bullets -= 1;
		Cooldown ();
	}

	public void calcPower(){
		Vector2 endTouch = new Vector2 (currPos.x, currPos.y);							// touch end position						
		power = fmultiply / (holdTime*holdTime);															// power
		Vector2 shootVector = endTouch - touchStart;									// direction of the shot
		if(power<minpower)power=minpower;
		if(power>maxpower)power=maxpower;
		upvectorpower = Mathf.Sqrt (Mathf.Pow (shootVector.x,2)+Mathf.Pow (shootVector.y,2))*power;
		if (shootVector.magnitude < 60) {
			clone.GetComponent<Bullet>().StopAllCoroutines();
			clone.GetComponent<Bullet>().StartCoroutine ("DestroyFlea",0.5f);
		}
		clone.rigidbody.detectCollisions=true;
		clone.rigidbody.useGravity=true;
		clone.rigidbody.AddForce (shootVector.x*power,upvectorpower,shootVector.y*power, ForceMode.Acceleration); 			 	 // add force to the object towards the direction	

	}

	public IEnumerator endTouch (){
		if(launchpadReady){
			if(clone != null){
				clone.GetComponent<Bullet>().StartCoroutine ("DestroyFlea",3.0f);
				clone.GetComponent<Bullet>().anim.SetBool("Shott",true);
			}
			calcPower ();
			if(bullets>=maxBullets){
				launchParam.setValue((float)bullets/(float)maxBullets);
				launchEvent.start();
				StartCoroutine (BulletCooldown());
				yield return new WaitForSeconds (5.0f);	
			}
			else{
				launchParam.setValue((float)bullets/(float)maxBullets);
				launchEvent.start();
			}
			power = 0.0f;																	// set power back to 0.0f
			touchStart = spawn.position;													// "null" the touch
			touched = false;																// the player area is no longer touched
			clone = null;																	// prepare for next bullet
			yield return new WaitForSeconds (0.1f);											// delay in spawn of bullets
			if(gameEnd == false){
			clone = poolingSystem.InstantiateAPS (cube.name, spawn.position, spawn.rotation);	// spawn the next bullet,
			clone.GetComponent<Animator>().SetBool("Shott",false);
			clone.GetComponent<Bullet>().StopAllCoroutines();
			clone.rigidbody.detectCollisions=false;
			clone.rigidbody.useGravity=false;
			clone.collider.enabled=true;
			bullets++;
			Cooldown();
			}
		}
	}

	public override void OnTouchEnded (){ 													// when touch ends on screen
		if ((virtualPhase == TouchPhase.Ended && touched) || ( touched && !myTex.HitTest (currPos) )) {
			StartCoroutine (endTouch ());
		}
	}

	public static void setEnded (){
		gameEnded = true;

	}
}
