using UnityEngine;
using System.Collections;

public class LevelDown : MonoBehaviour {
	
	Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
		gameObject.renderer.enabled = false;
	}

	public void play () {
		gameObject.renderer.enabled = true;
		anim.SetTrigger ("play");
		StartCoroutine (waitAndDisable ());
	}

	IEnumerator waitAndDisable () {
		yield return new WaitForSeconds(1.5f);
		gameObject.renderer.enabled = false;
	}
}
