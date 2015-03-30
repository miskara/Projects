using UnityEngine;
using System.Collections;

public class BugMenu : InputController {

	public GameObject slot;
	RaycastHit hit;
	Animator anim;
	
	void Start () {
		gameObject.transform.localScale = Vector3.zero;
		collider.enabled = false;
		anim = GetComponent<Animator> ();
	}

	public void reveal () {
		collider.enabled = true;
		LeanTween.scale (gameObject, Vector3.one, 0.5f );
		LeanTween.move(gameObject, slot.transform.position, 1.0f).setEase(LeanTweenType.easeInOutQuad); //Moves GameObject to its Slot
		anim.SetBool ("reveal", true);
		GameObject.Find ("ui_handParent1").GetComponent<Revealer> ().hider (0f);
		GameObject.Find ("ui_handParent2").GetComponent<Revealer> ().revealer (3f);
	}
	
	public override void OnTouchBegan (){
		if (rayCastThis()){
			GameObject.Find("MenuController").GetComponent<LoadLevel>().loadScene("ShyBugMenuLevelSelect");
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

	public override void Update () {
		
		base.Update ();
	
	}
}
