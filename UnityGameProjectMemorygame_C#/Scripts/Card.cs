using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : VersionedView{


	public enum CardState {Flipped,Hidden,Bombed}
	public CardState state = CardState.Hidden;

	public bool abletoflip = true;
	public int index;
	public GameObject cardView;
	public string CardType;
	public float delay = 5.0f;
	public Card pair;
	public Renderer child;
	public Color faded;
	bool setTimer = false;
	public List<Renderer> rlist;
	float cardTimer;
	public GameObject character;
	public Transform spawn;
	public ParticleSystem ps;
	public ParticleSystem burp;
	public Animator anim;


	public IEnumerator PlayAndStopV2(){
		ps.Play ();
		yield return new WaitForSeconds (1);
		ps.Stop ();
		yield return new WaitForSeconds (ps.duration);
		ps.Clear ();
	}

	void Start (){
		ps.Stop ();
		ps.Clear ();
		child = GetComponentInChildren<Renderer>();
		rlist.AddRange( GetComponentsInChildren<Renderer> ());
		faded = new Color (child.material.color.r, child.material.color.g, child.material.color.b, 0);
	}

	public void setAnimator(Animator anim){
		this.anim = anim;
	}

	public void setPair (Card c){	

		this.pair = c;
	}

	public override void DirtyUpdate (){

		StartCoroutine(StartCardFlip());
	}

	public override void Update(){
		base.Update ();
		if (setTimer){
			cardTimer -= Time.deltaTime;
			if (cardTimer <= 0){
				cardView.animation.Play();
				setTimer = false;
				anim.SetBool ("Revealed",false);
			}
		}
	}

	public IEnumerator StartCardFlip(){
		AnimationClip clip;
		clip = cardView.animation.GetClip("Card Flip");
		if (state == CardState.Flipped) {
			cardView.animation ["Card Flip"].speed = 1.0f;
			cardView.animation.Play();
			if (CardType == "Fleak")
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_flip_fleak", GameObject.Find("Main Camera").transform.position);
			else if (CardType == "Joejoe")
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_flip_joejoe", GameObject.Find("Main Camera").transform.position);
			else if (CardType == "Morphy")
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_flip_morphy", GameObject.Find("Main Camera").transform.position);
			else
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_flip", GameObject.Find("Main Camera").transform.position);
			yield return new WaitForSeconds (clip.length);
			anim.SetBool ("Revealed",true);
		}
		else{
			cardView.animation["Card Flip"].speed = -1.0f;
			cardView.animation["Card Flip"].time = clip.length;

			cardTimer = 3f;
			setTimer = true;

			yield return new WaitForSeconds(clip.length);
		}
	
		setMatch ();
		yield return 0;
	}

	public void setMatch(){
		if(state == CardState.Flipped){

			Playmat.GetPlaymat().SetCardsForMatch(this);

		}
		else if (state == CardState.Hidden){

			if(Playmat.GetPlaymat().NumberOfCardsFlipped <=0) Playmat.GetPlaymat().NumberOfCardsFlipped=0;
			else Playmat.GetPlaymat().NumberOfCardsFlipped--;
			
		}
	}

	public void GenerateCard(string cardType){

		CardType = cardType;
		GameObject resource = Resources.Load("Prefabs/"+cardType) as GameObject;
		character = GameObject.Instantiate(resource,spawn.position,spawn.rotation) as GameObject;
		character.transform.parent = cardView.transform;
		setAnimator (character.GetComponent<Animator> ());
		if (cardType == "Burpy") {
			burp = character.GetComponentInChildren<ParticleSystem>();
			burp.Stop ();
			burp.Clear ();
		}

	}

	public void Unflip(){
		state = CardState.Hidden;
		MarkDirty();
	}

	public void StartTwist(){
		StartCoroutine (SpecialTwist ());
	}
	public IEnumerator SpecialTwist(){
		float r = Random.Range (10f, 31f);
		yield return new WaitForSeconds (r);
		if(state == CardState.Hidden) {
			cardView.animation.Play ("SpecialTwist");
			//yield return new WaitForSeconds (r);
			StartTwist ();
		}
	}

	IEnumerator FadeTo(float aValue, float aTime)
	{
		float alpha = child.material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			foreach (Renderer r in rlist){
				r.material.color=newColor;
			}
			yield return 0;
		}
	}


	public void fadeOut(){
		if(CardType.Equals("Burpy")){
			anim.SetBool ("Ended",true);	
		}
		StartCoroutine (FadeTo(0.0f,0.2f));
	}

	void OnMouseDown(){

		for (int i=0; i<Playmat.GetPlaymat().cards.Count; i++) {
			Playmat.GetPlaymat().cards[i].cardTimer = 0f;
		}

		if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
			if (state == CardState.Hidden && Playmat.GetPlaymat ().NumberOfCardsFlipped != 2 && abletoflip) {
				Playmat.GetPlaymat ().NumberOfCardsFlipped ++;
				state = CardState.Flipped;
				MarkDirty ();
			}

		} 
		else {
			if (state == CardState.Hidden && Playmat.GetPlaymat ().NumberOfCardsFlipped != 3) {
				Playmat.GetPlaymat ().NumberOfCardsFlipped ++;
				state = CardState.Flipped;
				MarkDirty ();
			}
		}
	}

	void OnTouch(){

		for (int i=0; i<Playmat.GetPlaymat().cards.Count; i++) {
			Playmat.GetPlaymat().cards[i].cardTimer = 0f;
		}
		if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
			if (state == CardState.Hidden && Playmat.GetPlaymat ().NumberOfCardsFlipped != 2 && abletoflip) {
				Playmat.GetPlaymat ().NumberOfCardsFlipped ++;
				state = CardState.Flipped;
				MarkDirty ();
			}
		} 	
		else {
			if (state == CardState.Hidden && Playmat.GetPlaymat ().NumberOfCardsFlipped != 3) {
				Playmat.GetPlaymat ().NumberOfCardsFlipped ++;
				state = CardState.Flipped;
				MarkDirty ();
			}
		}
	}
}
