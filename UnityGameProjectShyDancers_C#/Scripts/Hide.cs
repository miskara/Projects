using UnityEngine;
using System.Collections;

public class Hide : InputController {

	public Sprite Closed;
	public Sprite Open;
	[HideInInspector]
	public GameObject bug;
	GameObject eyes;
	float timer;
	RaycastHit hit;
	//Animator anim;
	GameObject inputBlocker;
	public enum State {Closed, Peek, Open};
	[HideInInspector]
	public State state = State.Closed;

	void Start () {
		GetComponent<SpriteRenderer>().sprite = Closed;
		inputBlocker = GameObject.Find("InputBlocker");
		if(gameObject.transform.childCount <= 0){
			Debug.LogError(gameObject.name + " has no Eyes as its child");
		}
		eyes = gameObject.transform.GetChild (0).gameObject;
		eyes.GetComponent<SpriteRenderer>().enabled = false;
	}

	public override void OnTouchBegan (){
		Ray ray = Camera.main.ScreenPointToRay(InputController.GetTouchPosition());
		if (Physics.Raycast(ray, out hit)) {
			if(hit.collider.gameObject == this.gameObject){
				inputBlocker.GetComponent<InputBlocker>().enableBlock(); // Blocks input for X seconds
				if (state == State.Peek){
					if (bug != null){ // if there is a bug in the hide
						state = State.Open;
						eyes.GetComponent<SpriteRenderer>().enabled = false;
						bug.GetComponent<Bug>().reveal();
						FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/hide_bug", transform.position);
						bug = null;
						shake();
					}
					else{
						FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/hide_empty", transform.position);
						shake();
					}
				}
				if (state == State.Closed) {
					shake();
					FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/hide", transform.position);
					StartCoroutine(waitAndClose());
					if (bug != null){
						eyes.GetComponent<SpriteRenderer>().enabled = true;
					}
					GetComponent<SpriteRenderer>().sprite = Open;
					state = State.Peek;
				}
			}
		}
	}

	void shake(){
		if (state == State.Closed){ //open shake
		LeanTween.moveY (gameObject, 0.03f, 0.3f).setEase (LeanTweenType.easeShake);
		LeanTween.rotateZ (gameObject, 3.0f, 0.2f).setEase (LeanTweenType.easeShake);
		}
		else if (state == State.Peek){ //If hide is empty
			LeanTween.moveX (gameObject, 0.1f, 0.2f).setEase (LeanTweenType.easeShake);
		}
		else if (state == State.Open){ //If hide reveals a bug
			LeanTween.moveY (gameObject, 0.07f, 0.3f).setEase (LeanTweenType.easeShake);
			LeanTween.rotateZ (gameObject, 6.0f, 0.25f).setEase (LeanTweenType.easeShake);
		}
	}

	IEnumerator waitAndClose() {
		yield return new WaitForSeconds(3f);
		state = State.Closed;
		GetComponent<SpriteRenderer>().sprite = Closed;
		eyes.GetComponent<SpriteRenderer>().enabled = false;
		LeanTween.moveY (gameObject, -0.02f, 0.5f).setEase (LeanTweenType.easeShake); //closing shake
	}
}