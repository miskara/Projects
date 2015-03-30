using UnityEngine; using System.Collections;

public class BlobShadowController : MonoBehaviour {

	private Vector3 orientation;
	private Vector3 offset;

	public void Awake(){
	
		orientation = transform.rotation.eulerAngles;
		offset = transform.position - transform.parent.position;
	
	}


	void Update () {
		orientation.y = transform.parent.rotation.eulerAngles.y;
		transform.rotation = Quaternion.Euler (orientation);
		transform.position = transform.parent.position + offset;
	}

/*	void Update() {

		transform.position = transform.parent.position + Vector3.up * 8.246965f; 
		transform.rotation = Quaternion.LookRotation(-Vector3.up, transform.parent.forward); 
	}*/
}
