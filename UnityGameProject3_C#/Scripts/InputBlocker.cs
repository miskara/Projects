using UnityEngine;
using System.Collections;

public class InputBlocker : MonoBehaviour {

	public float inputBlockTime = 0.5f;

	void Start (){
		gameObject.collider.enabled = false;
	}

	public void enableBlock () {
		gameObject.collider.enabled = true;
		StartCoroutine(WaitAndDisable());
	}
	
	public void disableBlock () {
		gameObject.collider.enabled = false;
	}

	IEnumerator WaitAndDisable() {
		yield return new WaitForSeconds(inputBlockTime);
		disableBlock ();
	}
}
