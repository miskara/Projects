using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsBug : MonoBehaviour {

	Animator anim;
	public Slider slider;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateAnimSpeed() {
		anim.speed = 1/slider.value*2;
	}
}
