using UnityEngine;
using System.Collections;

public class Hide : InputController {

	public Sprite Closed;
	public Sprite Open;
	public GameObject bug;
	[HideInInspector]
	public GameObject eyes;
	public GameObject tutorialHand;
	float timer;
	RaycastHit hit;
	//Animator anim;
	InputBlocker inputBlocker;
	public enum State {Closed, Peek, Open};
	[HideInInspector]
	public State state = State.Closed;
	bool tutorialHandRevealed = false;

	Vector3 baseRotation;
	Vector3 baseScale;

	public float fadeSpeed = 1.0f;
	Color transparent = new Color (1f, 1f, 1f, 0f);

	void Start () {
		GetComponent<SpriteRenderer>().sprite = Closed;
		inputBlocker = GameObject.Find("InputBlocker").GetComponent<InputBlocker>();
		eyes = gameObject.transform.GetChild (0).gameObject;
		eyes.GetComponent<SpriteRenderer>().enabled = false;
		if (gameObject.transform.childCount > 1) {
			tutorialHand = gameObject.transform.FindChild("ui_hand_hide").gameObject;
		}
		baseRotation = gameObject.transform.localEulerAngles;
		baseScale = gameObject.transform.localScale;
		Debug.Log (eyes);
	}

	public override void OnTouchBegan (){
		Ray ray = Camera.main.ScreenPointToRay(InputController.GetTouchPosition());
		if (Physics.Raycast(ray, out hit)) {
			if(hit.collider.gameObject == this.gameObject){
				if (state == State.Peek){
					if (bug != null){ // if there is a bug in the hide
						state = State.Open;
						eyes.GetComponent<SpriteRenderer>().enabled = false;
						if (bug.GetComponent<Bug>() == null && Application.loadedLevelName == "ShyBugMenuMain") bug.GetComponent<BugMenu>().reveal(); //MENU BUG
						else bug.GetComponent<Bug>().reveal();
						inputBlocker.enableBlock(0.5f);
						FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/hide_bug", transform.position);
						bug = null;
						shake();
						if (tutorialHand != null && tutorialHandRevealed) {
							tutorialHand.GetComponent<Revealer>().hider(0f);
							tutorialHandRevealed = false;
						}
					}
					else{
						inputBlocker.enableBlock(0.1f); // Blocks input
						FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/hide_empty", transform.position);
						shake();
					}
				}
				if (state == State.Closed) {
					inputBlocker.enableBlock(0.1f);
					shake();
					FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/hide", transform.position);
					StartCoroutine(waitAndClose());
					if (bug != null){
						eyes.GetComponent<SpriteRenderer>().enabled = true;
						if (tutorialHand != null && !tutorialHandRevealed) {
							tutorialHand.GetComponent<Revealer>().revealer(0f);
							tutorialHandRevealed = true;
						}
					}
					GetComponent<SpriteRenderer>().sprite = Open;
					state = State.Peek;
				}
			}
		}
	}

	public void shake(){
		Debug.Log (this.name + baseRotation.z);
		if (state == State.Closed){ //open shake
			if (!LeanTween.isTweening(gameObject)) {
				//LeanTween.moveY (gameObject, 0.03f, 0.3f).setEase (LeanTweenType.easeShake);
				//LeanTween.rotateZ (gameObject, (baseRotation.z - 3.0f), 0.2f).setEase (LeanTweenType.easeShake);
				LeanTween.scaleX (gameObject, (baseScale.x * 0.9f), 0.2f).setEase (LeanTweenType.easeOutCubic);
				LeanTween.scaleY (gameObject, (baseScale.y * 1.1f), 0.2f).setEase (LeanTweenType.easeOutCubic);
				LeanTween.scaleX (gameObject, (baseScale.x), 0.2f).setEase (LeanTweenType.easeInOutCubic).setDelay(0.2f);
				LeanTween.scaleY (gameObject, (baseScale.y), 0.2f).setEase (LeanTweenType.easeInOutCubic).setDelay(0.2f);
			}
		}
		else if (state == State.Peek){ //If hide is empty
			if (!LeanTween.isTweening(gameObject))
				LeanTween.moveX (gameObject, 0.1f, 0.2f).setEase (LeanTweenType.easeShake);
		}
		else if (state == State.Open){ //If hide reveals a bug
			if (!LeanTween.isTweening(gameObject)) {
				LeanTween.moveY (gameObject, 0.07f, 0.3f).setEase (LeanTweenType.easeShake);
				LeanTween.rotateZ (gameObject, baseRotation.z + 6.0f, 0.25f).setEase (LeanTweenType.easeShake);
			}
		}
	}

	IEnumerator waitAndClose() {
		yield return new WaitForSeconds(3f);
		state = State.Closed;
		GetComponent<SpriteRenderer>().sprite = Closed;
		eyes.GetComponent<SpriteRenderer>().enabled = false;
		LeanTween.moveY (gameObject, -0.02f, 0.5f).setEase (LeanTweenType.easeShake); //closing shake
	}

	public void fadeIn(){
		gameObject.collider.enabled = true;
		LeanTween.value (gameObject, updateColor, transparent, Color.white, fadeSpeed).setEase (LeanTweenType.easeInOutSine).setDelay (0.5f);
	}

	public void fadeOut(){
		gameObject.collider.enabled = true;
		LeanTween.value (gameObject, updateColor, Color.white, transparent, fadeSpeed).setEase (LeanTweenType.easeInOutSine).setDelay (0.5f);
	}
	
	void updateColor(Color col){
		gameObject.GetComponent<SpriteRenderer> ().color = col;
	}
}