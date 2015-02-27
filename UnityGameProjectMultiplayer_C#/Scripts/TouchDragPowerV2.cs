using UnityEngine;
using System.Collections;

public class TouchDragPowerV2: TouchLogicV2 /*MonoBehaviour*/
{
	
	public int player;								// playerID

	public GameObject clone;						// used to clone the spawnable flea-bullet
	public GameObject cube;							// the launchable object
	private PoolingSystem poolingSystem;			// the instance pool;
	public SpriteRenderer launchpad;
	GUITexture myTex;								// player area
	Vector2 touchStart;								// touch position begin
	public Color orig;
	
	public static bool gameEnded;					// is the game ended?
	private bool touched = false;					// is this player area already touched?
	bool overHeat = false;
	public bool gamestart;

	public Transform spawn;							// spawn point for the object
	public float destroyTime = 5.0f;				// time in which the object will disappear from the scene
	public float fmultiply = 1.001f;				// multiplier to make the object speed more pleasant
	private float holdTime = 0.0f;					// how long the player has touched the screen (affects the power)
	public float power;								// total power
	public float gravity;
	public float overheatingMeter;
	public float upvectorpower;
	
	void Start (){
		gamestart = false;
		upvectorpower = 1000f;
		overheatingMeter = 1.0f;
		gameEnded = false;
		poolingSystem = PoolingSystem.Instance;
		cube = Resources.Load ("Prefabs/Fleak" + player) as GameObject;
		name = cube.name;
		Physics.gravity = new Vector3 (0f, -70, 0f);
		myTex = this.guiTexture;
		gameEnded = false;
		InvokeRepeating ("Cooldown",0f,0.15f);

		orig = launchpad.renderer.material.color;
	}

	public void FirstFleak(){
		clone = poolingSystem.InstantiateAPS (name, spawn.position, spawn.rotation);
	}
	public void rmFirstFleak(){
		PoolingSystem.DestroyAPS (clone);
	}

	public void ScoreScreen (){
		Ray ray;
		RaycastHit hit;
		if (InputController.HasTouchBegan ()) {
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.name.Contains ("Burpy")) {
					Application.LoadLevel (0);
					gameEnded = false;
					PoolingSystem.Instance.DeactiveAll ();
				}
			}
		}
	}
	

	void Cooldown(){

		if (overheatingMeter >= 1f) overheatingMeter = 1f;
		if (overheatingMeter < 0.35f && !overHeat){
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/launchpad_overheat", transform.position);
			overHeat = true;
		}
		if (overheatingMeter >=  0.35f && overHeat){
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/launchpad_cooldown", transform.position);
			overHeat = false;
		}

		if (overheatingMeter < 1){
			overheatingMeter +=(overheatingMeter/10)+0.01f;
		}
		launchpad.renderer.material.SetColor("_Color",Color.Lerp(Color.black,orig,overheatingMeter));

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

	public IEnumerator DestroyFlea (GameObject obj){
		for (float timer = 3; timer >= 0; timer -= Time.deltaTime){
			GameObject[] list = GameObject.FindGameObjectsWithTag("Target");
			for(int i = 0; i< list.Length; i++){
				if(obj.renderer.bounds.Intersects(list[i].renderer.bounds))
					break;
			}
			yield return 0;
		}
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_poof", transform.position);
		obj.rigidbody.velocity = new Vector3 (0f, 0f, 0f);
		PoolingSystem.DestroyAPS (obj);
		yield return null;
	}

	public IEnumerator endTouch (){
		if(overheatingMeter>0.35f){
			StartCoroutine (DestroyFlea(clone));																			
			Vector2 endTouch = new Vector2 (currPos.x, currPos.y);							// touch end position						
			power = fmultiply / (holdTime);													// power
			Vector2 shootVector = endTouch - touchStart;									// direction of the shot
			Vector3 shootVec3d = new Vector3 (shootVector.x*power, upvectorpower, shootVector.y*power);				// give the shot a nice parable
			clone.rigidbody.AddForce ((shootVec3d), ForceMode.Impulse); 					// add force to the object towards the direction
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_launch", transform.position); // play launch sound
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/launchpad_heat", transform.position);
			overheatingMeter -= 0.35f;
			power = 0.0f;																	// set power back to 0.0f
			touchStart = spawn.position;													// "null" the touch
			touched = false;																// the player area is no longer touched
			clone = null;																	// prepare for next bullet
			yield return new WaitForSeconds (0.05f);										// delay in spawn of bullets
			clone = poolingSystem.InstantiateAPS (name, spawn.position, spawn.rotation);	// spawn the next bullet
			StopCoroutine (DestroyFlea (clone));
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
