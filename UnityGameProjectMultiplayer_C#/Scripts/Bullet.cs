using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public TouchDragPowerV2 shooter;
	public int fleak;
	public Animator anim;
	public ParticleSystem bHit;



	public IEnumerator PlayAndStopV2(){
		bHit.transform.position = transform.position;
		bHit.Play ();
		yield return new WaitForSeconds (1);
		bHit.Stop ();
		yield return new WaitForSeconds (bHit.duration);
		bHit.Clear ();
	}

	public void HitBurpy(){

		StartCoroutine (PlayAndStopV2 ());
	}

	public void Start(){
		bHit = GameObject.FindGameObjectWithTag ("BHitPrtcl").GetComponent<ParticleSystem> ();
		bHit.Stop ();
		bHit.Clear ();
		anim = GetComponent<Animator> ();
		shooter = GameObject.FindGameObjectWithTag ("Player" + fleak).GetComponentInChildren<TouchDragPowerV2> ();
	}

	public void DecreaseBullets(){
		shooter.bullets--;
		if(shooter.launchpadReady) shooter.Cooldown ();
	}

	public IEnumerator DestroyFlea (float time){
		for (float timer = time; timer >= 0; timer -= Time.deltaTime){
			yield return null;
		}
		//yield return null;
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/flea_poof", transform.position);

		PoolingSystem.DestroyAPS (this.gameObject);
		DecreaseBullets ();
		yield return null;
	}
}