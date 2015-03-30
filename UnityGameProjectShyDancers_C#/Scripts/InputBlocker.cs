using UnityEngine;
using System.Collections;

public class InputBlocker : MonoBehaviour {

	void Awake () {
		gameObject.collider.enabled = false;
	}

	public void enableBlock (float duration) {
		gameObject.collider.enabled = true;
		StartCoroutine(WaitAndDisable(duration));
	}
	
	public void disableBlock () {
		gameObject.collider.enabled = false;
	}

	IEnumerator WaitAndDisable(float duration) {
		yield return new WaitForSeconds(duration);
		disableBlock ();
	}

	public void enableBlockTutorial () {
		gameObject.collider.enabled = true;
		StartCoroutine(WaitAndDisableTutorial());
	}

	IEnumerator WaitAndDisableTutorial() {
		yield return new WaitForSeconds(6f);
		disableBlock ();
	}

	public void blockOn () {
		gameObject.collider.enabled = true;
	}

	public void blockOff () {
		gameObject.collider.enabled = false;
	}

	public void endBlock () {
		gameObject.collider.enabled = true;
	}
}
