using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Platform : MonoBehaviour {

	SpriteRenderer sprite;
	Color transparent = new Color (1f, 1f, 1f, 0f);

	public float flashSpeed = 1f;

	public bool flashEnabled = false;

	void Start () {
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		sprite.color = transparent;
	}

	public void fadeOut(){
		LeanTween.value( gameObject, updateColor, sprite.color, transparent, 0.1f).setEase(LeanTweenType.easeInOutSine);
	}
	
	public void fadeIn(){
		LeanTween.value( gameObject, updateColor, transparent, sprite.color, 0.1f).setEase(LeanTweenType.easeInOutSine);
	}
	
	void updateColor(Color col){
		sprite.color = col;
	}

	public void flash () {
		if (flashEnabled) {
			float sin = Mathf.Sin (Time.time * 10 * flashSpeed);
			float a = (sin + 1f) / 2;
			sprite.color = new Color (1f, 1f, 1f, a);
		}
	}

	void Update () {
		flash ();
	}

}
