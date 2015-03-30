using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bug : InputController {

	[HideInInspector]
	public float power;
	float boogiemanPower;

	float timeMultiplier;

	public enum BugName {Fleak, JoeJoe, Morphy, Punky, Plurpy, Burpy, Newbee, Wacko};
	public BugName bugName;

	Vector3 place;
	Vector3 baseScale;
	
	//public bool isBoogieman = false;

	public bool tutorial01 = false;
	public bool tutorial02 = false;
	public GameObject tutorialSprite;

	public float jumpTime = 1f;
	public float dancingState1Time = 3f;
	public float dancingState2Time = 3f;
	public float dancingState3Time = 3f;
	public float boogiemanTime = 2f;

	float tutorialTimer;

	public enum State {Front, Hiding, Away, Transition, Victory};
	int danceLvl;
	[HideInInspector]
	public State state = State.Hiding;

	bool isScary = false;
	bool isTired = false;
	bool lvldwn;

	int ID;
	int score;

	RaycastHit hit;
	public Animator anim;
	GameObject controller;
	GameObject[] bugs;
	GameObject MC;

	TutorialController TC;

	ParticleSystem refreshParticles;
	ParticleSystem scareParticles;
	Platform platform;
	LevelDown levelDownEffect;
	GameObject icon;

	FMOD.Studio.ParameterInstance bugParam;

	void Awake () {
		place = gameObject.transform.localPosition;
		baseScale = gameObject.transform.localScale;
		power = 10f;
	}

	void Start () {
		//if (PlayerPrefs.HasKey("timeMultiplier")){
		//	timeMultiplier = PlayerPrefs.GetFloat ("timeMultiplier");
		//}
		//else {
		//	timeMultiplier = 1.0f;
		//}
		timeMultiplier = 1.0f; //Timemultiplier slider override

		anim = GetComponent<Animator> ();
		controller = GameObject.Find("Controller");
		bugs = controller.GetComponent<LevelSetup>().bugs;
		gameObject.transform.localScale = new Vector3 (0, 0, 0);
		collider.enabled = false;

		string iconName = this.name + "_Icon";
		icon = GameObject.Find (iconName);
		if (icon == null) Debug.LogWarning("Icon could not be found for " + this.name);

		MC = GameObject.Find ("MusicController(Clone)");

		if (bugName != BugName.Burpy) {
			platform = gameObject.transform.FindChild ("Platform").GetComponent<Platform> ();
			levelDownEffect = gameObject.transform.FindChild ("LevelDown").GetComponent<LevelDown> ();
			refreshParticles = gameObject.GetComponentInChildren<ParticleSystem> ();
		}
		
		if (MC != null){
			if 			(bugName == BugName.Fleak) 		MC.GetComponent<MusicController>().gameMusic.getParameter("Fleak", 	out bugParam);
			else if 	(bugName == BugName.JoeJoe) 	MC.GetComponent<MusicController>().gameMusic.getParameter("JoeJoe", out bugParam);
			else if 	(bugName == BugName.Morphy) 	MC.GetComponent<MusicController>().gameMusic.getParameter("Morphy", out bugParam);
			else if 	(bugName == BugName.Punky) 		MC.GetComponent<MusicController>().gameMusic.getParameter("Punky",	out bugParam);
			else if 	(bugName == BugName.Plurpy) 	MC.GetComponent<MusicController>().gameMusic.getParameter("Plurp", 	out bugParam);
			else if 	(bugName == BugName.Burpy) 		MC.GetComponent<MusicController>().gameMusic.getParameter("Burpy", 	out bugParam);
			else if 	(bugName == BugName.Newbee) 	MC.GetComponent<MusicController>().gameMusic.getParameter("Newbee", out bugParam);
			else 										MC.GetComponent<MusicController>().gameMusic.getParameter("Wacko", 	out bugParam);
		}

		if (bugName == BugName.Burpy){
			anim.SetBool("burpy", true);
			scareParticles = gameObject.transform.FindChild("prtclShockwave").GetComponent<ParticleSystem>();
		}

		if (tutorial01 || tutorial02) {

			tutorialSprite.renderer.enabled = false;
			collider.enabled = true;
			gameObject.GetComponent<Bug> ().state = Bug.State.Front;
			gameObject.transform.localPosition = place;
			gameObject.transform.localScale = baseScale;
			if (tutorial01){
				gameObject.GetComponent<Bug> ().anim.SetBool ("tutorial1", true);
				Debug.Log ("Tutorial 1");
			}
			else { 
				gameObject.GetComponent<Bug> ().anim.SetBool ("tutorial2", true);
				Debug.Log ("Tutorial 2");
			}

			TC = GameObject.Find("TutorialController").GetComponent<TutorialController>();
			if (icon != null) {
				icon.GetComponent<Image> ().color = Color.white;
			}
		}

	}

	public void reveal () {
		collider.enabled = true;
		LeanTween.scale (gameObject, baseScale, 0.5f );
		LeanTween.move(gameObject, place, jumpTime).setEase(LeanTweenType.easeInOutQuad); //Moves GameObject to its Slot
		anim.SetBool ("escape", false);
		anim.SetBool ("reveal", true);
		power = (dancingState1Time + dancingState2Time + dancingState3Time)*timeMultiplier;
		boogiemanPower = boogiemanTime;
		state = State.Transition;

		if (bugName == BugName.Burpy){
			isScary = true;
		}

		StartCoroutine(waitAndSetState(jumpTime, State.Front));
		StartCoroutine(waitAndAddScore(jumpTime));

	}

	public override void OnTouchBegan (){
		if (rayCastThis() & state == State.Front && bugName != BugName.Burpy){
			refreshPower ();

			if (tutorial01) {
				tutorial01 = false;
				anim.SetBool ("tutorial1", false);
				TC.tutorial1Done();
				tutorialSprite.renderer.enabled = false;
			}
			else if (tutorial02) {
				tutorial02 = false;
				anim.SetBool ("tutorial2", false);
				TC.bugReady();
				tutorialSprite.renderer.enabled = false;
			}
		}
		if (state == State.Victory) {
			controller.GetComponent<Score> ().exit ();
		}

	}

	public override void Update () {

		base.Update ();

		int prevDanceLvl = danceLvl;

		if (state == State.Front){
			if (bugName == BugName.Burpy) boogiemanStateSelector ();
			else bugStateSelector();
		}

		if (tutorial01) {
			power = dancingState1Time - 0.1f;
			tutorialTimer += Time.deltaTime;
			if (tutorialTimer > 4f) tutorialSprite.renderer.enabled = true;
		}

		if (tutorial02) {
			power = 20f;
		}

		if (prevDanceLvl > danceLvl && danceLvl != 0) {
			levelDownEffect.play();
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

	void refreshPower (){
		if (platform != null) {
			platform.flashEnabled = false;
			platform.fadeOut ();
		}
			anim.SetTrigger ("refresh");
			power = (dancingState1Time + dancingState2Time+ dancingState3Time)*timeMultiplier + 1f; //Plus time it takes to play the refresh animation (1f?)
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/bug_refresh", transform.position);
		// Play particle effect
		if (refreshParticles != null)
			refreshParticles.Play ();
		else
			Debug.Log (this.name + ": refresh particles not found");
	}

	public void scared(){
		tired ();
		anim.SetTrigger ("flinch");
		power = dancingState3Time*timeMultiplier + 1f; //Plus time it takes to play the flinch animation (1f?)
		if (tutorial02) tutorialSprite.renderer.enabled = true;
	}

	public void flee(){
		if (platform != null) {
			platform.flashEnabled = false;
			platform.fadeOut ();
		}
		state = State.Transition;
		controller.GetComponent<Score> ().substract(); // Substracts 1 from score
		if (MC != null) {
			bugParam.setValue (0f);

		}
		if (icon != null) {
			icon.GetComponent<Image> ().color = Color.black;
		}
		LeanTween.moveY (gameObject, 10f, jumpTime).setEase (LeanTweenType.easeInOutQuad); //Moves bug out of screen
		StartCoroutine(waitAndSetState (jumpTime, State.Away));
		collider.enabled = false;
		isTired = false;
	}

	public void tired (){
		if (platform != null)
			platform.flashEnabled = true;
		if(!isTired){
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/bug_whimper", transform.position);
			isTired = true;
		}
	}

	public void levelDown (){
		if(!isTired){
			isTired = true;
		}
	}

	public void victory(){
		state = State.Victory;
		refreshPower ();
		anim.SetBool ("danceLevel3", true);
		anim.SetBool ("danceLevel2", false);
		anim.SetBool ("danceLevel1", false);
		power += 9999f; //float.PositiveInfinity didn't work on device?
		Debug.Log (gameObject.name + " is in victory state!");
		if (MC != null) {
			bugParam.setValue (0f);
		}
	}

	IEnumerator waitAndSetState( float time, State s) {
		yield return new WaitForSeconds(time);
		state = s;
		//Debug.Log (this.name + s);
	}

	IEnumerator waitAndAddScore(float time) {
		yield return new WaitForSeconds(time);
		controller.GetComponent<Score> ().add(); // Adds 1 to score
		if (MC != null) {
			bugParam.setValue (1f);
		}
		if (icon != null) {
			icon.GetComponent<Image> ().color = Color.white;
		}
		if(bugName == BugName.Burpy) FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_burp", transform.position);
	}
	
	void bugStateSelector() {

		anim.SetBool ("reveal", false);
		power -= Time.deltaTime;
		
		if (power >= (dancingState1Time + dancingState2Time)*timeMultiplier) { //LVL 3
			anim.SetBool ("danceLevel3", true);
			anim.SetBool ("danceLevel2", false);
			anim.SetBool ("danceLevel1", false);
			danceLvl = 3;
			isTired = false;
		}
		else if (power <= (dancingState1Time + dancingState2Time)*timeMultiplier && power >= dancingState3Time*timeMultiplier) { //LVL 2
			anim.SetBool ("danceLevel3", false);
			anim.SetBool ("danceLevel2", true);
			anim.SetBool ("danceLevel1", false);
			danceLvl = 2;
			isTired = false;
		}
		else if (power <= dancingState3Time*timeMultiplier && power >= 0.0f) { //LVL 1
			anim.SetBool ("danceLevel3", false);
			anim.SetBool ("danceLevel2", false);
			anim.SetBool ("danceLevel1", true);
			danceLvl = 1;
			tired ();
		}
		else if (power <= 0.0f) { //ESCAPE
			anim.SetBool ("danceLevel3", false);
			anim.SetBool ("danceLevel2", false);
			anim.SetBool ("danceLevel1", false);
			anim.SetBool ("escape", true);
			danceLvl = 0;
			flee();
		}
	}

	void boogiemanStateSelector(){

		score = controller.GetComponent<Score>().score; // Get updated score
		anim.SetBool ("reveal", false);

		if (state == State.Front) {
			if (score > 1){ // If there's other bugs dancing
				anim.SetBool("burpyAlone", false);
				boogiemanPower = boogiemanTime;
				if(isScary && score < bugs.Length){
					scareParticles.Play (); // Burpy's particle effect
					for (int i=0; i<bugs.Length; i++) {
						if (bugs[i].GetComponent<Bug>().state == State.Front && bugs[i].GetComponent<Bug>().bugName != BugName.Burpy){
							bugs[i].GetComponent<Bug>().scared(); //Scare every bug that's dancing
						}
					}
					isScary = false;
				}
			}
			
			else { // If only Boogieman is dancing
				boogiemanPower -= Time.deltaTime;
				isScary = false;
				anim.SetBool("burpyAlone", true);
				if (boogiemanPower <= 0f) {
					isScary = false;
					anim.SetBool("escape", true);
					flee ();
				}
			}
		}
	}
}
