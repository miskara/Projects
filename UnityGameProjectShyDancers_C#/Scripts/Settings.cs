using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public Slider timeSlider;

	void Start(){
		if (!PlayerPrefs.HasKey("timeMultiplier")){
			PlayerPrefs.SetFloat ("timeMultiplier", 1.0f);
		}
		else {
			timeSlider.value = PlayerPrefs.GetFloat ("timeMultiplier");
		}
	}

	public void saveSettings() {
		PlayerPrefs.SetFloat ("timeMultiplier", timeSlider.value);
		PlayerPrefs.Save();
	}
}
