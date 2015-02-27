using UnityEngine;
using System.Collections;

public class MenuSound : MonoBehaviour {

	public void tapSound () {
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/ui_tap", Vector3.zero);
	}
}
