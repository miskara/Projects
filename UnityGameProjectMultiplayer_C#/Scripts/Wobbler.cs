using UnityEngine;
using System.Collections;

public class Wobbler : MonoBehaviour {
	
	public bool wobble;
	public float wobbleAmount = 0.1f;
	public float wobbleSpeed = 1f;
	public float wobbleOffset = 0f;
	
	public bool tap;
	public float tapAmount = 1f;
	public float tapSpeed = 1f;
	public float tapOffset = 0f;
	
	public bool sway;
	public float swayAmount = 1f;
	public float swaySpeed = 1f;
	public float swayOffset = 0f;
	
	public bool bounce;
	public float bounceAmount = 1f;
	public float bounceSpeed = 1f;
	public float bounceOffset = 0f;
	
	public bool hover;
	public float hoverAmount = 1f;
	public float hoverSpeed = 1f;
	public float hoverOffset = 0f;

	public bool spin;
	public float spinSpeed = 1f;
	public float spinOffset = 0f;
	public enum rotationAxis  {x,y,z};
	public rotationAxis axis;
	
	float result;
	Vector3 baseScale;
	Vector3 baseRotation;
	Vector3 basePosition;
	
	void Start () {
		if(gameObject.name.Equals("PlayButton")){
			baseScale = new Vector3(4f,4f,2f);
		}
		else{
		baseScale = gameObject.transform.localScale;
		}
		baseRotation = gameObject.transform.localEulerAngles;
		basePosition = gameObject.transform.localPosition;
	}
	
	void Update () {
		if (wobble)
			wobbler ();
		if (tap)
			tapper ();
		if (sway)
			swayer ();
		if (bounce)
			bouncer ();
		if (hover)
			hoverer ();
		if (spin)
			spinner ();
	}
	
	void wobbler() {
		float sin = Mathf.Sin (Time.time * 5 * wobbleSpeed + wobbleOffset);
		float y = 1 + sin / (1 / wobbleAmount);
		float x = 1 / y;
		transform.localScale = new Vector3(baseScale.x * x, baseScale.y * y, baseScale.z);
	}
	
	void tapper() {
		float sin = Mathf.Sin (Time.time * 5 * tapSpeed + tapOffset);
		float z = sin;
		if (sin >= 0)
			z = sin;
		if (sin < 0)
			//z = sin*(-sin*0.05f);
			//z = sin-(sin*0.95f);
			z = sin*0.05f;
		transform.localEulerAngles = new Vector3 (baseRotation.x, baseRotation.y, baseRotation.z - (z * 20 * tapAmount));
	}
	
	void swayer () {
		float sin = Mathf.Sin (Time.time * 5 * swaySpeed + swayOffset);
		float z = sin;
		transform.localEulerAngles = new Vector3 (baseRotation.x, baseRotation.y, baseRotation.z + (z * 5f * swayAmount));
	}
	
	void bouncer () {
		float sin = Mathf.Sin (Time.time * 5 * bounceSpeed + bounceOffset);
		float y = sin;
		if (sin >= 0)
			y = sin;
		if (sin < 0)
			y = -sin;
		transform.localPosition = new Vector3 (basePosition.x, basePosition.y + (y * 5f * bounceAmount), basePosition.z);
	}
	
	void hoverer () {
		float sin = Mathf.Sin (Time.time * 5 * hoverSpeed + hoverOffset);
		float y = sin;
		transform.localPosition = new Vector3 (basePosition.x, basePosition.y + (y * 5f * hoverAmount), basePosition.z);
	}

	void spinner () {
		float rot = Time.time * 10f * spinSpeed;
		if (axis == rotationAxis.x)
			gameObject.transform.localEulerAngles = new Vector3 (rot, baseRotation.y, baseRotation.z);
		else if (axis == rotationAxis.y)
			gameObject.transform.localEulerAngles = new Vector3 (baseRotation.x, rot, baseRotation.z);
		else if (axis == rotationAxis.z)
			gameObject.transform.localEulerAngles = new Vector3 (baseRotation.x, baseRotation.y, rot);
	}
}
