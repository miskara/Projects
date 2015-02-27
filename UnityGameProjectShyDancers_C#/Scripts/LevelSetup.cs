using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSetup : MonoBehaviour {

	public GameObject[] hides;
	public GameObject[] bugs;
	public GameObject[] icons;
	Vector3 hideOffset = new Vector3 (0f, 0f, 0.0f);

	Bug.State prevState;
	Bug.State currentState;

	void Start () {

		hides = GameObject.FindGameObjectsWithTag ("Hide");
		bugs = GameObject.FindGameObjectsWithTag ("Bug");
		icons = GameObject.FindGameObjectsWithTag ("Icon");

		if (hides.Length < bugs.Length) Debug.LogError ("There are not enough hides for bugs!");
		if (bugs.Length != icons.Length) Debug.LogError ("Icon count is different from Bug count");

		for (int i=0; i<bugs.Length; i++) {
			selectHide (i);
		}

		for (int i=0; i<icons.Length; i++) {
			icons[i].GetComponent<Image>().color = Color.gray;
		}
	}

	public void selectHide(int j){
		int hide = Random.Range (0, hides.Length);
		if (hides[hide].GetComponent<Hide>().bug == null) {
			hides[hide].GetComponent<Hide>().bug = bugs[j];
			bugs[j].GetComponent<Bug>().state = Bug.State.Hiding;
			bugs[j].transform.localScale = Vector3.zero;
			bugs[j].transform.position = hides[hide].transform.position + hideOffset; //Move the bug behind a hide
		}
		else {
			selectHide (j);
				}
	}

	void Update () {
		for (int i=0; i<bugs.Length; i++) {
			if (bugs[i].GetComponent<Bug>().state == Bug.State.Away){
				icons[i].GetComponent<Image>().color = Color.gray;
				selectHide(i);
			}
			if (bugs[i].GetComponent<Bug>().state == Bug.State.Front){
				icons[i].GetComponent<Image>().color = Color.green;
			}
		}
	}
}
