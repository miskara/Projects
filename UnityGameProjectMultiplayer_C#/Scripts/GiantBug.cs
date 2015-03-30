using UnityEngine;
using System.Collections;

public class GiantBug : MonoBehaviour {
	
	public Transform center;
	public float degreesPerSecond = 1f;
	
	private Vector3 v;
	private Quaternion q;

	
	void Update () {
		transform.RotateAround (center.position, Vector3.up, degreesPerSecond * Time.deltaTime);

	}
}