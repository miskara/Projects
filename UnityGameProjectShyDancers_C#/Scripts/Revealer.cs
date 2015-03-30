using UnityEngine;
using System.Collections;

public class Revealer : MonoBehaviour {

	public bool startAtZeroScale;

	public bool reveal;
	public float revealSpeed = 1f;
	public float revealDelay = 0f;

	public bool hide;
	public float hideSpeed = 0.5f;
	public float hideDelay = 1f;

	Vector3 baseScale;

	void Start () {
		baseScale = gameObject.transform.localScale;
		if (startAtZeroScale)
			gameObject.transform.localScale = Vector3.zero;
		if (reveal)
			revealer (revealDelay);
		if (hide)
			hider (hideDelay);
	}
	
	void Update () {
	
	}

	public void revealer (float delay) {
		gameObject.transform.localScale = Vector3.zero;
		LeanTween.scale (gameObject, baseScale, revealSpeed).setEase (LeanTweenType.easeOutBack).setDelay (delay);
	}

	public void hider (float delay) {
		LeanTween.scale (gameObject, Vector3.zero, hideSpeed).setEase (LeanTweenType.easeInSine).setDelay (delay);
	}
}
