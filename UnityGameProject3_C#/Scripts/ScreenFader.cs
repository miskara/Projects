﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {

	public float fadeSpeed = 0.5f;
	Image image;
	Color transparent = new Color (1f, 1f, 1f, 0f);

	void Start (){
		image = GetComponent<Image> ();
		fadeIn ();
	}

	public void fadeIn(){
		image.enabled = true;
		LeanTween.value( gameObject, updateColor, Color.white, transparent, fadeSpeed).setEase(LeanTweenType.easeInOutSine);
		StartCoroutine (waitAndDisable (true));
	}

	public void fadeOut(){
		image.enabled = true;
		LeanTween.value( gameObject, updateColor, transparent, Color.white, fadeSpeed).setEase(LeanTweenType.easeInOutSine);
	}

	void updateColor(Color col){
		image.color = col;
	}

	IEnumerator waitAndDisable(bool b) {
		yield return new WaitForSeconds(fadeSpeed);
		image.enabled = !b;
	}
}
