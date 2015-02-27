using UnityEngine;
using System.Collections;

public class Card : VersionedView{


	public enum CardState {Flipped,Hidden,Bombed}
	public CardState state = CardState.Hidden;

	public int index;
	public GameObject cardView;
	public string CardType;
	public float delay = 5.0f;
	public Card pair;
	public Renderer child;
	public Color faded;
	bool setTimer = false;
	float cardTimer;

	void Start (){
		child = GetComponentInChildren<Renderer>();
		faded = new Color (child.material.color.r, child.material.color.g, child.material.color.b, 0);
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
			}
		}
	}

	public IEnumerator StartCardFlip(){
		AnimationClip clip;
		clip = cardView.animation.GetClip("Card Flip");
		if (state == CardState.Flipped) {
			cardView.animation ["Card Flip"].speed = 1.0f;
			cardView.animation.Play();
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_flip", transform.position);

			yield return new WaitForSeconds (clip.length);

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
		cardView.renderer.material.mainTexture = Resources.Load("Graphics/" +CardType)as Texture2D;

	}

	public void Unflip(){

		state = CardState.Hidden;
		MarkDirty();

	}

	IEnumerator FadeTo(float aValue, float aTime)
	{
		float alpha = child.material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			child.material.color = newColor;
			yield return 0;
		}
	}


	public void fadeOut(){
		StartCoroutine (FadeTo(0.0f,0.2f));
	}

	void OnMouseDown(){

		for (int i=0; i<Playmat.GetPlaymat().cards.Count; i++) {
			Playmat.GetPlaymat().cards[i].cardTimer = 0f;
		}
		if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
			if (state == CardState.Hidden && Playmat.GetPlaymat ().NumberOfCardsFlipped != 2) {
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
		if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
			if (state == CardState.Hidden && Playmat.GetPlaymat ().NumberOfCardsFlipped != 2) {
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
