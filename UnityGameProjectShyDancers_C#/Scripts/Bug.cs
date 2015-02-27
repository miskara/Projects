using UnityEngine;
using System.Collections;

public class Bug : InputController {

	float power;
	float boogiemanPower;

	float timeMultiplier;

	public GameObject slot;
	public bool isBoogieman = false;
	public float jumpTime = 1f;
	public float dancingState1Time = 3f;
	public float dancingState2Time = 3f;
	public float dancingState3Time = 3f;
	public float boogiemanTime = 2f;

	public enum State {Front, Hiding, Away, Transition, Victory};
	[HideInInspector]
	public State state = State.Hiding;

	bool isScary = false;
	bool isTired = false;

	int ID;
	int score;

	RaycastHit hit;
	Animator anim;
	GameObject controller;
	GameObject[] bugs;

	void Start () {
		if (PlayerPrefs.HasKey("timeMultiplier")){
			timeMultiplier = PlayerPrefs.GetFloat ("timeMultiplier");
		}
		else {
			timeMultiplier = 1.0f;
		}

		anim = GetComponent<Animator> ();
		controller = GameObject.Find("Controller");
		bugs = controller.GetComponent<LevelSetup>().bugs;
		gameObject.transform.localScale = new Vector3 (0, 0, 0);
		collider.enabled = false;
	}

	public void reveal () {
		collider.enabled = true;
		LeanTween.scale (gameObject, Vector3.one, 0.5f );
		LeanTween.move(gameObject, slot.transform.position, jumpTime).setEase(LeanTweenType.easeInOutQuad); //Moves GameObject to its Slot
		anim.SetBool ("escape", false);
		anim.SetBool ("reveal", true);
		power = (dancingState1Time + dancingState2Time + dancingState3Time)*timeMultiplier;
		boogiemanPower = boogiemanTime;
		state = State.Transition;

		if (isBoogieman){
			isScary = true;
		}

		StartCoroutine(waitAndSetState(jumpTime, State.Front));
		StartCoroutine(waitAndAddScore(jumpTime));
	}

	public override void OnTouchBegan (){
		if (rayCastThis() & state == State.Front){
			refreshPower ();
		}
	}

	public override void Update () {

		base.Update ();

		if (state == State.Front){
			if (isBoogieman) boogiemanStateSelector ();
			else bugStateSelector();
		}
	}

	public bool rayCastThis(){
		Ray ray = Camera.main.ScreenPointToRay(InputController.GetTouchPosition());
		if (Physics.Raycast(ray, out hit)) {
			if(hit.collider.gameObject == this.gameObject){
				return true;
			}
			else return false;
		}
		else return false;
	}

	void refreshPower(){
		if(!anim.GetBool("danceLevel3")){
			anim.SetTrigger ("refresh");
			power = (dancingState1Time + dancingState2Time+ dancingState3Time)*timeMultiplier + 1f; //Plus time it takes to play the refresh animation (1f?)
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/bug_refresh", transform.position);
		}
	}

	public void scared(){
		anim.SetTrigger ("flinch");
		power = dancingState3Time*timeMultiplier + 1f; //Plus time it takes to play the flinch animation (1f?)
	}

	public void flee(){
		state = State.Transition;
		controller.GetComponent<Score> ().substract(); // Substracts 1 from score
		LeanTween.moveY (gameObject, 10f, jumpTime).setEase (LeanTweenType.easeInOutQuad); //Moves bug out of screen
		StartCoroutine(waitAndSetState (jumpTime, State.Away));
		collider.enabled = false;
		isTired = false;
	}

	public void tired(){
		if(!isTired){
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/bug_whimper", transform.position);
			isTired = true;
		}
	}

	public void victory(){
		state = State.Victory;
		anim.SetTrigger ("refresh");
		anim.SetBool ("danceLevel3", true);
		anim.SetBool ("danceLevel2", false);
		anim.SetBool ("danceLevel1", false);
		power += 99999f; //float.PositiveInfinity didn't work on device?
		Debug.Log (gameObject.name + " is in victory state!");
		//anim.SetBool ("Victory", true); //Possible victory animation
	}

	IEnumerator waitAndSetState( float time, State s) {
		yield return new WaitForSeconds(time);
		state = s;
	}

	IEnumerator waitAndAddScore(float time) {
		yield return new WaitForSeconds(time);
		controller.GetComponent<Score> ().add(); // Adds 1 to score
	}
	
	void bugStateSelector() {

		anim.SetBool ("reveal", false);
		power -= Time.deltaTime;
		
		if (power >= (dancingState1Time + dancingState2Time)*timeMultiplier) { //LVL 3
			anim.SetBool ("danceLevel3", true);
			anim.SetBool ("danceLevel2", false);
			anim.SetBool ("danceLevel1", false);
			isTired = false;
		}
		else if (power <= (dancingState1Time + dancingState2Time)*timeMultiplier && power >= dancingState3Time*timeMultiplier) { //LVL 2
			anim.SetBool ("danceLevel3", false);
			anim.SetBool ("danceLevel2", true);
			anim.SetBool ("danceLevel1", false);
			isTired = false;
		}
		else if (power <= dancingState3Time*timeMultiplier && power >= 0.0f) { //LVL 1
			anim.SetBool ("danceLevel3", false);
			anim.SetBool ("danceLevel2", false);
			anim.SetBool ("danceLevel1", true);
			tired ();
		}
		else if (power <= 0.0f) { //ESCAPE
			anim.SetBool ("danceLevel3", false);
			anim.SetBool ("danceLevel2", false);
			anim.SetBool ("danceLevel1", false);
			anim.SetBool ("escape", true);
			flee();
		}
	}

	void boogiemanStateSelector(){

		score = controller.GetComponent<Score>().score; // Get updated score
		
		if (state == State.Front) {
			if (score > 1){ // If there's other bugs dancing 
				if(isScary){
					for (int i=0; i<bugs.Length; i++) {

						if (bugs[i].GetComponent<Bug>().state == State.Front && !bugs[i].GetComponent<Bug>().isBoogieman){
							bugs[i].GetComponent<Bug>().scared(); //Scare every bug that's dancing
						}
					}
					isScary = false;
				}
			}
			
			else { // Only Boogieman is dancing
				boogiemanPower -= Time.deltaTime;
				isScary = false;
				if (boogiemanPower <= 0f) {
					isScary = false;
					flee ();
				}
			}
		}
	}
}
