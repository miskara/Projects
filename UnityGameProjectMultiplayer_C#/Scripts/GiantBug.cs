using UnityEngine;
using System.Collections;

public class GiantBug : MonoBehaviour {
	
	public Transform center;
	public float degreesPerSecond = -9000.0f;
	
	private Vector3 v;
	private Quaternion q;
	
	void Start() {
		v = transform.position - center.position;
		q = transform.rotation;
	}
	
	void Update () {
		v = Quaternion.AngleAxis (degreesPerSecond * Time.deltaTime, Vector3.up) * v;
		transform.position = center.position + v;
		transform.Rotate (0, 0, -degreesPerSecond * Time.deltaTime);
	}
}