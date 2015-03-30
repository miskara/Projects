using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {
	
	GameObject cam;
	GameObject camPos;
	GameObject [] hides;
	InputBlocker inputBlocker;
	GameObject tutorialWindow;

	int readyCount = 0;
	
	void Start () {
		//Debug.Log ("Tutorial");
		inputBlocker = GameObject.Find("InputBlocker").GetComponent<InputBlocker>();
		cam = GameObject.Find ("Main Camera");
		camPos = GameObject.Find ("CameraGamePosition");
		hides = GameObject.FindGameObjectsWithTag ("Hide");

		if (Application.loadedLevelName == "ShyBugLevel02") {
			inputBlocker.enableBlock(4f);
		}
		else if (Application.loadedLevelName == "ShyBugLevel01") {
			inputBlocker.enableBlock(2f);
			tutorialWindow = GameObject.Find("tutorialWindow");
		}

		for (int i=0; i<hides.Length; i++) {
			hides [i].AddComponent<HideTutorial>();
		}
	}

	public void tutorial1Done() {
		Debug.Log ("Tutorial 1 is done");
		LeanTween.move (cam, camPos.transform.position, 2.0f).setDelay (0.1f).setEase (LeanTweenType.easeInOutSine);
		LeanTween.rotateX (cam, camPos.transform.localRotation.x, 2.0f).setDelay (0.1f).setEase (LeanTweenType.easeInOutSine);
		for (int i=0; i<hides.Length; i++) {
			hides [i].GetComponent<HideTutorial> ().fadeIn ();
			hides [i].GetComponent<HideTutorial> ().tutorialShake ();
		}
		LeanTween.move (tutorialWindow, camPos.transform.position, 1.0f).setDelay (1.0f).setEase (LeanTweenType.easeInSine);
	}

	public void tutorial2Done() {
		Debug.Log ("Tutorial 2 is done");
		LeanTween.move (cam, camPos.transform.position, 2.0f).setDelay (0.1f).setEase (LeanTweenType.easeInOutSine);
		for (int i=0; i<hides.Length; i++) {
			hides [i].GetComponent<HideTutorial> ().fadeIn ();
			hides [i].GetComponent<HideTutorial> ().tutorialShake ();
		}
	}

	public void bugReady() {
		readyCount += 1;
		//Debug.Log (readyCount);
		if (readyCount == 2)
			tutorial2Done ();
	}

	void Update () {
	}
}


