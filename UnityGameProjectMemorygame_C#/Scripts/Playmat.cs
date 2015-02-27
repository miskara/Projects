using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Playmat : VersionedView{
	
	public char[] delimiter={'b','y'};
	public string[] xy;
	public UI ui;
	public bool gameWon = false;
	public int TotalPoints =0;
	public float Points = 0;
	public int NumberOfCardsFlipped =0;
	public int last,x,y;
	public enum BoardState {Flipping,Comparing}
	public BoardState state = BoardState.Flipping; 
	public List<List<Card>> pairMatrix = new List<List<Card>>();
	private static Playmat mat;
	public GameObject cardPrefab;
	public Layout layout;
	public GameObject board;
	public List<Card> cards = new List<Card>();
	public List<Card> all = new List<Card> ();
	public Card card1;
	public Card card2;
	public Card card3;
	// Use this for initialization
	void Start (){
		GameSettings.Instance().difficulty = GameSettings.GameDifficulty.Easy;
		last = 0;
		ui = GameObject.FindGameObjectWithTag ("UI").GetComponent<UI> ();
		mat = this;
		xy = board.name.Split (delimiter);
		int.TryParse (xy [0], out x);
		int.TryParse (xy [2], out y);
	}
	
	public static Playmat GetPlaymat(){
		return mat;
	}
	
	public void CreateLayout(string s){
		board = layout.GetLayoutFromName(s);
		CreateCardsFromLayout();
		CreateCardTypes();
	}
	
	void CreateCardsFromLayout(){
		int i = 0;
		foreach(Slot s in layout.slots){
			Vector3  strans = s.transform.position;
			GameObject go = GameObject.Instantiate(cardPrefab,strans,Quaternion.Euler(0,-90,270)) as GameObject;
			go.transform.parent = s.transform;
			Destroy(s.GetComponent<BoxCollider>());
			cards.Add(go.GetComponent<Card>());
			all.Add(go.GetComponent<Card>());
			go.GetComponent<Card>().index=i;
			i++;
		}
		
		cards = shuffleCards (cards);
		cards = shuffleCards (cards);
		cards = shuffleCards (cards);

		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_shuffle", transform.position);
	}

	public void shuffleSlots(List<Slot> alpha){
		for(int i = 0; i < alpha.Count; i++) {
			Slot tempslot = alpha[i];
			int randomIndex = Random.Range(0, alpha.Count);
			alpha[i] = alpha[randomIndex];
			alpha[randomIndex]= tempslot;
		}
	}

	public List<Card> SurroundingCards (List<Card> sorted, int i){

		List<Card> surrounding = new List<Card> ();
		foreach (Card c in sorted) {
			int j = c.GetComponent<Card> ().index;
			if (i == 0) {														//topleft corner
				//i+1, i+x, i+(x+1)
				if (j == (i + 1) || j == (i + x) || j == (i + (x + 1))) surrounding.Add (c);

			} else if (i / x < 1 && i != x - 1 && i != 0) {						//top row
				//i-1, i+1, i+(x-1), i+x, i+(x+1)
				if (j == i - 1 || j == i + 1 || j == i + (x - 1) || j == i + x || j == i + x + 1) surrounding.Add (c);	

			} else if (i == (x - 1)) {											//topright corner
				// i-1, i+(x-1), i+x
				if (j == i - 1 || j == i + (x + 1) || j == i + x) surrounding.Add (c);	 

			} else if (i % x == 0 && i != 0 && i != (x * y) - x) {				//left column
				// i-x, i-(x-1), i+1, i+x, i+(x+1)
				if (j == i - x || j == i - (x - 1) || j == i + 1 || j == i + x || j == i + (x + 1))	surrounding.Add (c);

			} else if (i % x == x - 1 && i != x - 1 && i != (x * y) - 1) {		//right column
				// i-(x+1), i-x, i-1, i+(x-1), i+x
				if (j == i - (x + 1) || j == i - x || j == i - 1 || j == i + (x - 1) || j == i + x) surrounding.Add (c);	 

			} else if (i == x * y - x) {											//bottomleft corner
				// i-x, i-(x-1), i+1
				if (j == i - x || j == i - (x - 1) || j == i + 1) surrounding.Add (c);	 

			} else if (i / x >= y - 1 && i != (x * y) - x && i != (x * y) - 1) {	//bottom row;
				// i-(x+1), i-x, i-(x-1), i-1, i+1
				if (j == i - (x + 1) || j == i - x || j == i - (x - 1) || j == i - 1 || j == i + 1) surrounding.Add (c);	 

			} else if (i == x * y - 1) {											//bottomright corner
				// i-(x+1), i-x, i-1
				if (j == i - (x + 1) || j == i - x || j == i - 1) surrounding.Add (c); 

			} else {																//default
				if (j == i - (x + 1) || j == i - x || j == i - (x - 1) || j == i - 1 || j == i + 1 || j == i + (x - 1) || j == i + x || j == i + (x + 1)) surrounding.Add (c);

			}
		//}
	}

		return surrounding;
	}


	public IEnumerator CandyFound(List<Card> alpha, int i){

		List<Card> surrounding = SurroundingCards (alpha,i);

		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/fruit_open", transform.position);

		foreach (Card c in surrounding) {;
				c.cardView.animation ["HideFast"].speed = -3.0f;
				c.cardView.animation.Play("HideFast");
				c.state=Card.CardState.Flipped;
		}
		yield return new WaitForSeconds (3);
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/fruit_used", transform.position);
		foreach (Card c in surrounding) {
			if(c.pair==null){
			c.cardView.animation ["HideFast"].speed = 3.0f;
			c.cardView.animation.Play("HideFast");
			c.state=Card.CardState.Hidden;
			}
		}
		card1 = null;
		card2 = null;
		card3 = null;
		state=BoardState.Flipping;
	}

	public void BurpyFound(List<Card> alpha, int i){ // work in progress burpycardfound
		List<Card> surrounding = SurroundingCards (alpha,i);

		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/burpy_burp", transform.position);
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_fly", transform.position);

		foreach (Card c in surrounding) {
				if(c.state == Card.CardState.Flipped){
				c.cardView.animation ["HideFast"].speed = 3.0f;
				c.cardView.animation.Play("HideFast");
				c.state=Card.CardState.Hidden;

				if(c.pair != null){
					Points -= 1.0f;
					c.pair.cardView.animation ["HideFast"].speed = 3.0f;
					c.pair.cardView.animation.Play("HideFast");
					c.pair.state=Card.CardState.Hidden;
					c.pair.pair = null;
					c.pair = null;
				}
			}
		}

		foreach (Card c in cards){
			if(c.state == Card.CardState.Flipped && c.pair == null){
				c.cardView.animation ["HideFast"].speed = 3.0f;
				c.cardView.animation.Play("HideFast");
				c.state=Card.CardState.Hidden;
			}
		}
		shuffleCards (surrounding);
		state=BoardState.Flipping;
	}

	public List<Card> shuffleCards(List<Card> alpha){
		for(int i = 0; i < alpha.Count; i++) {
			Card tempcard = alpha[i];
			int randomIndex = Random.Range(0, alpha.Count);
			alpha[i] = alpha[randomIndex];
			alpha[randomIndex] = tempcard;
		}
		
		return alpha;
	}
	
	public void BombFound(List<Card> alpha){
		foreach (Card c in alpha) {
			if(c.state==Card.CardState.Flipped){
				c.cardView.animation ["HideFast"].speed = 1.0f;
				c.cardView.animation.Play("HideFast");
				c.state=Card.CardState.Hidden;
			}
		}
		
		card1=null;
		card2=null;
		card3=null;
		shuffleCards (cards);
		state=BoardState.Flipping;
	}
	
	
	void CreateCardTypes(){
		if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
			
			for (int i=0; i < cards.Count/2-1; i++) {
				Card c1 = cards [i*2];
				Card c2 = cards [(i*2)+1];
				string type = GameSettings.Instance ().GetRandomType ();
				c1.GenerateCard (type);
				c2.GenerateCard (type);
				last = (i*2)+1;
			}
			
			Card b1 = cards[last+1];
			Card b2 = cards[last+2];
			b1.GenerateCard ("Bomb");
			b2.GenerateCard ("Bomb");
			TotalPoints = cards.Count/2-1;
		} 
		
		else {
			for (int i=0; i < cards.Count/3-1; i++) {
				Card c1 = cards [i*3];
				Card c2 = cards [i*3+1];
				Card c3 = cards [i*3+2]; 
				string type = GameSettings.Instance ().GetRandomType ();
				c1.GenerateCard (type);
				c2.GenerateCard (type);
				c3.GenerateCard (type);
			}
			Card b1 = cards[cards.Count];
			Card b2 = cards[cards.Count-1];
			Card b3 = cards[cards.Count-2];
			b1.GenerateCard ("Bomb");
			b2.GenerateCard ("Bomb");
			b3.GenerateCard ("Bomb");
			TotalPoints = cards.Count / 3-1;
		}
		
	}
	public string GetPointsString(){
		
		return Points.ToString() + "/" + TotalPoints.ToString();
	}

	public void goThroughLists(){
		for (int i =0; i<this.pairMatrix.Count; i++) {
			for (int j =0; j<this.pairMatrix[i].Count; j++) {
				Debug.Log ("Pair "+(i+1)+" Card "+(j+1)+" Type:");
				Debug.Log (this.pairMatrix [i] [j].CardType);
				Debug.Log ("Pair "+(i+1)+" Card "+(j+1)+" Index:");
				Debug.Log (this.pairMatrix [i] [j].index);
			}
		}
	}
	
	public override void DirtyUpdate (){
		
		switch(state){
		case BoardState.Comparing:
			Compare();
			break;
		}
	}
	
	
	
	void Compare(){
		
		if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
			if (card1.CardType == card2.CardType) {

				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_match", transform.position);

				NumberOfCardsFlipped = 0;
				Points++;
				List<Card> pair = new List<Card>();
				pair.Add (card1);
				pair.Add (card2);
				pairMatrix.Add(pair);
				card1.setPair (card2);
				card2.setPair (card1);
				if (TotalPoints == Points) {
					gameWon = true;
					FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/win", transform.position);
					if(PlayerPrefs.GetInt("Levels") == ui.currLevel){
						GameController.levels = ui.level+1;
						PlayerPrefs.SetInt ("levels",GameController.levels);
					}
				}
			} 
			else {
				card1.Unflip ();
				card2.Unflip ();
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_no_match", transform.position);

			}
			card1 = null;
			card2 = null;
			state = BoardState.Flipping;
		} 
		
		else {
			
			if (card1.CardType == card2.CardType && card1.CardType == card3.CardType) {

				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_match", transform.position);

				NumberOfCardsFlipped = 0;
				Points++;
				if (TotalPoints == Points) {
					gameWon = true;
					GameController.levels +=1;
				}
			} 
			
			else {
				card1.Unflip ();
				card2.Unflip ();
				card3.Unflip ();
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/card_no_match", transform.position);
			}
			card1 = null;
			card2 = null;
			card3 = null;
			state = BoardState.Flipping;
			
		}
	}
	
	public void SetCardsForMatch(Card c){
		int i;
		switch(c.CardType){
		case "Bomb":
			NumberOfCardsFlipped=0;
			i = c.GetComponent<Card>().index;
			cards.Remove (c);
			//BurpyFound(cards, i);
			StartCoroutine(CandyFound(cards, i));
			state = BoardState.Flipping;
			card1 = null;
			card2 = null;
			card3 = null;
			break;
		case "Candy":
			NumberOfCardsFlipped=0;
			i = c.GetComponent<Card>().index;
			cards.Remove (c);
			StartCoroutine(CandyFound(cards, i));
			state = BoardState.Flipping;
			card1 = null;
			card2 = null;
			card3 = null;
			break;

		default:
			if (GameSettings.Instance ().difficulty == GameSettings.GameDifficulty.Easy) {
				if (card1 == null) {
					card1 = c;
					state = BoardState.Flipping;
				} 
				else {
					card2 = c;
					state = BoardState.Comparing;
				}
				
				MarkDirty ();
				
			} 
			else {
				if (card1 == null) {
					card1 = c;
					state = BoardState.Flipping;
				} 
				else if (card2 == null) {
					card2 = c;
					state = BoardState.Flipping;
				} 
				else {
					card3 = c;
					state = BoardState.Comparing;
				}
				MarkDirty ();
			}
			break;
		}
	}
}
